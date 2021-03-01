using UnityEngine;

namespace PixelPeeps.HeadlessChickens.GameState
{
    public class GameSceneState : GameState
    {
        public override void StateEnter()
        {
            Debug.Log("StateEnter GameSceneState");

            StateManager.LoadNextScene(StateManager.mainScene);
        }

        public override void OnSceneLoad()
        {
            
        }

        public override void StateExit()
        {
            Debug.Log("StateExit on GameSceneState");
        }
    }
}