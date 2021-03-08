using Photon.Pun;
using PixelPeeps.HeadlessChickens._Project.Scripts.UI;
using UnityEngine;

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