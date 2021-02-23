using System;
using System.Collections.Generic;
using System.Linq;
using com.pixelpeeps.headlesschickens;
using Photon.Pun;
using Photon.Realtime;
using PixelPeeps.HeadlessChickens.GameState;
using PixelPeeps.HeadlessChickens.UI;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace PixelPeeps.HeadlessChickens.Network
{
    public class NewGameManager : MonoBehaviourPunCallbacks, IPunObservable
    {
        private static NewGameManager _instance;
        
        public static NewGameManager Instance
        {
            get => _instance;
            set => _instance = value;
        }

        [Header("Fox")]
        public GameObject foxPrefab;
        public Transform foxSpawnPoint;
        
        [Header("Chickens")]
        public GameObject chickPrefab;
        public List<Transform> chickSpawnPoints;
        
        
        private GameObject playerPrefab; // The prefab this player uses. Assigned as fox or chick when roles are assigned
        private Transform spawnPos;

        [Header("Game State")]
        public int chickensCaught = 0;
        public int chickensEscaped = 0;
        public int chickenThreshold = 2;

        [Header("HidingSpot")]
        [SerializeField] public List<Transform> hidingSpotSpawnPos;

        public GameObject hidingSpotPrefab;

        [Header("Rooms")]
        [SerializeField] public List<RoomTile> rooms;

        [Header("Levers")]
        public GameObject leverSpotPrefab;
        public int maxNumberOfLevers;

        [Header("Exits")]
        [SerializeField] public List<ExitDoor> exits;
        public float exitTime;
        
        void Awake()
        {
            if (_instance != null && _instance != this)
            {
                Destroy(gameObject);
            }
            else
            {
                _instance = this;
            }
        }

        #region Initialisation 
        
        public void DeterminePlayerRole()
        {
            Debug.Log("DeterminePlayerRole");
            Player localPlayer = PhotonNetwork.LocalPlayer;
            PlayerAssignmentRPC assignmentRPC = PlayerAssignmentRPC.Instance;
            
            // Turning array to list with LINQ
            List<Player> chickenPlayersList = 
                assignmentRPC.chickenPlayersActorNumbers.Select(i => PhotonNetwork.CurrentRoom.GetPlayer(i)).ToList();

            if (Equals(assignmentRPC.foxPlayer, localPlayer))
            {
                playerPrefab = foxPrefab;
                spawnPos = foxSpawnPoint;
            }
            else
            {
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
            }
        }
        
        public void Initialise()
        {
            // if (playerPrefab == null)
            // {
            //     Debug.LogError("<Color=Red><a>Missing</a></Color> playerPrefab Reference. Please set it up in GameObject 'NewGameManager'",this);
            // }
            // else
            // {
            //     Debug.LogFormat("We are Instantiating LocalPlayer from {0}", SceneManager.GetActiveScene());
            //     if (PlayerManager.LocalPlayerInstance == null)
            //     {
            //         Debug.LogFormat("We are Instantiating LocalPlayer from {0}", SceneManagerHelper.ActiveSceneName);
            //         PhotonNetwork.Instantiate(playerPrefab.name, spawnPos.position, Quaternion.identity, 0);
            //     }
            //     else
            //     {
            //         Debug.LogFormat("Ignoring scene load for {0}", SceneManagerHelper.ActiveSceneName);
            //     }
            // }
            
            SpawnPlayers();

            // foreach (var hidingSpawnPos in hidingSpotSpawnPos)
            // {
            //     PhotonNetwork.InstantiateRoomObject(hidingSpotPrefab.name, hidingSpawnPos.position,
            //         Quaternion.identity, 0);
            // }
            
            SpawnHidingSpots();
            
            // maxNumberOfLevers = PhotonNetwork.CurrentRoom.PlayerCount;
            // HUDManager.Instance.UpdateLeverCount(0);
            //
            // List<RoomTile> tempRooms = rooms;
            // for (int i = 0; i < maxNumberOfLevers; i++)
            // {
            //     // Get random room from list
            //     int roomNumber = UnityEngine.Random.Range(0, tempRooms.Count);
            //
            //     RoomTile room = tempRooms[roomNumber];
            //
            //     // Get random lever from room
            //     int leverNumber = UnityEngine.Random.Range(0, room.leverPositions.Count);
            //     Transform lever = room.leverPositions[leverNumber];
            //
            //     PhotonNetwork.InstantiateRoomObject(leverSpotPrefab.name, lever.position,
            //         lever.rotation, 0);
            //
            //     tempRooms.RemoveAt(roomNumber);
            //     
            // }
            
            SpawnLevers();
            
            StartTimer();
        }

        private void SpawnPlayers()
        {

            if(chickensEscaped >= chickenThreshold)

                if (playerPrefab == null)

                {
                    Debug.LogError("<Color=Red><a>Missing</a></Color> playerPrefab Reference. Please set it up in GameObject 'NewGameManager'",this);
                }
            
                else
                {    
                    PhotonNetwork.Instantiate(playerPrefab.name, spawnPos.position, Quaternion.identity, 0);
                }
        }

        private void SpawnHidingSpots()
        {
            foreach (var hidingSpawnPos in hidingSpotSpawnPos)
            {
                PhotonNetwork.InstantiateRoomObject(hidingSpotPrefab.name, hidingSpawnPos.position,
                    Quaternion.identity, 0);
            }
        }

        private void SpawnLevers()
        {
            maxNumberOfLevers = PhotonNetwork.CurrentRoom.PlayerCount;
            HUDManager.Instance.UpdateLeverCount(0);

            List<RoomTile> tempRooms = rooms;
            for (int i = 0; i < maxNumberOfLevers; i++)
            {
                // Get random room from list
                int roomNumber = UnityEngine.Random.Range(0, tempRooms.Count);

                RoomTile room = tempRooms[roomNumber];

                // Get random lever from room
                int leverNumber = UnityEngine.Random.Range(0, room.leverPositions.Count);
                Transform lever = room.leverPositions[leverNumber];

                PhotonNetwork.InstantiateRoomObject(leverSpotPrefab.name, lever.position,
                    lever.rotation, 0);

                tempRooms.RemoveAt(roomNumber);

            }
        }

        #endregion
        
        public override void OnLeftRoom()
        {
            GameStateManager.Instance.SwitchGameState(new MainMenuState());
        }

        public void LeaveRoom()
        {
            PhotonNetwork.LeaveRoom();
        }

        #region Timer

        public float totalGameTime;
        
        private bool timerIsRunning;
        private float timeRemaining;
        
        void StartTimer()
        {
            timeRemaining = totalGameTime;
            timerIsRunning = true;
        }
        
        void Update()
        {
            if (timerIsRunning)
            {
                if (timeRemaining > 0)
                {
                    timeRemaining -= Time.deltaTime;
                    HUDManager.Instance.UpdateTimeDisplay(timeRemaining);
                }

                else
                {
                    timerIsRunning = false;
                    timeRemaining = 0;
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
}