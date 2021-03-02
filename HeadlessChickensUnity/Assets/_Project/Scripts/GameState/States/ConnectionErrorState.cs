using PixelPeeps.HeadlessChickens.UI;
using UnityEngine;

namespace PixelPeeps.HeadlessChickens.GameState
{
    public class ConnectionErrorState : GameState
    {
        private readonly string
            errorMessage;

        public ConnectionErrorState(string errorString)
        {
            errorMessage = errorString;
            StateManager.ShowErrorScreen(errorMessage);
        }
        
        public override void StateEnter()
        {
            StateManager.ShowErrorScreen(errorMessage);
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