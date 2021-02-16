using PixelPeeps.HeadlessChickens.Network;
using UnityEngine;
using UnityEngine.UI;

namespace PixelPeeps.HeadlessChickens.UI
{
    public class CreateRoomButton : MonoBehaviour
    {
        private Button thisButton;
        private UIManager uiManager;

        private void Start()
        {
            uiManager = FindObjectOfType<UIManager>();
            
            thisButton = this.GetComponent<Button>();
            
            thisButton.onClick.AddListener(OnClick);
        }

        public void OnClick()
        {
            NetworkManager.Instance.CreateRoom();
        }
    }
}