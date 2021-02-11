using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace PixelPeeps.HeadlessChickens.GameState
{
    public class GameSceneState : GameState
    {
        public GameSceneState(GameStateManager stateManager) : base(stateManager){ }
        
        public override void StateEnter()
        {
            StateManager.LoadNextScene(StateManager.playScene);
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