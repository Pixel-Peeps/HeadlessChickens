using System;
using UnityEngine;
using UnityEngine.UI;

namespace PixelPeeps.HeadlessChickens.Managers
{
    public class GameStateChangeButton : MonoBehaviour
    {
        public eGameStates stateToChangeTo;
        private GameState newState;

        public void Start()
        {
            gameObject.GetComponent<Button>().onClick.AddListener(OnButtonClick);
        }

        private void OnButtonClick()
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
                    //newState = new StoreScreen(GameStateManager.Instance);
                    break;
                
                case eGameStates.LobbyScreen:
                    //newState = new LobbyScreen(GameStateManager.Instance);
                    break;
                
                case eGameStates.GameScreen:
                    //newState = new GameScreen(GameStateManager.Instance);
                    break;
                
                default:
                    throw new ArgumentOutOfRangeException();
            }
            
            GameStateManager.Instance.SwitchGameState(newState);
        }
    }
}