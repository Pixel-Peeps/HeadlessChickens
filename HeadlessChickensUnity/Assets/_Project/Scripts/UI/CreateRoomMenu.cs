using TMPro;
using UnityEngine;

namespace PixelPeeps.HeadlessChickens.UI
{
    public class CreateRoomMenu : Menu
    {
        public TMP_InputField roomNameInput;
        public GameObject emptyRoomNameError;
        
        public override string GetInputFieldText()
        {
            return roomNameInput.text;
        }
        
        public override void SetInputFieldText(string text)
        {
            roomNameInput.text = text;
        }

        public override void DisplayErrorMessage()
        {
            emptyRoomNameError.SetActive(true);
        }

        public override void HideErrorMessage()
        {
            Debug.Log("Hiding error message");
            
            if (emptyRoomNameError != null)
            {
                emptyRoomNameError.SetActive(false);
                Debug.Log("Hid error message");
            }
        }
    }
}