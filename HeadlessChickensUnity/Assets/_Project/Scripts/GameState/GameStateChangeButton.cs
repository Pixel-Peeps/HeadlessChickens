using System;
using UnityEngine;
using UnityEngine.UI;

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
                case eGameStates.SplashScreen:
                    newState = new SplashScreenState(GameStateManager.Instance);
                    break;
                
                case eGameStates.MainMenu:
                    newState = new MainMenuState(GameStateManager.Instance);
                    break;
                
                case eGameStates.StoreScreen:
                    newState = new StoreScreenState(GameStateManager.Instance);
                    break;
                
                case eGameStates.LobbyScene:
                    newState = new LobbySceneState(GameStateManager.Instance);
                    break;
                
                case eGameStates.PlayScene:
                    newState = new GameSceneState(GameStateManager.Instance);
                    break;
                
                case eGameStates.ChickenWin:
                    newState = new ChickenWinState(GameStateManager.Instance);
                    break;
                
                case eGameStates.ChickenLoss:
                    newState = new ChickenLossState(GameStateManager.Instance);
                    break;
                
                case eGameStates.FoxWin:
                    newState = new FoxWinState(GameStateManager.Instance);
                    break;
                
                case eGameStates.FoxLoss:
                    newState = new FoxLossState(GameStateManager.Instance);
                    break;
                
                default:
                    throw new ArgumentOutOfRangeException();
            }
            
            GameStateManager.Instance.SwitchGameState(newState);
        }
    }
}