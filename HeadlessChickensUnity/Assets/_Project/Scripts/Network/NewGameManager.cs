using System;
using System.Collections.Generic;
using System.Linq;
using Photon.Pun;
using Photon.Realtime;
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
        
        private GameObject playerPrefab; // The prefab this player uses. Assigned as fox or chick when roles are assigned
        private Transform spawnPos;

        [Header("Game State")]
        public int chickensCaught = 0;
        public int chickensEscaped = 0;
        public int chickenEscapeThreshold = 2;

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
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
            }
            else
            {
                Instance = this;
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
            SpawnPlayers();
            
            SpawnHidingSpots();
            
            SpawnLevers();
            
            StartTimer();
            
            NetworkManager.Instance.GameSetupComplete();
        }

        public void CheckForFinish()
        {
            if (chickensCaught + chickensEscaped == PhotonNetwork.CurrentRoom.PlayerCount - 1)
            {
                DetermineWinner();
            }
        }

        public void DetermineWinner()
        {
            if (chickensEscaped >= chickenEscapeThreshold)
            {
                // Show Chickens Win Screen / Fox Lose Screen
                Debug.Log("Chickens Wins!");
            }
            else
            {
                // Show Fox Win Screen / Chickens Lose Screen
                Debug.Log("Fox Wins!");
            }
        }

        private void SpawnPlayers()
        {
            if (playerPrefab == null)
            {
                Debug.LogError("<Color=Red><a>Missing</a></Color> playerPrefab Reference. Please set it up in GameObject 'NewGameManager'",this);
            }
            
            else
            {    
                PhotonNetwork.Instantiate(playerPrefab.name, spawnPos.position, Quaternion.identity);
            }
        }

        private void SpawnHidingSpots()
        {
            //foreach (Transform hidingSpawnPos in hidingSpotSpawnPos)
            //{
            //    PhotonNetwork.InstantiateRoomObject(hidingSpotPrefab.name, hidingSpawnPos.position,
            //        hidingSpawnPos.rotation);
            //}
        }

        private void SpawnLevers()
        {
            maxNumberOfLevers = PhotonNetwork.CurrentRoom.PlayerCount;
            HUDManager.Instance.UpdateLeverCount(0);

            List<RoomTile> tempRooms = rooms;

            // If a room has no levers, remove it from the list

            foreach (RoomTile room in tempRooms.ToList())
            {
                if(!room.leverPositions.Any())
                {
                    tempRooms.Remove(room);
                    continue;
                }
            }

            for (int i = 0; i < maxNumberOfLevers; i++)
            {
                // Get random room from list
                int roomNumber = UnityEngine.Random.Range(0, tempRooms.Count);

                RoomTile room = tempRooms[roomNumber];

                // Get random lever from room
                int leverNumber = UnityEngine.Random.Range(0, room.leverPositions.Count);
                GameObject lever = room.leverPositions[leverNumber].gameObject;

                lever.transform.GetChild(0).gameObject.SetActive(true);
                //PhotonNetwork.InstantiateRoomObject(leverSpotPrefab.name, lever.position,
                //    lever.rotation);

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

        [Header("Timer")]
        public float totalGameTime;
        
        private bool timerIsRunning;
        private float timeRemaining;

        private void StartTimer()
        {
            timeRemaining = totalGameTime;
            timerIsRunning = true;
        }

        private void Update()
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