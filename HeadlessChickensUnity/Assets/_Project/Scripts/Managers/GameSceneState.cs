using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace PixelPeeps.HeadlessChickens.Managers
{
    public class GameSceneState : GameState
    {
        public GameSceneState(GameStateManager stateManager) : base(stateManager){ }
        
        public override void StateEnter()
        {
            Debug.Log("<color=green> Entered GameScene state </color>");
            LoadScene(StateManager.playScene);
        }

        public override void StateExit()
        {
            Debug.Log("<color=red> Exited GameScene state </color>");
            LoadScene(StateManager.menuScene);
        }

        public override void LoadScene(string scene)
        {
            SceneManager.LoadSceneAsync(scene);
        }
    }
}