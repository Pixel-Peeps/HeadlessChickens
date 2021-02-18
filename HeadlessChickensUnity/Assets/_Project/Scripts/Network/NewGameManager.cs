using System.Collections.Generic;
using System.Diagnostics.Contracts;
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
            List<Player> chickenPlayersList = new List<Player>();
            
            // Turning array to list
            foreach (int i in assignmentRPC.chickenPlayersActorNumbers)
            {
                Player thisPlayer = PhotonNetwork.CurrentRoom.GetPlayer(i);
                chickenPlayersList.Add(thisPlayer);
            }
            
            if (Equals(assignmentRPC.foxPlayer, localPlayer))
            {
                playerPrefab = foxPrefab;
                spawnPos = foxSpawnPoint;
            }
            else
            {
                playerPrefab = chickPrefab;
                
                // Find index of this player 
                int indexInChickenList = chickenPlayersList.FindIndex(x => x.NickName == localPlayer.NickName);
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