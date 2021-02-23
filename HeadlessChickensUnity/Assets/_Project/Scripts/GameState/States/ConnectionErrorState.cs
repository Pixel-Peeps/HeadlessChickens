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
            StateManager.ShowErrorScreen();
        }

        public override void OnSceneLoad()
        {
        }

        public override void StateExit()
        {
            StateManager.HideErrorScreen();
        }
    }
}