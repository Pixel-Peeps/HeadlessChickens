using PixelPeeps.HeadlessChickens.UI;
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
            
            if (StateManager.menuManager.mainMenuCanvas != null)
            {
                StateManager.menuManager.mainMenuCanvas.SetActive(true);
                
                GameObject firstSelectedButton = StateManager.menuManager.mainMenuFirstSelected;
                StateManager.menuManager.SetEventSystemCurrentSelection(firstSelectedButton);
            }
        }

        public override void OnSceneLoad()
        {
            GameObject menuManagerObj = GameObject.FindGameObjectWithTag("MenuManager");
            StateManager.menuManager = menuManagerObj.GetComponent<MenuManager>();

            StateManager.menuManager.mainMenuCanvas.SetActive(true);
            
            GameObject firstSelectedButton = StateManager.menuManager.mainMenuFirstSelected;
            StateManager.menuManager.SetEventSystemCurrentSelection(firstSelectedButton);
        }

        public override void StateExit()
        {
            StateManager.menuManager.mainMenuCanvas.SetActive(false);
        }
    }
}