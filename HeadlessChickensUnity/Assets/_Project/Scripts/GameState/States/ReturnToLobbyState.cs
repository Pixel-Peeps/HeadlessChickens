using PixelPeeps.HeadlessChickens.Network;
using PixelPeeps.HeadlessChickens.UI;
using UnityEngine;

namespace PixelPeeps.HeadlessChickens.GameState
{
    public class ReturnToLobbyState : GameState
    {
        private readonly Menu
            waitingRoomMenu = StateManager.uiManager.waitingRoom;
        
        private readonly Menu
            mainMenu = StateManager.uiManager.mainMenu;

        private readonly string 
            sceneName = StateManager.menuScene;
        
        public override void StateEnter()
        {
            NetworkManager.Instance.gameIsRunning = false;
            StateManager.LoadNextScene(sceneName);
        }

        public override void OnSceneLoad()
        {
            Debug.Log("OnSceneLoad");
            GameStateManager.Instance.SwitchGameState(new WaitingRoomState());
        }

        public override void StateExit()
        {
            DeactivateMenu(mainMenu);
        }
    }
}