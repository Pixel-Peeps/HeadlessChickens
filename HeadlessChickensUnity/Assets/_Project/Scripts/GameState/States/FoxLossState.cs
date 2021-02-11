using UnityEngine;
using UnityEngine.SceneManagement;

namespace PixelPeeps.HeadlessChickens.GameState
{
    public class FoxLossState : GameState
    {
        public FoxLossState(GameStateManager stateManager) : base(stateManager){ }
        
        public override void StateEnter()
        {
            StateManager.resultsScreenManager.foxLossCanvas.SetActive(true);
        }

        public override void OnSceneLoad()
        {
            throw new System.NotImplementedException();
        }

        public override void StateExit()
        {
            StateManager.resultsScreenManager.foxLossCanvas.SetActive(false);
        }
    }
}