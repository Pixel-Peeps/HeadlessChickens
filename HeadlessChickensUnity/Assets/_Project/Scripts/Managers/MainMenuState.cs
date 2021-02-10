using UnityEngine;
using UnityEngine.SceneManagement;

namespace PixelPeeps.HeadlessChickens.Managers
{
    public class MainMenuState : GameState
    {
        public MainMenuState(GameStateManager stateManager) : base(stateManager){ }
        
        public override void StateEnter()
        {
            Debug.Log("<color=green> Entered MainMenu state </color>");
            StateManager.mainMenuCanvas.SetActive(true);
        }

        public override void StateExit()
        {
            Debug.Log("<color=red> Exited MainMenu state </color>");
            StateManager.mainMenuCanvas.SetActive(false);
        }

        public override void LoadScene(string scene)
        {
            SceneManager.LoadSceneAsync(scene);
        }
    }
}