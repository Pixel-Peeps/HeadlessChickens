using Photon.Pun;
using PixelPeeps.HeadlessChickens.UI;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace PixelPeeps.HeadlessChickens.GameState
{
    public class MainMenuState : GameState
    {
        private readonly Menu 
            menu = StateManager.uiManager.mainMenu;

        private readonly string 
            sceneName = StateManager.menuScene;
        
        public override void StateEnter()
        {
            if (SceneManager.GetActiveScene().name != sceneName)
            {
                StateManager.LoadNextScene(sceneName);
            }

            if (StateManager.uiManager.createRoom != null)
            {
                ActivateMenu(menu);

                StateManager.uiManager.createRoom.SetInputFieldText("");
                Debug.Log("<color=green> set input field to </color>" + StateManager.uiManager.createRoom.inputField.text);
            }
        }

        public override void OnSceneLoad()
        {
            ActivateMenu(menu);
            StateManager.uiManager.createRoom.SetInputFieldText("");
            Debug.Log("<color=green> set input field to </color>" + StateManager.uiManager.createRoom.inputField.text);
        }

        public override void StateExit()
        {
            DeactivateMenu(menu);
        }
    }
}