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
            StateManager.menuManager.mainMenuCanvas.SetActive(true);
        }

        public override void OnSceneLoad()
        {
            throw new System.NotImplementedException();
        }

        public override void StateExit()
        {
            StateManager.menuManager.mainMenuCanvas.SetActive(false);
        }
    }
}