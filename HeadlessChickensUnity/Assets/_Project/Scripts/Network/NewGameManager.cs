using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Photon.Pun;
using Photon.Realtime;
using PixelPeeps.HeadlessChickens._Project.Scripts.Character;
using PixelPeeps.HeadlessChickens.GameState;
using PixelPeeps.HeadlessChickens.UI;
using UnityEngine;

namespace PixelPeeps.HeadlessChickens.Network
{
    public class NewGameManager : MonoBehaviourPunCallbacks, IPunObservable
    {
        public static NewGameManager Instance { get; private set; }

        [Header("Fox")]
        public GameObject foxPrefab;
        public Transform foxSpawnPoint;
        
        [Header("Chickens")]
        public GameObject chickPrefab;
        public List<Transform> chickSpawnPoints;

        [HideInInspector] public PlayerType myType;
        
        private GameObject playerPrefab; // The prefab this player uses. Assigned as fox or chick when roles are assigned
        private Transform spawnPos;

        [Header("Game State")]
        //public int chickensCaught = 0;
        //public int chickensEscaped = 0;
        public int chickenEscapeThreshold = 2;

        public ChickenManager chickenManager;

        [Header("Players and Controllers")] 
        public GameObject myController;
        public ChickenBehaviour localChickBehaviour;

        [Header("Hiding Spot")]
        [SerializeField] public List<Transform> hidingSpotSpawnPos;

        public GameObject hidingSpotPrefab;

        [Header("Rooms")]
        [SerializeField] public List<RoomTile> rooms;

        [Header("Levers")]
        public GameObject leverSpotPrefab;
        public int maxNumberOfLevers;
        [SerializeField] public List<RoomTile> inactiveLevers;

        [Header("Exits")]
        [SerializeField] public List<ExitDoor> exits;
        public float exitTime;
        
        [Header("Trap Pick-Ups")]
        [SerializeField] public List<Transform> trapSpawnPos;
        public GameObject trapPickUpPrefab;
        public bool canBeFakeLever;

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
            }
            else
            {
                Instance = this;
            }

            canBeFakeLever = true;
        }

        #region Initialisation 
        
        public void DeterminePlayerRole()
        {
            print("<color=magenta> Determining player role </color>");
            Player localPlayer = PhotonNetwork.LocalPlayer;
            PlayerAssignmentRPC assignmentRPC = PlayerAssignmentRPC.Instance;
            
            // Turning array to list with LINQ
            List<Player> chickenPlayersList = 
                assignmentRPC.chickenPlayersActorNumbers.Select(i => PhotonNetwork.CurrentRoom.GetPlayer(i)).ToList();

            if (Equals(assignmentRPC.foxPlayer, localPlayer))
            {
                print("<color=magenta> Determining type for fox </color>");
                playerPrefab = foxPrefab;
                spawnPos = foxSpawnPoint;
                myType = PlayerType.Fox;
                print("<color=magenta> MyType: " + myType + "</color>");
            }
            else
            {
                print("<color=magenta> Determining type for chicken </color>");
                playerPrefab = chickPrefab;
                int indexInChickenList;
                // Find index of this player 
                try
                {
                    indexInChickenList = chickenPlayersList.FindIndex(x => Equals(x, localPlayer));
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    throw;
                }
                Debug.Log("index: " + indexInChickenList);
                spawnPos = chickSpawnPoints[indexInChickenList];
                myType = PlayerType.Chick;
                print("<color=magenta> MyType: " + myType + "</color>");
            }
        }
        
        public void Initialise()
        {
            //chickensCaught = 0;
            //chickensEscaped = 0;
            
            foxWinScreen.SetActive(false);
            foxLossScreen.SetActive(false);
            chickenWinScreen.SetActive(false);
            chickenLossScreen.SetActive(false);
            
            SpawnPlayers();

            SpawnHidingSpots();

            SpawnLevers();

            SpawnTrapPickUps();

            StartTimer();
            //LeverManager.Instance.Initialise();

            HUDManager.Instance.Initialise();
            Debug.Log("<color=magenta> initialised HUD</color>");
            
            NetworkManager.Instance.GameSetupComplete();
            
            HUDManager.Instance.DisplayObjectiveMessage(myType, 3f, "Catch all the chicks!", "Find all the levers!");
        }

        public void ExitDoorOpened()
        {
            HUDManager.Instance.DisplayObjectiveMessage(myType, 3f, "Prevent their escape!", "Rush for the exit!");
        }

        private void SpawnPlayers()
        {
            if (myController != null) return;
            
            if (playerPrefab == null)
            {
                Debug.LogError("<Color=Red><a>Missing</a></Color> playerPrefab Reference. Please set it up in GameObject 'NewGameManager'",this);
            }
            
            else
            {
                if (playerPrefab == null)
                {
                    CheckPlayerPrefab();
                }
                
                GameObject newController = PhotonNetwork.Instantiate(playerPrefab.name, spawnPos.position, Quaternion.identity);
                myController = newController;
                localChickBehaviour = newController.GetComponentInChildren<ChickenBehaviour>();
                Debug.Log("<color=magenta> Spawned a player </color>");
            }
        }

        private void CheckPlayerPrefab()
        {
            switch (myType)
            {
                case PlayerType.Fox:
                    playerPrefab = foxPrefab;
                    break;

                case PlayerType.Chick:
                    playerPrefab = chickPrefab;
                    break;
                
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void SpawnTrapPickUps()
        {
            //one for each player
            maxNumberOfLevers = PhotonNetwork.CurrentRoom.PlayerCount;

            List<Transform> tempPickupPos = trapSpawnPos;
            //for (int i = 0; i < maxNumberOfLevers; i++)
            foreach (Transform t in trapSpawnPos)
            {
                PhotonNetwork.InstantiateRoomObject(trapPickUpPrefab.name, t.position,
                    Quaternion.identity);
            }
            // {
            //     int trapNumber = UnityEngine.Random.Range(0, trapSpawnPos.Count);
            //     
            //     Debug.Log("Spawning trap pick ups!!");
            //     PhotonNetwork.InstantiateRoomObject(trapPickUpPrefab.name, trapSpawnPos[trapNumber].position,
            //         Quaternion.identity);
            //     Debug.Log("Spawned!?!");
            //     tempPickupPos.RemoveAt(trapNumber);
            // }
        }
        
        private void SpawnHidingSpots()
        {
            
            foreach (Transform hidingSpawnPos in hidingSpotSpawnPos)
            {
                PhotonNetwork.InstantiateRoomObject(hidingSpotPrefab.name, hidingSpawnPos.position,
                    hidingSpawnPos.rotation);
            }
        }

        private void SpawnLevers()
        {
            maxNumberOfLevers = PhotonNetwork.CurrentRoom.PlayerCount;

            if (!PhotonNetwork.IsMasterClient) return;

            List<RoomTile> tempRooms = rooms;

            // If a room has no levers, remove it from the list

            foreach (RoomTile room in tempRooms.ToList())
            {
                if(!room.leverPositions.Any())
                {
                    tempRooms.Remove(room);
                }
            }

            //send this to lever manager for later
            LeverManager.Instance.SetLeverPosList(tempRooms);
            

            for (int i = 0; i < maxNumberOfLevers; i++)
            {
                // Get random room from list
                int roomNumber = UnityEngine.Random.Range(0, tempRooms.Count);

                RoomTile room = tempRooms[roomNumber];

                // Get random lever from room
                int leverNumber = UnityEngine.Random.Range(0, room.leverPositions.Count);
                LeverHolder leverHolder = room.leverPositions[leverNumber];
                leverHolder.isChildFake = false;
                leverHolder.photonView.RPC("RPC_EnableLever", RpcTarget.AllBufferedViaServer);
                tempRooms.RemoveAt(roomNumber);

            }
        }

        #endregion

        #region Results and Ending Game

        [Header("Results Screens")]
        public GameObject foxWinScreen;
        public GameObject foxLossScreen;
        public GameObject chickenWinScreen;
        public GameObject chickenLossScreen;
        
        [Header("Return to Lobby")]
        public float lobbyReturnCountdown;

        public void EndGame()
        {
            if (!PhotonNetwork.IsMasterClient)
            {
                return;
            }
            
            photonView.RPC("EndGameRPC", RpcTarget.AllBuffered);
            
            StartCoroutine(LobbyReturnCountdownCoroutine());
        }

        public void CheckForFinish()
        {
            // Change to check the Active Chicks List
            //if (chickensCaught + chickensEscaped == PhotonNetwork.CurrentRoom.PlayerCount - 1)
            //{
            //    // DetermineWinner();
            //}

            if (!chickenManager.activeChicks.Any())
            {
                DetermineWinner();
            }
        }

        private void DetermineWinner()
        {
            if (chickenManager.escapedChicks.Count >= chickenEscapeThreshold)
            {
                photonView.RPC("ChickenWinRPC", RpcTarget.All);
            }
            else
            {
                photonView.RPC("FoxWinRPC", RpcTarget.All);
            }
        }


        // ReSharper disable once UnusedMember.Global
        [PunRPC]
        public void ChickenWinRPC()
        {
            switch (myType)
            {
                case PlayerType.Fox:
                    foxLossScreen.SetActive(true);
                    break;

                case PlayerType.Chick:
                    chickenWinScreen.SetActive(true);
                    break;

                default:
                    throw new ArgumentOutOfRangeException();
            }

            EndGame();
        }

        // ReSharper disable once UnusedMember.Global
        [PunRPC]
        public void FoxWinRPC()
        {
            switch (myType)
            {
                case PlayerType.Fox:
                    foxWinScreen.SetActive(true);
                    break;

                case PlayerType.Chick:
                    chickenLossScreen.SetActive(true);
                    break;

                default:
                    throw new ArgumentOutOfRangeException();
            }

            EndGame();
        }

        // ReSharper disable once UnusedMember.Global
        [PunRPC]
        public void EndGameRPC()
        {
            PhotonNetwork.Destroy(myController);
            NetworkManager.Instance.gameIsRunning = false;
        }
        
        private IEnumerator LobbyReturnCountdownCoroutine()
        {            
            yield return new WaitForSecondsRealtime(lobbyReturnCountdown);
            photonView.RPC("RestartGameRPC", RpcTarget.All); 
            //ReturnToMenu();
            NetworkManager.Instance.MakeRoomPublic();
        }
        
        // ReSharper disable once UnusedMember.Local
        [PunRPC]
        private void RestartGameRPC()
        {
            GameStateManager.Instance.SwitchGameState(new RestartGameState());  
        }
        
/*
        private void ReturnToMenu()
        {
            GameStateManager.Instance.SwitchGameState(new MainMenuState());
        }
*/

        #endregion

        #region Timer

        [Header("Timer")]
        public float totalGameTime;
        
        private bool timerIsRunning;
        private float timeRemaining = 10;

        private void StartTimer()
        {
            timeRemaining = 10f;
            totalGameTime = NetworkManager.Instance.gameTime;
            timeRemaining = totalGameTime;
            timerIsRunning = true;
            canBeFakeLever = true;
        }

        private void Update()
        {
            if (timerIsRunning)
            {
                if (timeRemaining < (totalGameTime *0.8))
                {
                    canBeFakeLever = false;
                }
                
                if (timeRemaining >= 0)
                {
                    timeRemaining -= Time.deltaTime;
                    HUDManager.Instance.UpdateTimeDisplay(timeRemaining);
                }
               
                else
                {
                    timerIsRunning = false;
                    timeRemaining = 0;
                    HUDManager.Instance.UpdateTimeDisplay(timeRemaining);
                    photonView.RPC("FoxWinRPC", RpcTarget.All);
                }
            }
        }
        
        #endregion
        
        #region Data Streaming

        public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
        {
            if (stream.IsWriting && PhotonNetwork.IsMasterClient)
            {
                stream.SendNext(timeRemaining);
            }
            else if (stream.IsReading)
            {
                this.timeRemaining = (float) stream.ReceiveNext();
                HUDManager.Instance.UpdateTimeDisplay(timeRemaining);
            }
        }

        #endregion
    }

    public enum PlayerType
    {
        Fox,
        Chick
    }
}