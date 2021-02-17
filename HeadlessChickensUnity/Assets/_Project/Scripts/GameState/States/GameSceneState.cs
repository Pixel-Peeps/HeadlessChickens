using System;
using PixelPeeps.HeadlessChickens.UI;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace PixelPeeps.HeadlessChickens.GameState
{
    public class GameSceneState : GameState
    {
        public override void StateEnter()
        {
            StateManager.LoadNextScene(StateManager.playScene);
        }

        public override void OnSceneLoad()
        {
            // GameObject resultsScreenManager = GameObject.FindGameObjectWithTag("ResultsScreenManager");
            //     
            // if (resultsScreenManager != null)
            // {
            //     StateManager.resultsScreenManager = resultsScreenManager.GetComponent<ResultsScreenManager>();
            // }
            //
            // else
            // {
            //     Debug.LogError("Object tagged ResultsScreenManager was not found in scene. Make sure to add it!");
            // }
        }

        public override void StateExit()
        {
        }
    }
}