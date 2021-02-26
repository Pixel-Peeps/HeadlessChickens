using Photon.Pun;
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
