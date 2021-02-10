using UnityEngine;
using UnityEngine.SceneManagement;

namespace PixelPeeps.HeadlessChickens.Managers
{
    public class StoreScreenState : GameState
    {
        public StoreScreenState(GameStateManager stateManager) : base(stateManager){ }
        
        public override void StateEnter()
        {
            Debug.Log("<color=green> Entered StoreScreen state </color>");
            StateManager.storeScreenCanvas.SetActive(true);
        }

        public override void StateExit()
        {
            Debug.Log("<color=red> Exited StoreScreen state </color>");
            StateManager.storeScreenCanvas.SetActive(false);
        }

        public override void LoadScene(string scene)
        {
            SceneManager.LoadSceneAsync(scene);
        }
    }
}