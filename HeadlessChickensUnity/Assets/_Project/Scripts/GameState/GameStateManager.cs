using System.Collections;
using PixelPeeps.HeadlessChickens.UI;
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

        [Header("GUI")] 
        [HideInInspector] public MenuManager menuManager;

        public GameObject chickenLossCanvas;
        public GameObject chickenWinCanvas;
        public GameObject foxLossCanvas;
        public GameObject foxWinCanvas;
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
            menuManager = FindObjectOfType<MenuManager>();
            
            currentState = new SplashScreenState(this);
            currentState.StateEnter();
        }

        public void SwitchGameState(GameState newState)
        {
            currentState.StateExit();

            currentState = newState;
            currentState.StateEnter();
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
                StartCoroutine(AsyncSceneLoadCoroutine(sceneName));
                //SceneManager.UnloadSceneAsync(activeScene);
            }
        }

        private IEnumerator AsyncSceneLoadCoroutine(string sceneName)
        {
            AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);

            while (!asyncLoad.isDone)
            {
                yield return new WaitForSecondsRealtime(0.2f);
                Debug.Log("Waiting for async load...");
            }
            
            Debug.Log("Async load complete!");
            
            GameObject menuManagerObj = GameObject.FindGameObjectWithTag("MenuManager");

            if (menuManagerObj != null)
            {
                Debug.Log("MenuManager object was found. Getting component..");
                menuManager = menuManagerObj.GetComponent<MenuManager>();

                if (menuManager != null)
                {
                    Debug.Log("MenuManager component was found! Executing state OnSceneLoad function");
                    currentState.OnSceneLoad();
                }
            }
            
            yield return null;
        }

        public void OnSceneLoad()
        {
            
        }
    }
}
