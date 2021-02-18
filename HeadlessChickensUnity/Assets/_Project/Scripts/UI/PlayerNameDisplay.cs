using Photon.Pun;
using TMPro;
using UnityEngine;

namespace PixelPeeps.HeadlessChickens.UI
{
    public class PlayerNameDisplay : MonoBehaviour
    {
        public TextMeshProUGUI nameDisplayText;
        
        public void Start()
        {
            nameDisplayText.text = PhotonNetwork.LocalPlayer.NickName;
        }
    }
}
