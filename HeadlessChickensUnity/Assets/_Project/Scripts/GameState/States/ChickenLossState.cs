using UnityEngine;
using UnityEngine.SceneManagement;

namespace PixelPeeps.HeadlessChickens.GameState
{
    public class ChickenLossState : GameState
    {
        public ChickenLossState(GameStateManager stateManager) : base(stateManager){ }
        
        public override void StateEnter()
        {
            StateManager.resultsScreenManager.chickenLossCanvas.SetActive(true);
        }

        public override void OnSceneLoad()
        {
            throw new System.NotImplementedException();
        }

        public override void StateExit()
        {
            StateManager.resultsScreenManager.chickenLossCanvas.SetActive(false);
        }
    }
}