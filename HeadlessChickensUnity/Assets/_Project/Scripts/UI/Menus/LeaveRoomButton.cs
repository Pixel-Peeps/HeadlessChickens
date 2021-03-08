using PixelPeeps.HeadlessChickens.GameState;
using PixelPeeps.HeadlessChickens.Network;
using UnityEngine;
using UnityEngine.UI;

namespace PixelPeeps.HeadlessChickens.UI
{
    public class LeaveRoomButton : MonoBehaviour
    {
        private Button thisButton;

        private void Start()
        {      
            thisButton = GetComponent<Button>();
            
            thisButton.onClick.AddListener(OnClick);
        }

        private static void OnClick()
        {
            NetworkManager.Instance.LeaveRoom();
            GameStateManager.Instance.SwitchGameState(new MainMenuState());
        }
    }
}