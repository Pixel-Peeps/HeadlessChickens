using PixelPeeps.HeadlessChickens.UI;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace PixelPeeps.HeadlessChickens.GameState
{
    public class RoomSearchState : GameState
    {
        private readonly GameObject canvasObject = StateManager.uiManager.roomSearchCanvas;
        private readonly GameObject firstSelectedButton = StateManager.uiManager.roomSearchCanvasFirstSelected;

        private readonly string sceneName = StateManager.menuScene;
        
        public override void StateEnter()
        {
            StateManager.LoadNextScene(sceneName);
            ActivateCanvas(canvasObject, firstSelectedButton);
        }

        public override void OnSceneLoad()
        {
            menuManagerObj = GameObject.FindGameObjectWithTag("MenuManager");
            StateManager.uiManager = menuManagerObj.GetComponent<UIManager>();
            
            ActivateCanvas(canvasObject, firstSelectedButton);
        }

        public override void StateExit()
        {
            canvasObject.SetActive(false);
        }
    }
}