using PixelPeeps.HeadlessChickens.UI;
using UnityEngine;

namespace PixelPeeps.HeadlessChickens.GameState
{
    public class ConnectionErrorState : GameState
    {
        private readonly Menu 
            menu = StateManager.uiManager.connectionError;

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
            ActivateMenu(menu);
        }

        public override void OnSceneLoad()
        {
            menuManagerObj = GameObject.FindGameObjectWithTag("MenuManager");
            StateManager.uiManager = menuManagerObj.GetComponent<UIManager>();

            ActivateMenu(menu);
        }

        public override void StateExit()
        {
            DeactivateMenu(menu);
        }
    }
}