using System;
using TMPro;
using UnityEngine;
using PixelPeeps.HeadlessChickens.Network;

namespace PixelPeeps.HeadlessChickens.UI
{
    public class HUDManager : MonoBehaviour
    {
        [Header("Singleton Instance")] 
        private static HUDManager _instance;
        
        public static HUDManager Instance
        {
            get => _instance;
            set => _instance = value;
        }
        
        [Header("Managers")]

        public LeverManager leverManager;
        
        [Header("HUD Elements")]
        public TextMeshProUGUI leverCounter;
        public TextMeshProUGUI timerDisplay;
        public TextMeshProUGUI chickenCounter;

        public void Awake()
        {
            if (_instance != null && _instance != this)    
            {
                Destroy(gameObject);
            }
            else
            {    
                _instance = this;
            }
        }

        public void UpdateLeverCount()
        {
            int currentLeversPulled = leverManager.leversPulled;
            leverCounter.text = $"{currentLeversPulled} / 4";
        }

        public void UpdateTimeDisplay()
        {
            
        }

        public void UpdateChickenCount()
        {
            
        }
    }
}
