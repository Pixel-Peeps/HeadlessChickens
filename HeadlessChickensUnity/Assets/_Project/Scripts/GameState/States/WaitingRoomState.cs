using PixelPeeps.HeadlessChickens.UI;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace PixelPeeps.HeadlessChickens.GameState
{
    public class WaitingRoomState : GameState
    {
        private readonly Menu 
            menu = StateManager.uiManager.waitingRoom;

        private readonly string 
            sceneName = StateManager.menuScene;
        
        public override void StateEnter()
        {
            if (SceneManager.GetActiveScene().name != sceneName)
            {
                StateManager.LoadNextScene(sceneName);
            }
            
            else
            {
                StateManager.uiManager = GameObject.FindGameObjectWithTag( "MenuManager" ).GetComponent<UIManager>();
                ActivateMenu(menu); 
                StateManager.uiManager.UpdateRoomInfo();
            }
        }

        public override void OnSceneLoad()
        {
            StateManager.uiManager = GameObject.FindGameObjectWithTag("MenuManager").GetComponent<UIManager>();
            ActivateMenu(menu);
            StateManager.uiManager.UpdateRoomInfo();
        }

        public override void StateExit()
        {
            DeactivateMenu(menu);
        }
    }
}