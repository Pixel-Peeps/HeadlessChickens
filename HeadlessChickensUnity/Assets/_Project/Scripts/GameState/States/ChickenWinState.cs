using UnityEngine;
using UnityEngine.SceneManagement;

namespace PixelPeeps.HeadlessChickens.GameState
{
    public class ChickenWinState : GameState
    {
        public ChickenWinState(GameStateManager stateManager) : base(stateManager){ }
        
        public override void StateEnter()
        {
            StateManager.LoadNextScene(StateManager.playScene);
            StateManager.chickenWinCanvas.SetActive(true);
        }

        public override void OnSceneLoad()
        {
            throw new System.NotImplementedException();
        }

        public override void StateExit()
        {
            StateManager.chickenWinCanvas.SetActive(false);
        }
    }
}