using UnityEngine;
using UnityEngine.EventSystems;

namespace PixelPeeps.HeadlessChickens.UI
{
    public class Menu : MonoBehaviour
    {
        public GameObject canvas;
        public GameObject firstSelectedButton;

        public void ActivateMenu()
        {
            if (canvas == null)
            {
                Debug.LogError
                ("The canvas object on " + gameObject.name + " Menu script is null. Set it in inspector.",
                    this);
                
                return;
            }

            canvas.SetActive(true);
            SetEventSystemCurrentSelection();
        }

        public void DeactivateMenu()
        {
            if (canvas == null)
            {
                Debug.LogError
                ("The canvas object on " + gameObject.name + " Menu script is null. Set it in inspector.",
                    this);
                
                return;
            }

            canvas.SetActive(false);
            SetEventSystemCurrentSelection();
        }
        
        private void SetEventSystemCurrentSelection()
        {
            if (firstSelectedButton == null)
            {
                Debug.LogError
                ("The firstSelectedButton object on " + gameObject.name +
                    " Menu script is null. Set it in inspector.", this);
                
                return;
            }

            EventSystem.current.SetSelectedGameObject(null);
            EventSystem.current.SetSelectedGameObject(firstSelectedButton);
        }

        public virtual string GetInputFieldText()
        {
            Debug.LogWarning("There is no input field object variable on the base Menu class; use a subclass", this);
            return null;
        }

        public virtual void DisplayErrorMessage()
        {
            Debug.LogWarning("There is no error message object variable on the base Menu class; use a subclass", this);
        }
        
        public virtual void HideErrorMessage()
        {
            Debug.LogWarning("There is no error message object variable on the base Menu class; use a subclass", this);
        }
    }
}
