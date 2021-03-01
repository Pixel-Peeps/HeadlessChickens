using Photon.Pun;
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
            GameStateManager.Instance.HideLoadingScreen();
            
            Debug.Log("StateEnter on ReturnToLobbyState");

            NetworkManager.Instance.gameIsRunning = false;
            StateManager.LoadNextScene(sceneName);
        }

        public override void OnSceneLoad()
        {
            StateManager.uiManager.UpdateRoomInfo();
            Debug.Log("OnSceneLoad");
            GameStateManager.Instance.SwitchGameState(new WaitingRoomState());
        }

        public override void StateExit()
        {
            Debug.Log("StateExit on ReturnToLobbyState");
            StateManager.uiManager.ActivateMenu(waitingRoomMenu);
        }
    }
}