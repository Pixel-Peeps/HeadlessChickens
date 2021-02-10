namespace PixelPeeps.HeadlessChickens.Managers
{
    public abstract class GameState
    {
        protected GameStateManager StateManager;

        public GameState(GameStateManager stateManager)
        {
            StateManager = stateManager;
        }

        public abstract void StateEnter();
        public abstract void StateExit();
    }
}