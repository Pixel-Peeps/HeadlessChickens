using UnityEngine;
using UnityEngine.SceneManagement;

namespace PixelPeeps.HeadlessChickens.GameState
{
    public class GameStateManager : MonoBehaviour
    {
        private static GameStateManager _instance;
        public static GameStateManager Instance
        {
            get => _instance;
            set => _instance = value;
        }

        private bool initialised;
        private GameState currentState;

        #region GUI Elements

        [Header("GUI Prefabs")] 
        public GameObject splashScreenCanvas;
        public GameObject mainMenuCanvas;
        public GameObject storeScreenCanvas;
        public GameObject lobbyScreenCanvas;
        public GameObject playSceneHUD;
        public GameObject chickenLossCanvas;
        public GameObject chickenWinCanvas;
        public GameObject foxLossCanvas;
        public GameObject foxWinCanvas;
        
        // [Header("GUI Instances")]
        // public GameObject splashScreenCanvasInstance;
        // public GameObject mainMenuCanvasInstance;
        // public GameObject storeScreenCanvasInstance;
        // public GameObject lobbyScreenCanvasInstance;
        // public GameObject playSceneHUDInstance;
        // public GameObject chickenLossCanvasInstance;
        // public GameObject chickenWinCanvasInstance;
        // public GameObject foxLossCanvasInstance;
        // public GameObject foxWinCanvasInstance;

        #endregion
        
        [Header("Game Scenes")] 
        [HideInInspector] public string menuScene = "MenuScene";
        [HideInInspector] public string lobbyScene = "LobbyScene";
        [HideInInspector] public string playScene = "PlayScene";

        private void Awake()
        {
            DontDestroyOnLoad(gameObject);
            
            if (_instance != null && _instance != this)    
            {
                Destroy(gameObject);
            }
            else
            {
                _instance = this;
            }

            Initialise();
        }

        private void Initialise()
        {
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

        public GameObject InstantiateGUI(GameObject objectToInstantiate)
        {
            GameObject newInstance = Instantiate(objectToInstantiate);
            return newInstance;
        }

        public void DestroyGUI(GameObject objectToDestroy)
        {
            Destroy(objectToDestroy);
        }

        public void LoadNextScene(string sceneName)
        {
            Scene activeScene = SceneManager.GetActiveScene();
            
            if (activeScene.name != sceneName)
            {
                SceneManager.LoadSceneAsync(sceneName);
                //SceneManager.UnloadSceneAsync(activeScene);
            }
        }
    }
}
