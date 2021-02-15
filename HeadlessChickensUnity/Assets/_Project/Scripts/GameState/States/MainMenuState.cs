using PixelPeeps.HeadlessChickens.UI;
using UnityEngine;

namespace PixelPeeps.HeadlessChickens.GameState
{
    public class MainMenuState : GameState
    {        
        public override void StateEnter()
        {
            StateManager.LoadNextScene(StateManager.menuScene);
            
            if (StateManager.uiManager != null)
            {
                GameObject canvasObject = StateManager.uiManager.mainMenuCanvas;
                GameObject firstSelectedButton = StateManager.uiManager.mainMenuFirstSelected;
                
                StateManager.uiManager.ActivateCanvas(canvasObject, firstSelectedButton);
            }
        }

        public override void OnSceneLoad()
        {
            GameObject menuManagerObj = GameObject.FindGameObjectWithTag("MenuManager");
            StateManager.uiManager = menuManagerObj.GetComponent<UIManager>();
            
            GameObject canvasObject = StateManager.uiManager.mainMenuCanvas;
            GameObject firstSelectedButton = StateManager.uiManager.mainMenuFirstSelected;
            
            StateManager.uiManager.ActivateCanvas(canvasObject, firstSelectedButton);
        }

        public override void StateExit()
        {
            StateManager.uiManager.mainMenuCanvas.SetActive(false);
        }
    }
}