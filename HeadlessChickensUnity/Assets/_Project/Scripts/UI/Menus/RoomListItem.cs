using Photon.Realtime;
using PixelPeeps.HeadlessChickens.Network;
using TMPro;
using UnityEngine;

namespace PixelPeeps.HeadlessChickens.UI
{
    public class RoomListItem : MonoBehaviour
    {
        public RoomInfo roomInfo;
    
        public TextMeshProUGUI nameText;

        public void SetUp(RoomInfo _info)
        {
            roomInfo = _info;
            nameText.text = string.Format("{0} ({1}/{2})", roomInfo.Name, roomInfo.PlayerCount, roomInfo.MaxPlayers);
        }

        public void OnClick()
        {
            Debug.Log( roomInfo );
            NetworkManager.Instance.JoinRoom(roomInfo);
        }
    }
}
