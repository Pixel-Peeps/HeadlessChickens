using UnityEngine;

namespace PixelPeeps.HeadlessChickens.GameState
{
    public abstract class GameState
    {
        protected static readonly GameStateManager StateManager = GameStateManager.Instance;
        protected GameObject menuManagerObj;
        
        public abstract void StateEnter();
        public abstract void OnSceneLoad();
        public abstract void StateExit();
        public void ActivateCanvas(GameObject canvasObject, GameObject firstSelectedButton)
        {
            if (StateManager.uiManager != null)
            {
                StateManager.uiManager.ActivateCanvas(canvasObject, firstSelectedButton);
            }
        }
    }
}