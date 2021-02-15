using UnityEngine;

namespace PixelPeeps.HeadlessChickens.GameState
{
    public abstract class GameState
    {
        protected GameStateManager StateManager = GameStateManager.Instance;

        public abstract void StateEnter();
        public abstract void OnSceneLoad();
        public abstract void StateExit();
    }
}