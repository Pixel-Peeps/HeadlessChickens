using UnityEngine;

namespace PixelPeeps.HeadlessChickens.Managers
{
    public class GameStateManager : MonoBehaviour
    {
        private static GameStateManager _instance;
        public static GameStateManager Instance
        {
            get => _instance;
            set => _instance = value;
        }

        private GameState currentState;
        
        [Header("GUI Elements")] 
        public GameObject splashScreenCanvas;
        public GameObject mainMenuCanvas;
        public GameObject storeScreenCanvas;
        public GameObject lobbyScreenCanvas;
        public GameObject gameScreenCanvas;

        private void Awake()
        {
            DontDestroyOnLoad(gameObject);
            
            if (_instance != null && _instance != this)    
            {
                Destroy(this.gameObject);
            }
            else
            {
                _instance = this;
            }

            Initialise();
        }

        private void Initialise()
        {
            LoadScreenGUI();
            
            currentState = new SplashScreenState(this);
            currentState.StateEnter();
        }

        public void SwitchGameState(GameState newState)
        {
            currentState.StateExit();

            currentState = newState;
            currentState.StateEnter();
        }

        public void LoadScreenGUI()
        {
            splashScreenCanvas = Instantiate(splashScreenCanvas);
            splashScreenCanvas.SetActive(false);
            
            mainMenuCanvas = Instantiate(mainMenuCanvas);
            mainMenuCanvas.SetActive(false);
            
            storeScreenCanvas = Instantiate(storeScreenCanvas);
            storeScreenCanvas.SetActive(false);
            
            lobbyScreenCanvas = Instantiate(lobbyScreenCanvas);
            lobbyScreenCanvas.SetActive(false);
            
            gameScreenCanvas = Instantiate(gameScreenCanvas);
            gameScreenCanvas.SetActive(false);
        }
    }
}
