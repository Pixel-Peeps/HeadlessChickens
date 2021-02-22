using TMPro;
using UnityEngine;

namespace PixelPeeps.HeadlessChickens.UI
{
    public class MainMenu : Menu
    {
        public TMP_InputField playerNameInput;
        public GameObject emptyPlayerNameError;
        
        public override string GetInputFieldText()
        {
            return playerNameInput.text;
        }

        public override void DisplayErrorMessage()
        {
            emptyPlayerNameError.SetActive(true);
        }

        public override void HideErrorMessage()
        {
            emptyPlayerNameError.SetActive(false);
        }
    }
}