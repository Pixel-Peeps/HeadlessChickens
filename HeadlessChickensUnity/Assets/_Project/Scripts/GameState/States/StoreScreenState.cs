using PixelPeeps.HeadlessChickens.UI;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace PixelPeeps.HeadlessChickens.GameState
{
    public class StoreScreenState : GameState
    {
        public StoreScreenState(GameStateManager stateManager) : base(stateManager){ }
        
        public override void StateEnter()
        {
            StateManager.LoadNextScene(StateManager.menuScene);
            
            if (StateManager.menuManager.storeScreenCanvas != null)
            {
                StateManager.menuManager.storeScreenCanvas.SetActive(true);
                
                GameObject firstSelectedButton = StateManager.menuManager.storeScreenFirstSelected;
                StateManager.menuManager.SetEventSystemCurrentSelection(firstSelectedButton);
            }
        }

        public override void OnSceneLoad()
        {
            GameObject menuManagerObj = GameObject.FindGameObjectWithTag("MenuManager");
            StateManager.menuManager = menuManagerObj.GetComponent<MenuManager>();

            StateManager.menuManager.storeScreenCanvas.SetActive(true);
            
            GameObject firstSelectedButton = StateManager.menuManager.storeScreenFirstSelected;
            StateManager.menuManager.SetEventSystemCurrentSelection(firstSelectedButton);
        }

        public override void StateExit()
        {
            StateManager.menuManager.storeScreenCanvas.SetActive(false);
        }
    }
}