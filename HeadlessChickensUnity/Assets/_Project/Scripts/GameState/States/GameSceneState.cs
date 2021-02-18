using Photon.Pun;
using PixelPeeps.HeadlessChickens.Network;
using UnityEngine;

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
            Debug.Log("OnSceneLoad");
        }

        public override void StateExit()
        {
        }
    }
}