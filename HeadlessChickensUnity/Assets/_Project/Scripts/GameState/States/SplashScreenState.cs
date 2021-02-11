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
            GameObject menuManagerObj = GameObject.FindGameObjectWithTag("MenuManager");
            StateManager.menuManager = menuManagerObj.GetComponent<MenuManager>();
            
            StateManager.menuManager.splashScreenCanvas.SetActive(true);
        }

        public override void StateExit()
        {
            StateManager.menuManager.splashScreenCanvas.SetActive(false);
        }
    }
}