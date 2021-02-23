using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using PixelPeeps.HeadlessChickens.GameState;
using PixelPeeps.HeadlessChickens.UI;
using UnityEngine;
using UnityEngine.UI;

namespace PixelPeeps.HeadlessChickens.Network
{
    public class NetworkManager : MonoBehaviourPunCallbacks
    {
        public static NetworkManager Instance { get; private set; }

        private UIManager uiManager;

        public bool gameIsRunning;

        private const int minPlayersPerRoom = 1;

        private string currentRoomName;

        private void Awake()
        {
            DontDestroyOnLoad(gameObject);
            
            if (Instance != null && Instance != this)    
            {
                Destroy(gameObject);
            }
            else
            {    
                Instance = this;
            }
            
            PhotonNetwork.AutomaticallySyncScene = true;
        }

        private void Start()
        {
            uiManager = GameObject.FindGameObjectWithTag("MenuManager").GetComponent<UIManager>();
            PhotonNetwork.ConnectUsingSettings();
            
            GameStateManager.Instance.ShowConnectingScreen();
        }

        #region Photon Callback Methods
        public override void OnConnectedToMaster()
        {
            GameStateManager.Instance.HideConnectingScreen();
            
            Debug.Log("Connected to master");
            PhotonNetwork.JoinLobby();
            
            GameStateManager.Instance.ShowConnectingScreen();
            
            PhotonNetwork.AutomaticallySyncScene = true;
        }

        public override void OnJoinedLobby()
        {
            GameStateManager.Instance.HideConnectingScreen();
            
            Debug.Log("Joined lobby");
            GameStateManager.Instance.SwitchGameState(new MainMenuState());
        }

        public void CreateRoom()
        {
            string roomName = uiManager.createRoom.GetInputFieldText();
            
            uiManager.createRoom.HideErrorMessage();    
            
            currentRoomName = roomName;

            if (!string.IsNullOrEmpty(roomName))
            {
                RoomOptions options = new RoomOptions
                {
                    MaxPlayers = 6, 
                    EmptyRoomTtl = 0
                };

                PhotonNetwork.CreateRoom(roomName, options);
                
                GameStateManager.Instance.ShowConnectingScreen();
            }
            
            if (string.IsNullOrEmpty(roomName))
            {
                uiManager.createRoom.DisplayErrorMessage();
            }
        }

        public override void OnCreateRoomFailed(short returnCode, string message)
        {
            GameStateManager.Instance.ShowErrorScreen();
        }
        
        public void LeaveRoom()
        {
            if (gameIsRunning)
            {
                return;
            }
            
            PhotonNetwork.LeaveRoom();
            GameStateManager.Instance.ShowLoadingScreen();
        }

        public override void OnLeftRoom()
        {
            if (gameIsRunning)
            {
                return;
            }
            
            Debug.Log("Left room: " + currentRoomName);
            GameStateManager.Instance.HideLoadingScreen();
            GameStateManager.Instance.SwitchGameState(new MainMenuState());
        }

        public void JoinRoom(RoomInfo info)
        {            
            PhotonNetwork.JoinRoom(info.Name);

            PhotonNetwork.NickName = GetPlayerNickname();

            GameStateManager.Instance.ShowConnectingScreen();
        }

        public override void OnJoinedRoom()
        {
            Room room = PhotonNetwork.CurrentRoom;
            
            PlayerAssignmentRPC.Instance.chickenPlayersActorNumbers = new int[room.PlayerCount - 1];
            
            Debug.Log("Room " + room.Name + " has been joined. It currently has " + room.PlayerCount + "/" + room.MaxPlayers + 
                      " players, isVisible is set to: " + room.IsVisible + " and isOpen is set to: " + room.IsOpen);

            GameStateManager.Instance.HideConnectingScreen();
            GameStateManager.Instance.SwitchGameState(new WaitingRoomState());

            uiManager.roomNameText.text = room.Name;
            
            uiManager.startGameButton.SetActive(PhotonNetwork.IsMasterClient); // Sets button active only for the host
            
            uiManager.startGameButton.GetComponent<Button>().interactable =
                PhotonNetwork.CurrentRoom.PlayerCount >= minPlayersPerRoom;

            uiManager.roomPlayerCount.text = string.Format("In Room: {0} / {1}", PhotonNetwork.CurrentRoom.PlayerCount, PhotonNetwork.CurrentRoom.MaxPlayers);
            
            Player[] players = PhotonNetwork.PlayerList;

            uiManager.playerList.DestroyCurrentList();
            
            foreach (Player t in players)
            {
                uiManager.UpdatePlayerList(t);
            }
            
            Debug.Log("Joined room: " + currentRoomName);
        }

        public override void OnRoomListUpdate(List<RoomInfo> roomList)
        {
            Debug.Log("RoomListUpdate");
            uiManager.GenerateRoomList(roomList);
        }

        public override void OnPlayerEnteredRoom(Player newPlayer)
        {
            uiManager.roomPlayerCount.text = string.Format("In Room: {0} / {1}", PhotonNetwork.CurrentRoom.PlayerCount, PhotonNetwork.CurrentRoom.MaxPlayers);
            Debug.Log("OnPlayerEnteredRoom");
            
            uiManager.startGameButton.SetActive(PhotonNetwork.IsMasterClient); // Sets button active only for the host
            
            if (PhotonNetwork.CurrentRoom.PlayerCount >= minPlayersPerRoom)
            {
                uiManager.startGameButton.GetComponent<Button>().interactable = true;
            }
            
            uiManager.UpdatePlayerList(newPlayer);
        }

        public override void OnPlayerLeftRoom(Player otherPlayer)
        {
            if (gameIsRunning)
            {
                return;
            }
            
            uiManager.roomPlayerCount.text = string.Format("In Room: {0} / {1}", PhotonNetwork.CurrentRoom.PlayerCount, PhotonNetwork.CurrentRoom.MaxPlayers);
            
            uiManager.startGameButton.SetActive(PhotonNetwork.IsMasterClient); // Sets button active only for the host
            
            if (PhotonNetwork.CurrentRoom.PlayerCount < minPlayersPerRoom)
            {
                uiManager.startGameButton.GetComponent<Button>().interactable = false;
            }
        }
        
        #endregion
        
        public void StartGameOnMaster()
        {
            PhotonNetwork.CurrentRoom.IsOpen = false;
            PhotonNetwork.CurrentRoom.IsVisible = false;
            
            GameStateManager.Instance.SwitchGameState(new GameSceneState());
            PlayerAssignmentRPC.Instance.AssignPlayerRoles();

            photonView.RPC("StartGameForOthers", RpcTarget.Others);
            
            GameSetupBegin();
        }
        
        
        // ReSharper disable once UnusedMember.Global
        [PunRPC]
        public void StartGameForOthers()
        {            
            GameStateManager.Instance.SwitchGameState(new GameSceneState());
            
            GameSetupBegin();
        }

        private void GameSetupBegin()
        {
            gameIsRunning = true;
            GameStateManager.Instance.ShowSetupScreen();
        }

        public void GameSetupComplete()
        {
            GameStateManager.Instance.HideSetupScreen();
        }

        private string GetPlayerNickname()
        {
            return uiManager.mainMenu.GetInputFieldText();
        }
    }
}
