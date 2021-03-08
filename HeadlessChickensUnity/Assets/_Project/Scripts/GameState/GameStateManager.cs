using System.Collections;
using Photon.Pun;
using PixelPeeps.HeadlessChickens.UI;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace PixelPeeps.HeadlessChickens.GameState
{
    public class GameStateManager : MonoBehaviourPunCallbacks
    {
        private static GameStateManager _instance;
        public static GameStateManager Instance
        {
            get => _instance;
            set => _instance = value;
        }

        private bool initialised;
        private GameState currentState;

        [Header("GUI")] 
        public UIManager uiManager;

        [Header("Loading / Connecting")] 
        public GameObject loadingScreen;
        public GameObject connectingScreen;
        public GameObject gameSetUpScreen;
        public Menu connectionErrorMenu;
        public TextMeshProUGUI connectionErrorText;
        
        [Header("Game Scenes")] 
        [HideInInspector] public string menuScene = "MenuScene";
        [HideInInspector] public string lobbyScene = "LobbyScene";
        [HideInInspector] public string mainScene = "MainScene";

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
            uiManager = FindObjectOfType<UIManager>();

            currentState = new MainMenuState();
            currentState.StateEnter();
        }

        public void SwitchGameState(GameState newState)
        {
            currentState.StateExit();

            currentState = newState;
            currentState.StateEnter();
        }

        public void LoadNextScene(string sceneName)
        {
            Debug.Log("LoadNextScene: " + sceneName);
            Scene activeScene = SceneManager.GetActiveScene();
            
            if (activeScene.name != sceneName)
            {
                StartCoroutine(AsyncSceneLoadCoroutine(sceneName));
            }
        }

        private IEnumerator AsyncSceneLoadCoroutine(string sceneName)
        {
            print("Loading scene: " + sceneName);
            // AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);
            PhotonNetwork.LoadLevel(sceneName);
            AsyncOperation asyncLoad = PhotonNetwork._AsyncLevelLoadingOperation;
            
            while (asyncLoad != null && !asyncLoad.isDone)
            {
                ShowLoadingScreen();
                yield return new WaitForSecondsRealtime(0.2f);
                HideLoadingScreen();
            }

            GameObject uiManagerObj = GameObject.FindGameObjectWithTag("MenuManager");

            if (uiManagerObj != null)
            {
                uiManager = uiManagerObj.GetComponent<UIManager>();
            }
            
            currentState.OnSceneLoad();
            
            yield return null;
        }

        public void ShowLoadingScreen()
        {
            loadingScreen.SetActive(true);
        }
        
        public void HideLoadingScreen()
        {
            if (loadingScreen != null)
            {
                loadingScreen.SetActive(false);
            }
        }
        
        //TODO - Fix bug with setup screen not disabling after set-up; may be related to PhotonView IDs
        public void ShowSetupScreen()
        {
            //gameSetUpScreen.SetActive(true);
        }
        
        public void HideSetupScreen()
        {
            //gameSetUpScreen.SetActive(false);
        }
        
        public void ShowConnectingScreen()
        {
            connectingScreen.SetActive(true);
        }
        
        public void HideConnectingScreen()
        {
            connectingScreen.SetActive(false);
        }

        public void ShowErrorScreen(string errorMessage)
        {
            connectionErrorMenu.ActivateMenu();
            connectionErrorText.text = errorMessage;
        }
        
        public void HideErrorScreen()
        {
            connectionErrorMenu.DeactivateMenu();
        }
    }
}
