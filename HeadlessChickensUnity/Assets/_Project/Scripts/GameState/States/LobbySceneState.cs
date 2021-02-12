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
            StateManager.LoadNextScene(StateManager.lobbyScene);
        }

        public override void OnSceneLoad()
        {
            throw new NotImplementedException();
        }

        public override void StateExit()
        {
        }
    }
}