using UnityEngine;
using UnityEngine.SceneManagement;

namespace PixelPeeps.HeadlessChickens.GameState
{
    public class FoxLossState : GameState
    {
        public FoxLossState(GameStateManager stateManager) : base(stateManager){ }
        
        public override void StateEnter()
        {
            StateManager.LoadNextScene(StateManager.playScene);
            StateManager.foxLossCanvas = StateManager.InstantiateGUI(StateManager.foxLossCanvas);
        }

        public override void StateExit()
        {
            StateManager.DestroyGUI(StateManager.foxLossCanvas);
        }
    }
}