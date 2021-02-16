using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace PixelPeeps.HeadlessChickens.UI
{
    // Handles all the different canvas that are present in a scene. Interfaced with by the GameStates    
    public class UIManager : MonoBehaviour
    {
        [Header("Loading and Connecting Screens")] 
        public GameObject loadingScreenCanvas;
        public GameObject connectingScreenCanvas;
        public GameObject connectionErrorCanvas;
        
        [Header("Main Menu")]
        public GameObject splashScreenCanvas;
        public GameObject splashScreenFirstSelected;
        
        public GameObject mainMenuCanvas;
        public GameObject mainMenuFirstSelected;

        [Header("Rooms")] 
        public GameObject roomSearchCanvas;
        public GameObject roomSearchCanvasFirstSelected;
        public TMP_InputField playerNameInputField;

        public GameObject createRoomCanvas;
        public GameObject createRoomCanvasFirstSelected;
        public TMP_InputField roomNameInputField;
        
        public GameObject waitingRoomCanvas;
        public GameObject waitingRoomFirstSelected;
        public GameObject startGameButton;

        [Header("Play Scene HUD")] 
        public GameObject playSceneHUDCanvas;

        public void ActivateCanvas(GameObject canvasObject, GameObject firstSelectedButton)
        {
            if (canvasObject == null)
            {
                return;
            }
            
            canvasObject.SetActive(true);
            SetEventSystemCurrentSelection(firstSelectedButton);
        }

        private void SetEventSystemCurrentSelection(GameObject firstSelected)
        {
            EventSystem.current.SetSelectedGameObject(null);
            EventSystem.current.SetSelectedGameObject(firstSelected);
        }
    }
}