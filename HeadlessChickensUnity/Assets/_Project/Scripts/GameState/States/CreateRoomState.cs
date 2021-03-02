using ExitGames.Client.Photon.Encryption;
using Photon.Pun;
using PixelPeeps.HeadlessChickens.Network;
using PixelPeeps.HeadlessChickens.UI;
using UnityEngine;

namespace PixelPeeps.HeadlessChickens.GameState
{
    public class CreateRoomState : GameState
    {
        private readonly Menu 
            menu = StateManager.uiManager.createRoom;

        private readonly string 
            sceneName = StateManager.menuScene;
        
        public override void StateEnter()
        {
            StateManager.uiManager.DeactivateMenu(StateManager.uiManager.mainMenu);
            
            if (PhotonNetwork.CurrentRoom != null)
            {
                NetworkManager.Instance.LeaveRoom(); 
            }
            
            Debug.Log("StateEnter on CreateRoomState");
            StateManager.LoadNextScene(sceneName);
            ActivateMenu(menu);
        }

        public override void OnSceneLoad()
        {
            ActivateMenu(menu);
        }

        public override void StateExit()
        {
            DeactivateMenu(menu);
        }
    }
}