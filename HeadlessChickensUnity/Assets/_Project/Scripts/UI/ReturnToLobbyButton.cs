using PixelPeeps.HeadlessChickens.Network;
using UnityEngine;
using UnityEngine.UI;

namespace PixelPeeps.HeadlessChickens.UI
{
    public class ReturnToLobbyButton : MonoBehaviour
    {
        private Button thisButton;

        private void Start()
        {            
            thisButton = GetComponent<Button>();
            
            thisButton.onClick.AddListener(OnClick);
        }

        private void OnClick()
        {
            //NewGameManager.Instance.ReturnAllToLobby();
        }
    }
}