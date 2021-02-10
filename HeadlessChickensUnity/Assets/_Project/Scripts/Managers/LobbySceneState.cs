using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace PixelPeeps.HeadlessChickens.Managers
{
    public class LobbySceneState : GameState
    {
        public LobbySceneState(GameStateManager stateManager) : base(stateManager){ }
        
        public override void StateEnter()
        {
            Debug.Log("<color=green> Entered LobbyScene state </color>");
            LoadScene(StateManager.lobbyScene);
        }

        public override void StateExit()
        {
            Debug.Log("<color=red> Exited LobbyScene state </color>");
            StateManager.lobbyScreenCanvas.SetActive(false);
        }

        public override void LoadScene(string scene)
        {
            SceneManager.LoadSceneAsync(scene);
        }
    }
}