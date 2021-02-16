using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Photon.Pun;
using Photon.Realtime;
using PixelPeeps.HeadlessChickens.GameState;
using PixelPeeps.HeadlessChickens.UI;
using UnityEngine;
using UnityEngine.UIElements;

namespace PixelPeeps.HeadlessChickens.Network
{
    public class NetworkManager : MonoBehaviourPunCallbacks
    {
        private static NetworkManager _instance;
        public static NetworkManager Instance
        {
            get => _instance;
            set => _instance = value;
        }

        private UIManager uiManager;

        private string currentRoomName;

        private void Awake()
        {
            DontDestroyOnLoad(gameObject);
            
            if (_instance != null && _instance != this)    
            {
                Destroy(gameObject);
            }
            else
            {    
                _instance = this;
            }
        }

        private void Start()
        {
            uiManager = GameObject.FindGameObjectWithTag("MenuManager").GetComponent<UIManager>();
            PhotonNetwork.ConnectUsingSettings();
        }

        public override void OnConnectedToMaster()
        {
            Debug.Log("Connected to master");
            PhotonNetwork.JoinLobby();
            PhotonNetwork.AutomaticallySyncScene = true;
        }

        public override void OnJoinedLobby()
        {
            Debug.Log("Joined lobby");
            GameStateManager.Instance.SwitchGameState(new MainMenuState());
        }

        public void CreateRoom()
        {
            string roomName = uiManager.roomNameInputField.text;
            
            if (string.IsNullOrEmpty(roomName))
            {
                return;
            }

            currentRoomName = roomName;
            PhotonNetwork.CreateRoom(roomName);
            uiManager.ShowConnectionScreen();
        }

        public override void OnCreateRoomFailed(short returnCode, string message)
        {
            GameStateManager.Instance.SwitchGameState(new ConnectionErrorState(message));
        }
        
        public void LeaveRoom()
        {
            PhotonNetwork.LeaveRoom();
            uiManager.ShowLoadingScreen();
        }

        public override void OnLeftRoom()
        {
            Debug.Log("Left room: " + currentRoomName);
            uiManager.HideLoadingScreen();
            GameStateManager.Instance.SwitchGameState(new MainMenuState());
        }

        public void JoinRoom(RoomInfo info)
        {
            PhotonNetwork.JoinRoom(info.Name);
            uiManager.ShowConnectionScreen();
        }

        public override void OnJoinedRoom()
        {
            uiManager.HideConnectionScreen();
            GameStateManager.Instance.SwitchGameState(new WaitingRoomState());

            uiManager.roomNameText.text = currentRoomName;
            uiManager.startGameButton.SetActive(PhotonNetwork.IsMasterClient); // Sets button active only for the host
            
            Debug.Log("Joined room: " + currentRoomName);
        }

        public override void OnRoomListUpdate(List<RoomInfo> roomList)
        {
            if (roomList.Any())
            {
                Debug.Log("New room created: " + roomList[0].Name);
            }
            
            uiManager.GenerateRoomList(roomList);
        }

        public void StartGame()
        {
            GameStateManager.Instance.SwitchGameState(new GameSceneState());
        }
    }
}
