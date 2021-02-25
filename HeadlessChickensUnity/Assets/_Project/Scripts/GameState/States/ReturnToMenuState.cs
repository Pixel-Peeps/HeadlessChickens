using Photon.Pun;
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
            NetworkManager.Instance.LeaveRoom();
            PhotonNetwork.LeaveRoom();
            Debug.Log("Left room");
            NetworkManager.Instance.gameIsRunning = false;
            Debug.Log("game is not running");
            StateManager.SwitchGameState(new MainMenuState());
            Debug.Log("<color=green> switch to main menu state</color>");
        }

        public override void OnSceneLoad()
        {

        }

        public override void StateExit()
        {
            
        }
    }
}