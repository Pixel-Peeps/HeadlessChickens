using UnityEngine;
using UnityEngine.SceneManagement;

namespace PixelPeeps.HeadlessChickens.GameState
{
    public class StoreScreenState : GameState
    {
        public StoreScreenState(GameStateManager stateManager) : base(stateManager){ }
        
        public override void StateEnter()
        {
            StateManager.LoadNextScene(StateManager.menuScene);
            StateManager.storeScreenCanvas.SetActive(true);
        }

        public override void StateExit()
        {
            StateManager.storeScreenCanvas.SetActive(false);
        }
    }
}