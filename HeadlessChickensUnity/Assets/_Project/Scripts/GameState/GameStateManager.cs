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
        [HideInInspector] public ResultsScreenManager resultsScreenManager;
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
            
            currentState = new SplashScreenState();
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
            // Pass to Launcher.cs on lobby / play scene load
            // Launcher will connect the player to the room, and load the lobby / play scene, synced for every player
            
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
                yield return new WaitForSecondsRealtime(0.2f);
            }
            
            currentState.OnSceneLoad();
            
            yield return null;
        }
    }
}
