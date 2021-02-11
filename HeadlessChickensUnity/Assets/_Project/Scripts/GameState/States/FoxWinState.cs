using UnityEngine;
using UnityEngine.SceneManagement;

namespace PixelPeeps.HeadlessChickens.GameState
{
    public class FoxWinState : GameState
    {
        public FoxWinState(GameStateManager stateManager) : base(stateManager){ }

        public override void StateEnter()
        {
            StateManager.resultsScreenManager.foxWinCanvas.SetActive(true);
        }

        public override void OnSceneLoad()
        {
            throw new System.NotImplementedException();
        }

        public override void StateExit()
        {
            StateManager.resultsScreenManager.foxWinCanvas.SetActive(false);
        }
    }
}