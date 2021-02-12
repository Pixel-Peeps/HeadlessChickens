using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Timer : MonoBehaviour
{
    public float totalTimeInSeconds;
    public float timeRemaining;
    public bool timerIsRunning;

    public TextMeshProUGUI timerText;

    void Start()
    {
        StartTimer();
    }
    
    void Update()
    {
        if (timerIsRunning)
        {
            if (timeRemaining > 0)
            {
                timeRemaining -= Time.deltaTime;
                DisplayTime(timeRemaining);
            }

            else
            {
                timerIsRunning = false;
                timeRemaining = 0;
            }
        }
    }
    
    void StartTimer()
    {
        timeRemaining = totalTimeInSeconds;
        timerIsRunning = true;
    }

    void DisplayTime(float timeToDisplay)
    {
        timeToDisplay += 1;
        
        float minutes = Mathf.FloorToInt(timeToDisplay / 60); 
        float seconds = Mathf.FloorToInt(timeToDisplay % 60);

        timerText.text = $"{minutes:00}:{seconds:00}";
    }
}