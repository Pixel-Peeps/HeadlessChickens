﻿using UnityEngine;

namespace PixelPeeps.HeadlessChickens.GameState
{
    public class GameSceneState : GameState
    {
        public override void StateEnter()
        {
            StateManager.LoadNextScene(StateManager.mainScene);
        }

        public override void OnSceneLoad()
        {
            
        }

        public override void StateExit()
        {
        }
    }
}