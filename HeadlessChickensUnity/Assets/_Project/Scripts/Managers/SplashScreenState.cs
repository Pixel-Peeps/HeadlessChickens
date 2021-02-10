using UnityEngine;
using UnityEngine.SceneManagement;

namespace PixelPeeps.HeadlessChickens.Managers
{
    public class SplashScreenState : GameState
    {
        public SplashScreenState(GameStateManager stateManager) : base(stateManager){ }
        
        public override void StateEnter()
        {
            Debug.Log("<color=green> Entered SplashScreen state </color>");
           
            if (StateManager.splashScreenCanvas != null)
            {
                StateManager.splashScreenCanvas.SetActive(true);
            }
        }

        public override void StateExit()
        {
            Debug.Log("<color=red> Exited SplashScreen state </color>");
            StateManager.splashScreenCanvas.SetActive(false);
        }
        
        public override void LoadScene(string scene)
        {
            SceneManager.LoadSceneAsync(scene);
        }
    }
}