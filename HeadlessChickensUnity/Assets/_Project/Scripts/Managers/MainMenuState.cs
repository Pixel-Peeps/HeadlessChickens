using UnityEngine;

namespace PixelPeeps.HeadlessChickens.Managers
{
    public class MainMenuState : GameState
    {
        public MainMenuState(GameStateManager stateManager) : base(stateManager){ }
        
        public override void StateEnter()
        {
            Debug.Log("<color=lime> Entered MainMenu state </color>");
            StateManager.mainMenuCanvas.SetActive(true);
        }

        public override void StateExit()
        {
            Debug.Log("<color=red> Exited MainMenu state </color>");
            StateManager.mainMenuCanvas.SetActive(false);
        }

        
    }
}