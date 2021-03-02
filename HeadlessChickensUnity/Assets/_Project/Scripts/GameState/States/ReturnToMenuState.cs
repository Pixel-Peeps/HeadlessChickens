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
            NetworkManager.Instance.DisconnectFromMaster();
            NetworkManager.Instance.gameIsRunning = false;
            GameStateManager.Instance.LoadNextScene(sceneName);
        }

        public override void OnSceneLoad()
        {
            StateManager.uiManager = GameObject.FindGameObjectWithTag("MenuManager").GetComponent<UIManager>();
            ActivateMenu(menu);
        }

        public override void StateExit()
        {
            
        }
    }
}