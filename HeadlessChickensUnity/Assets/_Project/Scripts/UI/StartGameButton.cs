using Photon.Pun;
using Photon.Realtime;
using PixelPeeps.HeadlessChickens.Network;
using UnityEngine;
using UnityEngine.UI;

namespace PixelPeeps.HeadlessChickens.UI
{
    public class StartGameButton : MonoBehaviour
    {
        private Button thisButton;
        
        private void Start()
        {
            thisButton = gameObject.GetComponent<Button>();
            
            thisButton.onClick.AddListener(OnClick);
        }
        
        private void OnClick()
        {
            PlayerAssignment.Instance.AssignPlayerRoles(PhotonNetwork.PlayerList);
            
            PhotonNetwork.LoadLevel("PlayScene");
        }
    }
}
