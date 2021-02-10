using UnityEngine;
using UnityEngine.SceneManagement;

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
        public GameObject playScreenCanvas;

        [Header("Game Scenes")] 
        [HideInInspector] public string menuScene = "MenuScene";
        [HideInInspector] public string lobbyScene = "LobbyScene";
        [HideInInspector] public string playScene = "PlayScene";

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
        }
    }
}
