﻿using PixelPeeps.HeadlessChickens.UI;
using UnityEngine;

namespace PixelPeeps.HeadlessChickens.GameState
{
    public class CreateRoomState : GameState
    {
        private readonly Menu 
            menu = StateManager.uiManager.createRoom;

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