﻿using Photon.Pun;
using Photon.Realtime;
using PixelPeeps.HeadlessChickens.GameState;

namespace PixelPeeps.HeadlessChickens.Network
{
    public class QuickStartButton : MonoBehaviourPunCallbacks
    {
        public void QuickStart()
        {
            const string roomName = "Quick Start Room";
            
            RoomOptions options = new RoomOptions
            {
                MaxPlayers = 5, 
                EmptyRoomTtl = 0
            };

            
            if (PhotonNetwork.JoinOrCreateRoom(roomName, options, TypedLobby.Default))
            {
                PhotonNetwork.LocalPlayer.NickName = "Player";
            }
        }

        public override void OnJoinedRoom()
        {
            GameStateManager.Instance.SwitchGameState(new WaitingRoomState());
        }
    }
}