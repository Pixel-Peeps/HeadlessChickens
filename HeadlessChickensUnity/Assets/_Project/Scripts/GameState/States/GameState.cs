using UnityEngine;

namespace PixelPeeps.HeadlessChickens.GameState
{
    public abstract class GameState
    {
        protected GameStateManager StateManager;

        public GameState(GameStateManager stateManager)
        {
            StateManager = stateManager;
        }

        public abstract void StateEnter();
        public abstract void OnSceneLoad();
        public abstract void StateExit();
    }
}