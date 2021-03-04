using Photon.Pun;
using PixelPeeps.HeadlessChickens.Network;
using PixelPeeps.HeadlessChickens.UI;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace PixelPeeps.HeadlessChickens.GameState
{
    public class RestartGameState : GameState
    {
        private readonly Menu
            menu = StateManager.uiManager.mainMenu;

        private readonly string 
            menuScene = StateManager.menuScene;
        
        private readonly string 
            playScene = StateManager.menuScene;
        
        public override void StateEnter()
        {
            StateManager.LoadNextScene("BufferScene");
        }

        public override void OnSceneLoad()
        {
            if (PhotonNetwork.IsMasterClient)
            {
                NetworkManager.Instance.StartGameOnMaster();
            }
            
        }

        public override void StateExit()
        {
            
        }
    }
}