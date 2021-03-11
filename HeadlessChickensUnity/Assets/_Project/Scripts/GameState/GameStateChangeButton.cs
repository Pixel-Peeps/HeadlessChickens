using System;
using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;
using WebSocketSharp;

namespace PixelPeeps.HeadlessChickens.GameState
{
    public class GameStateChangeButton : MonoBehaviour
    {
        // Mostly for testing changes between states, this will probably be changed as the point is to avoid enums 
        
        public eGameStates stateToChangeTo;
        private GameState newState;

        private GameStateManager manager;

        public void Start()
        {
            if (manager == null)
            {
                manager = FindObjectOfType<GameStateManager>();
                
                if (manager == null)
                {
                    Debug.LogError("Found NO GameStateManager on " + gameObject.name + ". Make sure you load the scenes starting from MenuScene in order to have a GameStateManager in scene"); 
                }
            }
            
            gameObject.GetComponent<Button>().onClick.AddListener(OnButtonClick);
        }

        public void OnButtonClick()
        {
            switch (stateToChangeTo)
            {
                case eGameStates.MainMenu:
                    newState = new MainMenuState();
                    break;
                
                case eGameStates.WaitingRoom:
                    newState = new WaitingRoomState();
                    break;
                
                case eGameStates.PlayScene:
                    newState = new GameSceneState();
                    break;

                case eGameStates.RoomSearch:
                    if (CheckPlayerNameForNull())
                    {
                        break;
                    }
                    
                    else
                    {
                        newState = new RoomSearchState();
                        break;
                    }
                
                case eGameStates.CreateRoom:
                    if (CheckPlayerNameForNull())
                    {
                        break;
                    }
                    
                    else
                    {
                        newState = new CreateRoomState();
                        break;
                    }
                
                case eGameStates.ConnectionError:
                    newState = new ConnectionErrorState("Called from button");
                    break;
                
                case eGameStates.HowToPlay:
                    newState = new HowToPlayState();
                    break;
                
                case eGameStates.ReturnToLobby:
                    newState = new ReturnToLobbyState();
                    break;
                
                case eGameStates.ReturnToMenu:
                    newState = new ReturnToMenuState();
                    break;
                
                default:
                    throw new ArgumentOutOfRangeException();
            }

            if (newState != null)
            {
                GameStateManager.Instance.SwitchGameState(newState);
            }
        }
        
        private bool CheckPlayerNameForNull()
        {
            string inputFieldText = manager.uiManager.mainMenu.GetInputFieldText();
            
            if (inputFieldText.IsNullOrEmpty())
            {
                manager.uiManager.mainMenu.DisplayErrorMessage();
                return true;
            }
            else
            {
                manager.uiManager.mainMenu.HideErrorMessage();
                PhotonNetwork.NickName = inputFieldText;
                return false;
            }
        }
    }
}