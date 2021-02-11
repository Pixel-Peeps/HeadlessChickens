using UnityEngine;
using UnityEngine.SceneManagement;

namespace PixelPeeps.HeadlessChickens.GameState
{
    public class MainMenuState : GameState
    {
        public MainMenuState(GameStateManager stateManager) : base(stateManager){ }
        
        public override void StateEnter()
        {
            StateManager.LoadNextScene(StateManager.menuScene);
            StateManager.mainMenuCanvas.SetActive(true);
        }

        public override void StateExit()
        {
            StateManager.mainMenuCanvas.SetActive(false);
        }
    }
}