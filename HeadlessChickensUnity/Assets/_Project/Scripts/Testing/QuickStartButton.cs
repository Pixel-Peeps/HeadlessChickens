using Photon.Pun;
using Photon.Realtime;
using PixelPeeps.HeadlessChickens.GameState;
using UnityEngine;
using UnityEngine.UI;

namespace PixelPeeps.HeadlessChickens.Network
{
    public class QuickStartButton : MonoBehaviourPunCallbacks
    {
        public void Start()
        {
            this.GetComponent<Button>().onClick.AddListener(QuickStart);
        }

        private static void QuickStart()
        {
            const string roomName = "Quick Start Room";
            
            RoomOptions options = new RoomOptions
            {
                MaxPlayers = 5, 
                EmptyRoomTtl = 0
            };

            
            if (PhotonNetwork.JoinOrCreateRoom(roomName, options, TypedLobby.Default))
            {
                if (PlayerPrefs.GetString("Nickname") == "")
                {
                    PhotonNetwork.LocalPlayer.NickName = "Player";
                }
                else
                {
                    PhotonNetwork.LocalPlayer.NickName = PlayerPrefs.GetString("Nickname");
                }
            }
        }

        public override void OnCreatedRoom()
        {
            PhotonNetwork.CurrentRoom.IsVisible = false;
        }

        public override void OnJoinedRoom()
        {
            GameStateManager.Instance.SwitchGameState(new WaitingRoomState());
        }
    }
}