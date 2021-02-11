using UnityEngine;
using UnityEngine.SceneManagement;

namespace PixelPeeps.HeadlessChickens.GameState
{
    public class SplashScreenState : GameState
    {
        public SplashScreenState(GameStateManager stateManager) : base(stateManager){ }
        
        public override void StateEnter()
        {
            StateManager.LoadNextScene(StateManager.menuScene);
                        
            if (StateManager.splashScreenCanvas != null)
            {
                StateManager.splashScreenCanvas.SetActive(true);
            }
        }

        public override void StateExit()
        {
            StateManager.splashScreenCanvas.SetActive(false);
        }
    }
}