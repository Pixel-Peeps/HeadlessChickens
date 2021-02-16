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
            uiManager.ShowConnectionScreen();
        }

        public override void OnConnectedToMaster()
        {
            uiManager.HideConnectionScreen();
            Debug.Log("Connected to master");
            PhotonNetwork.JoinLobby();
            uiManager.ShowConnectionScreen();
            PhotonNetwork.AutomaticallySyncScene = true;
        }

        public override void OnJoinedLobby()
        {
            uiManager.HideConnectionScreen();
            Debug.Log("Joined lobby");
            GameStateManager.Instance.SwitchGameState(new MainMenuState());
        }

        public void CreateRoom()
        {
            string roomName = uiManager.roomNameInputField.text;
            string nickName = uiManager.hostPlayerNameInputField.text;
            
            uiManager.createRoomEnterRoomNamePrompt.SetActive(false);
            uiManager.createRoomEnterNicknamePrompt.SetActive(false);
            
            PhotonNetwork.NickName = nickName;
            currentRoomName = roomName;

            if (!string.IsNullOrEmpty(nickName) && !string.IsNullOrEmpty(roomName))
            {
                PhotonNetwork.CreateRoom(roomName);
                uiManager.ShowConnectionScreen();
            }
            
            if (string.IsNullOrEmpty(nickName))
            {
                uiManager.createRoomEnterNicknamePrompt.SetActive(true);
            }
            
            if (string.IsNullOrEmpty(roomName))
            {
                uiManager.createRoomEnterRoomNamePrompt.SetActive(true);
            }
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
            string nickname = uiManager.joiningPlayerNameInputField.text;

            if (string.IsNullOrEmpty(nickname))
            {
                uiManager.enterNamePrompt.SetActive(true);
                return;
            }

            uiManager.enterNamePrompt.SetActive(false);
            
            PhotonNetwork.JoinRoom(info.Name);

            PhotonNetwork.NickName = uiManager.joiningPlayerNameInputField.text;

            uiManager.ShowConnectionScreen();
        }

        public override void OnJoinedRoom()
        {
            uiManager.HideConnectionScreen();
            GameStateManager.Instance.SwitchGameState(new WaitingRoomState());

            uiManager.roomNameText.text = currentRoomName;
            uiManager.startGameButton.SetActive(PhotonNetwork.IsMasterClient); // Sets button active only for the host

            uiManager.roomPlayerCount.text = string.Format("In Room: {0} / 6", PhotonNetwork.CurrentRoom.PlayerCount);
            
            Player[] players = PhotonNetwork.PlayerList;

            foreach (Player t in players)
            {
                uiManager.UpdatePlayerList(t);
            }
            
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

        public override void OnPlayerEnteredRoom(Player newPlayer)
        {
            Debug.Log("OnPlayerEnteredRoom");
            uiManager.UpdatePlayerList(newPlayer);
        }

        public void StartGame()
        {
            GameStateManager.Instance.SwitchGameState(new GameSceneState());
        }
    }
}
