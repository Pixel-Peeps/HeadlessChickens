using PixelPeeps.HeadlessChickens.UI;
using UnityEngine;

namespace PixelPeeps.HeadlessChickens.GameState
{
    public class WaitingRoomState : GameState
    {        
        public override void StateEnter()
        {
            StateManager.LoadNextScene(StateManager.menuScene);

            if (StateManager.uiManager != null)
            {
                GameObject canvasObject = StateManager.uiManager.waitingRoomCanvas;
                GameObject firstSelectedButton = StateManager.uiManager.waitingRoomFirstSelected;
                
                StateManager.uiManager.ActivateCanvas(canvasObject, firstSelectedButton);
            }
        }

        public override void OnSceneLoad()
        {
            GameObject menuManagerObj = GameObject.FindGameObjectWithTag("MenuManager");
            StateManager.uiManager = menuManagerObj.GetComponent<UIManager>();
            
            GameObject canvasObject = StateManager.uiManager.waitingRoomCanvas;
            GameObject firstSelectedButton = StateManager.uiManager.waitingRoomFirstSelected;
            
            StateManager.uiManager.ActivateCanvas(canvasObject, firstSelectedButton);
        }

        public override void StateExit()
        {
        }
    }
}