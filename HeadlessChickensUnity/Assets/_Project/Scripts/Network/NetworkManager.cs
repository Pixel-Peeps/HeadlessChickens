﻿using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using PixelPeeps.HeadlessChickens.GameState;
using PixelPeeps.HeadlessChickens.UI;
using UnityEngine;
// ReSharper disable UnusedMember.Global

namespace PixelPeeps.HeadlessChickens.Network
{
    public class NetworkManager : MonoBehaviourPunCallbacks
    {
        public static NetworkManager Instance { get; private set; }

        private UIManager uiManager;

        public bool gameIsRunning;

        public const int MIN_PLAYERS = 1;
        private const int MAX_PLAYERS = 5;

        private string currentRoomName;

        public float gameTime = 600f;

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
            ConnectToMaster();
        }

        public void UpdateGameTime(float _gameTime)
        {
            photonView.RPC("UpdateGameTimeRPC", RpcTarget.All, _gameTime);
        }

        [PunRPC]
        public void UpdateGameTimeRPC(float _gameTime)
        {
            gameTime = _gameTime;
        }
        
        private void ConnectToMaster()
        {
           GameObject uiManagerObj = GameObject.FindGameObjectWithTag("MenuManager");

            if (uiManagerObj != null)
            {
                uiManager = uiManagerObj.GetComponent<UIManager>();
            }
            
            PhotonNetwork.ConnectUsingSettings();
            
            GameStateManager.Instance.ShowConnectingScreen();
        }

        public void DisconnectFromMaster()
        {
            gameIsRunning = false;
            PhotonNetwork.LeaveRoom();
            PhotonNetwork.LeaveLobby();
        }

        public void KickPlayer(Player targetPlayer)
        {
            if (!PhotonNetwork.IsMasterClient) return;
            
            photonView.RPC("KickPlayerRPC", targetPlayer);
        }

        [PunRPC]
        public void KickPlayerRPC()
        {
            PhotonNetwork.LeaveRoom();
            GameStateManager.Instance.SwitchGameState(new MainMenuState());
        }

        #region Ending Game

        public float lobbyReturnCountdown = 5f;

        public void EndGame()
        {
            if (!PhotonNetwork.IsMasterClient)
            {
                return;
            }
            
            photonView.RPC("EndGameRPC", RpcTarget.AllViaServer);
            
            StartCoroutine(LobbyReturnCountdownCoroutine());
        }
        
        [PunRPC]
        public void EndGameRPC()
        {
            Cursor.lockState = CursorLockMode.None;
            
            PhotonNetwork.Destroy(NewGameManager.Instance.myController);
            gameIsRunning = false;
        }
        
        private IEnumerator LobbyReturnCountdownCoroutine()
        {
            yield return new WaitForSecondsRealtime(lobbyReturnCountdown);
            //photonView.RPC("RestartGameRPC", RpcTarget.All); 
            //ReturnToMenu();
            
            photonView.RPC("ReturnToLobbyRPC", RpcTarget.AllViaServer); 
            
            MakeRoomPublic();
        }

        [PunRPC]
        public void ReturnToLobbyRPC()
        {
            GameStateManager.Instance.SwitchGameState( new ReturnToMenuState() );
        }

        #endregion

        #region Photon Callback Methods
        public override void OnConnectedToMaster()
        {
            GameStateManager.Instance.HideConnectingScreen();
            
            Debug.Log("Connected to master");
            PhotonNetwork.JoinLobby();
            
            GameStateManager.Instance.ShowConnectingScreen();
            
            PhotonNetwork.AutomaticallySyncScene = true;
        }

        public override void OnDisconnected(DisconnectCause cause)
        {
            ConnectToMaster();
        }

        public override void OnJoinedLobby()
        {
            GameStateManager.Instance.HideConnectingScreen();
            
            Debug.Log("Joined lobby");
            GameStateManager.Instance.SwitchGameState(new MainMenuState());
        }

        public void CreateRoom()
        {
            uiManager = GameObject.FindGameObjectWithTag("MenuManager").GetComponent<UIManager>();
            Debug.Log("<color=red> CreateRoom() INPUT FIELD IS " + uiManager.createRoom.GetInputFieldText() + "</color>");
            string roomName = uiManager.createRoom.GetInputFieldText();
            Debug.Log("<color=red> CreateRoom() GOT NAME OF " + roomName + "</color>");
            uiManager.createRoom.HideErrorMessage();    
            Debug.Log("<color=red> CreateRoom() HID ERROR MESSAGE </color>");
            currentRoomName = roomName;
            Debug.Log("<color=red> CreateRoom() SET CURRENT ROOM NAME TO " + currentRoomName + "</color>");
            
            if (!string.IsNullOrEmpty(roomName))
            {
                Debug.Log("<color=red> CreateRoom() ROOM INPUT IS NOT EMPTY </color>");
                RoomOptions options = new RoomOptions
                {
                    MaxPlayers = MAX_PLAYERS, 
                    EmptyRoomTtl = 0
                };
                Debug.Log("<color=red> CreateRoom() GENERATED ROOM OPTIONS </color>");
                PhotonNetwork.CreateRoom(roomName, options);
                Debug.Log("<color=red> CreateRoom() CREATED ROOM OF NAME: " + roomName + "</color>");
                GameStateManager.Instance.ShowConnectingScreen();
            }
            
            if (string.IsNullOrEmpty(roomName))
            {
                uiManager.createRoom.DisplayErrorMessage();
                Debug.Log("<color=red> CreateRoom() INPUT FIELD WAS EMPTY </color>");
            }
        }

        public override void OnCreateRoomFailed(short returnCode, string message)
        {
            GameStateManager.Instance.ShowErrorScreen(message);
        }
        
        public void LeaveRoom()
        {
            if (gameIsRunning)
            {
                return;
            }
            
            currentRoomName = PhotonNetwork.CurrentRoom.Name;
            PhotonNetwork.LeaveRoom();
            
            GameStateManager.Instance.ShowLoadingScreen();
        }

        public override void OnLeftRoom()
        {
            if (gameIsRunning)
            {
                print("<color=red> GAME IS RUNNING </color>");
                PhotonNetwork.Destroy(NewGameManager.Instance.myController);
            }
            
            Debug.Log("Left room: " + currentRoomName);
            GameStateManager.Instance.HideLoadingScreen();

            if (GameStateManager.Instance != null)
            {
                GameStateManager.Instance.SwitchGameState(new MainMenuState());
            }
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
                        
            Debug.Log("Room " + room.Name + " has been joined. It currently has " + room.PlayerCount + "/" + room.MaxPlayers + 
                      " players, isVisible is set to: " + room.IsVisible + " and isOpen is set to: " + room.IsOpen);

            GameStateManager.Instance.HideConnectingScreen();
            GameStateManager.Instance.SwitchGameState(new WaitingRoomState());
        }

        public override void OnRoomListUpdate(List<RoomInfo> roomList)
        {
            uiManager = GameObject.FindGameObjectWithTag("MenuManager").GetComponent<UIManager>();
            Debug.Log("RoomListUpdate");
            uiManager.GenerateRoomList(roomList);
        }

        public override void OnPlayerEnteredRoom(Player newPlayer)
        {            
            uiManager.UpdateRoomInfo();
        }

        public override void OnPlayerLeftRoom(Player otherPlayer)
        {
            if (gameIsRunning)
            {
                return;
            }
            
            uiManager.UpdateRoomInfo();
        }
        
        #endregion

        #region Game Set-up
        public void StartGameOnMaster()
        {
            Debug.Log("Starting game on master");
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
            Debug.Log("Starting game on others");
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
            Debug.Log("GAME SETUP COMPLETE CALLED");
            GameStateManager.Instance.HideSetupScreen();
        }

        public void MakeRoomPublic()
        {
            PhotonNetwork.CurrentRoom.IsOpen = true;
            PhotonNetwork.CurrentRoom.IsVisible = true;
        }

        public void MakeRoomPrivate()
        {
            PhotonNetwork.CurrentRoom.IsOpen = false;
            PhotonNetwork.CurrentRoom.IsVisible = false;
        }

        private string GetPlayerNickname()
        {
            return uiManager.mainMenu.GetInputFieldText();
        }
        
        #endregion
    }
}
