using System.Collections;
using Photon.Pun;
using PixelPeeps.HeadlessChickens.UI;
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

        #region GUI Elements

        [Header("GUI")] 
        [HideInInspector] public UIManager uiManager;

        [Header("Loading / Connecting")] 
        public GameObject loadingScreen;
        public GameObject connectingScreen;
        public GameObject connectionErrorScreen;
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
            }
        }

        private IEnumerator AsyncSceneLoadCoroutine(string sceneName)
        {
            // AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);
            PhotonNetwork.LoadLevel(sceneName);
            AsyncOperation asyncLoad = PhotonNetwork._AsyncLevelLoadingOperation;
            
            while (!asyncLoad.isDone)
            {
                ShowLoadingScreen();
                yield return new WaitForSecondsRealtime(0.2f);
            }
            
            HideLoadingScreen();
            uiManager = FindObjectOfType<UIManager>();
            currentState.OnSceneLoad();
            
            yield return null;
        }

        public void ShowLoadingScreen()
        {
            loadingScreen.SetActive(true);
        }
        
        public void ShowConnectingScreen()
        {
            connectingScreen.SetActive(true);
        }
        
        public void HideLoadingScreen()
        {
            if (loadingScreen != null)
            {
                loadingScreen.SetActive(false);
            }
        }
        
        public void HideConnectingScreen()
        {
            connectingScreen.SetActive(false);
        }
    }
}
