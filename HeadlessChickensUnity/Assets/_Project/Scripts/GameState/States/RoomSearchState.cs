﻿using PixelPeeps.HeadlessChickens.UI;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace PixelPeeps.HeadlessChickens.GameState
{
    public class RoomSearchState : GameState
    {
        private readonly Menu 
            menu = StateManager.uiManager.roomSearch;

        private readonly string 
            sceneName = StateManager.menuScene;
        
        public override void StateEnter()
        {
            StateManager.LoadNextScene(sceneName);
            ActivateMenu(menu);
        }

        public override void OnSceneLoad()
        {
            menuManagerObj = GameObject.FindGameObjectWithTag("MenuManager");
            StateManager.uiManager = menuManagerObj.GetComponent<UIManager>();
            
            ActivateMenu(menu);
        }

        public override void StateExit()
        {
            DeactivateMenu(menu);
        }
    }
}