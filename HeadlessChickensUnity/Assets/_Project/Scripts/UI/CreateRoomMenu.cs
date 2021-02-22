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

        public override void DisplayErrorMessage()
        {
            emptyRoomNameError.SetActive(true);
        }

        public override void HideErrorMessage()
        {
            emptyRoomNameError.SetActive(false);
        }
    }
}