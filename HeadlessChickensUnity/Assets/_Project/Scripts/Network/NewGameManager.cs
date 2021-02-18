using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using com.pixelpeeps.headlesschickens;
using Photon.Pun;
using Photon.Realtime;
using PixelPeeps.HeadlessChickens.GameState;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace PixelPeeps.HeadlessChickens.Network
{
    public class NewGameManager : MonoBehaviourPunCallbacks
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

        public int leversPulled = 0;
        public bool allLeversPulled = false;
    
    
        void Update()
        {
            // if all levels are active open the exit
            if(leversPulled == 4)
            {
                photonView.RPC(" RPC_AllLeversPulled", RpcTarget.AllBufferedViaServer);
            }
        }

        [PunRPC]
        public void RPC_AllLeversPulled()
        {
            Debug.Log("all levers pulled!");
            allLeversPulled = true;
        }

        [PunRPC]
        public void RPC_IncrementLeverCount()
        {
            leversPulled++;
        }

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
        public void Initialise()
        {
            if (playerPrefab == null)
            {
                Debug.LogError("<Color=Red><a>Missing</a></Color> playerPrefab Reference. Please set it up in GameObject 'NewGameManager'",this);
            }
            else
            {
                Debug.LogFormat("We are Instantiating LocalPlayer from {0}", SceneManager.GetActiveScene());
                if (PlayerManager.LocalPlayerInstance == null)
                {
                    Debug.LogFormat("We are Instantiating LocalPlayer from {0}", SceneManagerHelper.ActiveSceneName);
                    PhotonNetwork.Instantiate(playerPrefab.name, spawnPos.position, Quaternion.identity, 0);
                }
                else
                {
                    Debug.LogFormat("Ignoring scene load for {0}", SceneManagerHelper.ActiveSceneName);
                }
            }
        }

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
        
        public override void OnLeftRoom()
        {
            GameStateManager.Instance.SwitchGameState(new MainMenuState());
        }

        public void LeaveRoom()
        {
            PhotonNetwork.LeaveRoom();
        }
    }
}