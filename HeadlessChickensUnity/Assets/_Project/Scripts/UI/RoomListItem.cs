﻿using Photon.Realtime;
using PixelPeeps.HeadlessChickens.Network;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace PixelPeeps.HeadlessChickens.UI
{
    public class RoomListItem : MonoBehaviour
    {
        RoomInfo info;
    
        public TextMeshProUGUI nameText;

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
}
