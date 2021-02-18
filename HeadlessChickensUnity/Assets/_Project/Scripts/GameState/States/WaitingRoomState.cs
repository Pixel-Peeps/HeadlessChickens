using PixelPeeps.HeadlessChickens.UI;
using UnityEngine;

namespace PixelPeeps.HeadlessChickens.GameState
{
    public class WaitingRoomState : GameState
    {
        private readonly GameObject 
            canvasObject = StateManager.uiManager.waitingRoomCanvas;
        private readonly GameObject 
            firstSelectedButton = StateManager.uiManager.waitingRoomFirstSelected;

        private readonly string 
            sceneName = StateManager.menuScene;
        
        public override void StateEnter()
        {
            StateManager.LoadNextScene(sceneName);
            ActivateCanvas(canvasObject, firstSelectedButton);
        }

        public override void OnSceneLoad()
        {
            menuManagerObj = GameObject.FindGameObjectWithTag("MenuManager");
            StateManager.uiManager = menuManagerObj.GetComponent<UIManager>();
            
            ActivateCanvas(canvasObject, firstSelectedButton);
        }

        public override void StateExit()
        {
            if (canvasObject != null)
            {
                canvasObject.SetActive(false);
            }
        }
    }
}