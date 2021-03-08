using Photon.Pun;
using UnityEngine;
// ReSharper disable UnusedMember.Global

namespace PixelPeeps.HeadlessChickens.UI
{
    public class KickIconDisplay : MonoBehaviour
    {
        public KickPlayerButton kickScript;
        
        public void DisplayIcon(GameObject obj)
        {
            if (!PhotonNetwork.IsMasterClient) return;

            if (Equals(kickScript.targetPlayer, PhotonNetwork.LocalPlayer)) return;
            
            obj.SetActive(true);
        }

        public void HideIcon(GameObject obj)
        {
            if (!PhotonNetwork.IsMasterClient) return; 
            
            if (Equals(kickScript.targetPlayer, PhotonNetwork.LocalPlayer)) return;
            
            obj.SetActive(false);
        }
    }
}