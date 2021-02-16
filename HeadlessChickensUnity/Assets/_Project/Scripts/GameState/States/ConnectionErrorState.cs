using PixelPeeps.HeadlessChickens.UI;
using UnityEngine;

namespace PixelPeeps.HeadlessChickens.GameState
{
    public class ConnectionErrorState : GameState
    {
        private readonly GameObject 
            canvasObject = StateManager.uiManager.connectionErrorCanvas;
        
        private readonly GameObject
            firstSelectedButton = StateManager.uiManager.connectionErrorFirstSelected;

        private readonly string 
            sceneName = StateManager.menuScene;

        private readonly string
            errorMessage;

        public ConnectionErrorState(string errorString)
        {
            errorMessage = errorString;
        }
        
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
            canvasObject.SetActive(false);
        }
    }
}