using System;
using System.Collections;
using Photon.Pun;
using PixelPeeps.HeadlessChickens.Network;
using TMPro;
using UnityEngine;
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
        public LeverCounter leverCounter;
        public ChickCounter chickCounter;
        public TextMeshProUGUI timerDisplay;

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

            objectiveMsgTweener.ScaleUp();
            StartCoroutine(ObjectiveMessageCoroutine(lifetime));
        }

        private IEnumerator ObjectiveMessageCoroutine(float lifetime)
        {
            yield return new WaitForSecondsRealtime(lifetime);
            objectiveMsgTweener.ScaleDown();
        }
        
        public void UpdateTimeDisplay(float timeToDisplay)
        {
            timeToDisplay += 1;
        
            float minutes = Mathf.FloorToInt(timeToDisplay / 60); 
            float seconds = Mathf.FloorToInt(timeToDisplay % 60);

            timerDisplay.text = $"{minutes:00}:{seconds:00}";
        }

        public void GenerateChickIcons()
        {
            chickCounter.GenerateChickIcons();
        }
        
        public void UpdateChickCounter()
        {
            chickCounter.UpdateCounter();
        }
        
        public void GenerateLeverIcons()
        {
            leverCounter.GenerateLeverIcons();
        }

        public void UpdateLeverCounter(int currentLeverCount)
        {
            leverCounter.UpdateCounter(currentLeverCount);
        }
    }
}
