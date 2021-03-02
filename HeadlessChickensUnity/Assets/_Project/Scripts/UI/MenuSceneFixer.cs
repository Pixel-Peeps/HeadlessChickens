using PixelPeeps.HeadlessChickens.GameState;
using UnityEngine;

namespace PixelPeeps.HeadlessChickens.UI
{
    public class MenuSceneFixer : MonoBehaviour
    {
        void Start()
        {
            GameStateManager.Instance.SwitchGameState(new MainMenuState());
        }
    }
}
