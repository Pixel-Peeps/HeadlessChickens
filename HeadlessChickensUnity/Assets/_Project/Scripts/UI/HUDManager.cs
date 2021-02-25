﻿using System;
using PixelPeeps.HeadlessChickens.Network;
using TMPro;
using UnityEngine;

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

        public void UpdateLeverCount(int newLeverCount)
        {
            int maximumLevers = NewGameManager.Instance.maxNumberOfLevers;
            leverCounter.text = $"{newLeverCount} /{maximumLevers}";
        }

        public void UpdateTimeDisplay(float timeToDisplay)
        {
            timeToDisplay += 1;
        
            float minutes = Mathf.FloorToInt(timeToDisplay / 60); 
            float seconds = Mathf.FloorToInt(timeToDisplay % 60);

            timerDisplay.text = $"{minutes:00}:{seconds:00}";
        }

        public void UpdateChickenCount()
        {
            
        }
    }
}