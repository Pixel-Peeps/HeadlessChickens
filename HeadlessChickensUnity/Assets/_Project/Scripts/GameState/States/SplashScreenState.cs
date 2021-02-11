using PixelPeeps.HeadlessChickens.UI;
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
        }

        public override void OnSceneLoad()
        {
            if (StateManager.menuManager != null)
            {
                StateManager.menuManager.splashScreenCanvas.SetActive(true);
            }
            else
            {
                Debug.LogError("No menu manager found in scene");
            }
        }

        public override void StateExit()
        {
            StateManager.menuManager.splashScreenCanvas.SetActive(false);
        }
    }
}