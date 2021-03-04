using TMPro;
using UnityEngine;
using UnityEngine.Assertions.Must;
using UnityEngine.UI;

namespace PixelPeeps.HeadlessChickens.UI
{
    public class MainMenuAnimations : MonoBehaviour
    {
        [Header("Trigger Button")] public Button triggerButton;

        [Header("Logo")] public UITweener logoTween;

        [Header("Host Game")] public Button hostGameButton;
        public TextMeshProUGUI hostGameText;
        public UITweener hostGameTextTween;
        public UITweener hostGameButtonTween;

        [Header("Join Game")] public Button joinGameButton;
        public TextMeshProUGUI joinGameText;
        public UITweener joinGameTextTween;
        public UITweener joinGameButtonTween;

        [Header("How To Play")] public Button howToPlayButton;
        public TextMeshProUGUI howToPlayText;
        public UITweener howToPlayTextTween;
        public UITweener howToPlayButtonTween;

        [Header("Name Input")] public TextMeshProUGUI nameInputTitleText;
        public TMP_InputField nameInputField;
        public TextMeshProUGUI inputFieldText;
        public TextMeshProUGUI inputAreaText;
        public UITweener inputImageTween;
        public UITweener inputTitleTween;
        public UITweener inputTextTween;
        public UITweener inputAreaTween;

        public void Start()
        {
            // Setting things to disabled / invisible when menu first loads
            triggerButton.enabled = true;
            triggerButton.onClick.AddListener(BeginTweenSequence);

            SetButtonsInvisible();

            SetTextInvisible();
        }

        private void SetTextInvisible()
        {
            Color textColour = howToPlayText.color;
            print(textColour.r);
            Color invisibleTextColour = new Color( textColour.r, textColour.g, textColour.b, 0);
            print(invisibleTextColour.r);
            
            howToPlayText.color = invisibleTextColour;
            joinGameText.color = invisibleTextColour;
            hostGameText.color = invisibleTextColour;
            nameInputTitleText.color = invisibleTextColour;
            inputAreaText.color = invisibleTextColour;

            Color inputTextColour = inputFieldText.color;
            Color32 invisibleInputTextColour = new Color32((byte) inputTextColour.r, (byte) inputTextColour.g,
                (byte) inputTextColour.b, 0);

            inputFieldText.color = invisibleInputTextColour;
        }

        private void SetButtonsInvisible()
        {
            Image howToPlayImage = howToPlayButton.gameObject.GetComponent<Image>();
            Image joinGameImage = joinGameButton.gameObject.GetComponent<Image>();
            Image hostGameImage = hostGameButton.gameObject.GetComponent<Image>();
            Image inputFieldImage = nameInputField.gameObject.GetComponent<Image>();

            Color buttonColour = howToPlayImage.color;
            Color invisibleButtonColour =
                new Color( buttonColour.r, buttonColour.g, buttonColour.b, 0);

            Color inputFieldColour = inputFieldImage.color;
            Color invisibleInputColour = new Color(inputFieldColour.r, inputFieldColour.g,
                inputFieldColour.b, 0);

            howToPlayImage.color = invisibleButtonColour;
            joinGameImage.color = invisibleButtonColour;
            hostGameImage.color = invisibleButtonColour;

            inputFieldImage.color = invisibleInputColour;
        }

        private void BeginTweenSequence()
        {
            triggerButton.enabled = false;
            Destroy(triggerButton.gameObject);

            logoTween.MoveToTarget();

            hostGameTextTween.FadeInText();
            hostGameButtonTween.FadeInImage();

            joinGameTextTween.FadeInText();
            joinGameButtonTween.FadeInImage();

            howToPlayTextTween.FadeInText();
            howToPlayButtonTween.FadeInImage();

            howToPlayButton.enabled = true;
            joinGameButton.enabled = true;
            hostGameButton.enabled = true;

            inputTitleTween.FadeInText();
            inputImageTween.FadeInImage();
            inputTextTween.FadeInText();
            inputAreaTween.FadeInText();

            nameInputField.enabled = true;
        }
    }
}