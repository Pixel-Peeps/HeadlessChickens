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
        void Start()
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
                    PhotonNetwork.Instantiate(this.playerPrefab.name, new Vector3(0f, 5f, 0f), Quaternion.identity, 0);
                }
                else
                {
                    Debug.LogFormat("Ignoring scene load for {0}", SceneManagerHelper.ActiveSceneName);
                }
            }
        }

        public void DeterminePlayerRole()
        {
            if (PlayerAssignment.Instance.foxPlayer == PhotonNetwork.LocalPlayer)
            {
                playerPrefab = foxPrefab;
                spawnPos = foxSpawnPoint;
            }
            else
            {
                playerPrefab = chickPrefab;
                spawnPos = chickSpawnPoints[PhotonNetwork.LocalPlayer.ActorNumber];
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