using System.Collections;
using System.Collections.Generic;
using ExitGames.Client.Photon.Encryption;
using Photon.Pun;
using Photon.Realtime;
using PixelPeeps.HeadlessChickens.GameState;
using UnityEngine;

namespace PixelPeeps.HeadlessChickens.Network
{
    public class QuickStartButton : MonoBehaviourPunCallbacks
    {
        public void QuickStart()
        {
            string roomName = "Quick Start Room";
            
            RoomOptions options = new RoomOptions
            {
                MaxPlayers = 6, 
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