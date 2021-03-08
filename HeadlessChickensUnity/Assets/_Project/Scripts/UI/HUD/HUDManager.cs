using System.Collections;
using Photon.Pun;
using PixelPeeps.HeadlessChickens.Network;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// ReSharper disable UnusedMember.Global

namespace PixelPeeps.HeadlessChickens.UI
{
    public class HUDManager : MonoBehaviourPunCallbacks
    {
        [field: Header("Singleton Instance")]
        public static HUDManager Instance { get; private set; }

        [Header("HUD Elements")] 
        public TextMeshProUGUI objectiveMessage;
        public UITweener objectiveMsgTweener;
        public Image interactButton;
        public TextMeshProUGUI interactText;
        public Image itemButton;
        public Image dock;
        
        [Header("Fox HUD")]
        public Sprite foxInteract;
        public Sprite foxItem;
        public Sprite foxDock;
        public Color foxTimeTextColour;
        public Color foxLowTimeTextColour;

        #region Initialisation

        public void Awake()
        {
            if (Instance != null && Instance != this)    
            {
                Destroy(gameObject);
            }
            else
            {    
                Instance = this;
            }
        }

        public void Start()
        {
            objectiveMessage.gameObject.SetActive(false);
        }

        public void Initialise()
        {
            if (NewGameManager.Instance.myType == PlayerType.Fox)
            {
                SwitchToFoxHUD();
            }
            
            GenerateChickIcons();
            GenerateLeverIcons();

            InitialiseSpectatorHUD();
            InitialiseTimer();
            
            dock.gameObject.SetActive(true);
            
            interactButton.gameObject.SetActive(true);
            UpdateInteractionText();
            
            itemButton.gameObject.SetActive(true);
        }

        private void SwitchToFoxHUD()
        {
            interactButton.sprite = foxInteract;
            itemButton.sprite = foxItem;
            dock.sprite = foxDock;
            defaultTimerColour = foxTimeTextColour;
            redTimerColour = foxLowTimeTextColour;
            timerDisplay.color = foxTimeTextColour;
        }

        #endregion

        #region Objective Message
        public void DisplayObjectiveMessage(PlayerType playerType, float lifetime, string foxText, string chickText)
        {
            objectiveMessage.gameObject.SetActive(true);
            
            switch (playerType)
            {
                case PlayerType.Fox:
                    objectiveMessage.text = foxText;
                    break;
                
                case PlayerType.Chick:
                    objectiveMessage.text = chickText;
                    break;
            }

            objectiveMsgTweener.ScaleUpFromZero();
            StartCoroutine(ObjectiveMessageCoroutine(lifetime));
        }

        private IEnumerator ObjectiveMessageCoroutine(float lifetime)
        {
            yield return new WaitForSecondsRealtime(lifetime);
            objectiveMsgTweener.ScaleDown(true);
        }
        #endregion

        #region Interaction Text

        public void UpdateInteractionText( Interactable interactable )
        {
            int interactType = interactable.GetInteractionType();

            if ( !interactable.interactAllowed ) return;
            
            switch ( interactType )
            {
                case 0: // LEVER
                    interactText.text = "ACTIVATE";
                    break;
                
                case 1: // HIDE
                    interactText.text = "HIDE";
                    if ( NewGameManager.Instance.localChickBehaviour.isHiding )
                    {
                        interactText.text = "LEAVE";
                    }
                    break;
                
                case 2: // SHORTCUT
                    interactText.text = "SHORTCUT";
                    break;
            }
            
            // interactText.text = newText;
            // Color c = interactButton.color;
            //
            // interactButton.color = new Color(c.r, c.g, c.b, 1f);
            //
            // if ( newText == "" )
            // {
            //     interactButton.color = new Color(c.r, c.g, c.b, 0.5f);
            // } 
        }
        
        public void UpdateInteractionText( )
        {
            interactText.text = "";
        }
        
        public void UpdateInteractionText( string newText )
        {
            interactText.text = newText;
        }

        #endregion
        
        #region Timer

        [Header("Timer")]
        public TextMeshProUGUI timerDisplay;
        public Color32 defaultTimerColour = new Color32(147, 147, 147, 255);
        public Color32 redTimerColour = new Color32(206, 78, 78, 255);
        public UITweener timerTween;
        private bool hitHalfPoint;
        private bool hitLowTimePoint;

        private void InitialiseTimer()
        {
            timerDisplay.gameObject.SetActive(true);
            timerDisplay.color = defaultTimerColour;
        }
        
        public void UpdateTimeDisplay(float timeToDisplay)
        {          
            if (timeToDisplay < NewGameManager.Instance.totalGameTime / 2 && !timerTween.currentlyTweening && !hitHalfPoint)
            {
                timerTween.ScaleUpAndDown();
                hitHalfPoint = true;
            }
            
            if (timeToDisplay < 20 && !timerTween.currentlyTweening && !hitLowTimePoint)
            {
                timerTween.ScaleUpAndDown();
                
                timerDisplay.color = redTimerColour;
                
                hitLowTimePoint = true;
            }
            
            timeToDisplay += 1;
        
            float minutes = Mathf.FloorToInt(timeToDisplay / 60); 
            float seconds = Mathf.FloorToInt(timeToDisplay % 60);

            timerDisplay.text = $"{minutes:00}:{seconds:00}";
        }
        
        #endregion

        #region Counters
        
        public LeverCounter leverCounter;
        public ChickCounter chickCounter;

        private void GenerateChickIcons()
        {
            chickCounter.GenerateChickIcons();
        }
        
        public void UpdateChickCounter()
        {
            chickCounter.UpdateCounter();
        }

        private void GenerateLeverIcons()
        {
            leverCounter.GenerateLeverIcons();
        }

        public void UpdateLeverCounter(int currentLeverCount)
        {
            leverCounter.UpdateCounter(currentLeverCount);
        }
        #endregion

        #region Spectator Mode

        public GameObject spectatorCanvas;
        public TextMeshProUGUI spectatingText;

        private void InitialiseSpectatorHUD()
        {
            spectatorCanvas.SetActive(false);
        }
        
        public void EnableSpectatorHUD()
        {
            interactButton.gameObject.SetActive(false);
            itemButton.gameObject.SetActive(false);
            spectatorCanvas.SetActive(true);
        }

        public void UpdateSpectatorHUD(string currentPlayerName)
        {
            spectatingText.text = $"Spectating: {currentPlayerName}";
        }
        
        #endregion
    }
}
