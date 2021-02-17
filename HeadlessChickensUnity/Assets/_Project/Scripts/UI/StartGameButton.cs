using Photon.Pun;
using Photon.Pun.UtilityScripts;
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
            PhotonNetwork.LoadLevel("PlayScene");
        }
    }
}
