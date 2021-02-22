using PixelPeeps.HeadlessChickens.UI;
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

        protected static void ActivateMenu(Menu menu)
        {
            if (StateManager.uiManager == null)
            {
                Debug.LogError("Cannot find Menu Manager in scene");
                return;
            }
            
            StateManager.uiManager.ActivateMenu(menu);
        }
        
        protected static void DeactivateMenu(Menu menu)
        {
            if (StateManager.uiManager == null)
            {
                Debug.LogError("Cannot find Menu Manager in scene");
                return;
            }
            
            StateManager.uiManager.DeactivateMenu(menu);
        }
    }
}