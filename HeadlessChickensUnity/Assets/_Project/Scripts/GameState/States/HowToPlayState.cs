using PixelPeeps.HeadlessChickens.UI;

namespace PixelPeeps.HeadlessChickens.GameState
{
    public class HowToPlayState : GameState
    {
        private readonly Menu 
            menu = StateManager.uiManager.howToPlay;

        private readonly string 
            sceneName = StateManager.menuScene;
        
        public override void StateEnter()
        {
            StateManager.uiManager.DeactivateMenu(StateManager.uiManager.mainMenu);
            StateManager.LoadNextScene(sceneName);
            ActivateMenu(menu);
        }

        public override void OnSceneLoad()
        {
            ActivateMenu(menu);
        }

        public override void StateExit()
        {
            DeactivateMenu(menu);
        }
    }
}