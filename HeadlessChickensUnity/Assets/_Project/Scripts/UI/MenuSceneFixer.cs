using PixelPeeps.HeadlessChickens.GameState;
using PixelPeeps.HeadlessChickens.Network;
using UnityEngine;

namespace PixelPeeps.HeadlessChickens.UI
{
    public class MenuSceneFixer : MonoBehaviour
    {
        private void Start()
        {
            GameStateManager.Instance.SwitchGameState(new MainMenuState());
            NetworkManager.Instance.DisconnectFromMaster();
        }
    }
}
