using Photon.Pun;
using Photon.Realtime;
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
            Player[] allPlayersInRoom = PhotonNetwork.PlayerList;
            
            foreach (Player player in allPlayersInRoom)
            {
                
            }
            PhotonNetwork.LoadLevel("PlayScene");
        }
    }
}
