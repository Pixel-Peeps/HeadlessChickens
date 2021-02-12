using UnityEngine;
using UnityEngine.EventSystems;

namespace PixelPeeps.HeadlessChickens.UI
{
    public class MenuManager : MonoBehaviour
    {        
        public GameObject splashScreenCanvas;
        public GameObject splashScreenFirstSelected;
        
        public GameObject mainMenuCanvas;
        public GameObject mainMenuFirstSelected;
        
        public GameObject storeScreenCanvas;
        public GameObject storeScreenFirstSelected;

        public void SetEventSystemCurrentSelection(GameObject firstSelected)
        {
            EventSystem.current.SetSelectedGameObject(null);
            EventSystem.current.SetSelectedGameObject(firstSelected);
        }
    }
}