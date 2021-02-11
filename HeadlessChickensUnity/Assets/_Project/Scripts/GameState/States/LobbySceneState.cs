using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace PixelPeeps.HeadlessChickens.GameState
{
    public class LobbySceneState : GameState
    {
        public LobbySceneState(GameStateManager stateManager) : base(stateManager){ }
        
        public override void StateEnter()
        {
            Debug.Log("<color=green> Entered LobbyScene state </color>");
            StateManager.LoadNextScene(StateManager.lobbyScene);
        }

        public override void StateExit()
        {
            Debug.Log("<color=red> Exited LobbyScene state </color>");
        }
    }
}