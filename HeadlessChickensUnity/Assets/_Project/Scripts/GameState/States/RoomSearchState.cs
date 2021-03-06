﻿using PixelPeeps.HeadlessChickens.UI;
using UnityEngine;

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
            StateManager.uiManager = GameObject.FindGameObjectWithTag("MenuManager").GetComponent<UIManager>();
            StateManager.uiManager.DeactivateMenu(StateManager.uiManager.mainMenu);
            StateManager.LoadNextScene(sceneName);
            ActivateMenu(menu);
        }

        public override void OnSceneLoad()
        {
            ActivateMenu(menu);
        }

        public override void StateExit()
        {
            DeactivateMenu(menu);
        }
    }
}