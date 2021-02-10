using UnityEngine;

namespace PixelPeeps.HeadlessChickens.Managers
{
    public class SplashScreenState : GameState
    {
        public SplashScreenState(GameStateManager stateManager) : base(stateManager){ }
        
        public override void StateEnter()
        {
            Debug.Log("<color=lime> Entered SplashScreen state </color>");
            StateManager.splashScreenCanvas.SetActive(true);
        }

        public override void StateExit()
        {
            Debug.Log("<color=red> Exited SplashScreen state </color>");
            StateManager.splashScreenCanvas.SetActive(false);
        }
    }
}