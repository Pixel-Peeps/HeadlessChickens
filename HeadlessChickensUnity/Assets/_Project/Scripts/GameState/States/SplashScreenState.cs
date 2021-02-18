﻿using PixelPeeps.HeadlessChickens.UI;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace PixelPeeps.HeadlessChickens.GameState
{
    public class SplashScreenState : GameState
    {
        public override void StateEnter()
        {
            StateManager.LoadNextScene(StateManager.menuScene);
            
            if (StateManager.uiManager != null)
            {
                GameObject canvasObject = StateManager.uiManager.splashScreenCanvas;
                GameObject firstSelectedButton = StateManager.uiManager.splashScreenFirstSelected;
                
                StateManager.uiManager.ActivateCanvas(canvasObject, firstSelectedButton);
            }
        }

        public override void OnSceneLoad()
        {
            GameObject menuManagerObj = GameObject.FindGameObjectWithTag("MenuManager");
            StateManager.uiManager = menuManagerObj.GetComponent<UIManager>();

            GameObject canvasObject = StateManager.uiManager.splashScreenCanvas;
            GameObject firstSelectedButton = StateManager.uiManager.splashScreenFirstSelected;
            
            StateManager.uiManager.ActivateCanvas(canvasObject, firstSelectedButton);
        }

        public override void StateExit()
        {
            StateManager.uiManager.splashScreenCanvas.SetActive(false);
        }
    }
}