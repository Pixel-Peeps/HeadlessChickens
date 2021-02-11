using UnityEngine;
using UnityEngine.SceneManagement;

namespace PixelPeeps.HeadlessChickens.GameState
{
    public class FoxWinState : GameState
    {
        public FoxWinState(GameStateManager stateManager) : base(stateManager){ }

        public override void StateEnter()
        {
            StateManager.LoadNextScene(StateManager.playScene);
            StateManager.foxWinCanvas.SetActive(true);
        }

        public override void StateExit()
        {
            StateManager.foxWinCanvas.SetActive(false);
        }
    }
}