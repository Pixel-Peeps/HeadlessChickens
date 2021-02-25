using PixelPeeps.HeadlessChickens.Network;
using PixelPeeps.HeadlessChickens.UI;
using UnityEngine;

namespace PixelPeeps.HeadlessChickens.GameState
{
    public class ReturnToMenuState : GameState
    {
        private readonly Menu
            menu = StateManager.uiManager.mainMenu;

        private readonly string 
            sceneName = StateManager.menuScene;
        
        public override void StateEnter()
        {
            StateManager.LoadNextScene(sceneName);
        }

        public override void OnSceneLoad()
        {
            NetworkManager.Instance.LeaveRoom();
            NetworkManager.Instance.gameIsRunning = false;
            StateManager.SwitchGameState(new MainMenuState());
            Debug.Log("<color=green> switch to main menu state</color>");
        }

        public override void StateExit()
        {
            
        }
    }
}