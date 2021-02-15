using System;
using Photon.Pun;
using PixelPeeps.HeadlessChickens.GameState;
using UnityEngine;

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

        private void Start()
        {
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
            GameStateManager.Instance.SwitchGameState(new MainMenuState());
            Debug.Log("Joined lobby");
        }

        public void CreateRoom(string roomName)
        {
            if (string.IsNullOrEmpty(roomName))
            {
                return;
            }
            
            PhotonNetwork.CreateRoom(roomName);
            GameStateManager.Instance.SwitchGameState(new LoadingSceneState());
        }

        public override void OnCreateRoomFailed(short returnCode, string message)
        {
            GameStateManager.Instance.SwitchGameState(new ConnectionFailedState());
        }
        
        public void LeaveRoom()
        {
            PhotonNetwork.LeaveRoom();
            GameStateManager.Instance.SwitchGameState(new LoadingSceneState());
        }

        public override void OnLeftRoom()
        {
            GameStateManager.Instance.SwitchGameState(new MainMenuState());
        }

        public void OnJoinedRoom()
        {
            
        }

        public void StartGame()
        {
            GameStateManager.Instance.SwitchGameState(new GameSceneState());
        }
    }
}
