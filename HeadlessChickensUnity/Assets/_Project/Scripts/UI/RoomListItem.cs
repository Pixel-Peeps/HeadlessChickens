using Photon.Realtime;
using PixelPeeps.HeadlessChickens.Network;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RoomListItem : MonoBehaviour
{
    RoomInfo info;
    
    public TextMeshProUGUI nameText;
    public Button thisButton;

    public void SetUp(RoomInfo _info)
    {
        info = _info;
        nameText.text = info.Name;
    }

    public void OnClick()
    {
        NetworkManager.Instance.JoinRoom(info);
    }
}
