using System.Collections.Generic;
using System.Security.Cryptography;
using System.Threading;
using ExitGames.Client.Photon.StructWrapping;
using Photon.Realtime;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

namespace PixelPeeps.HeadlessChickens.UI
{
    // Handles all the different canvas that are present in a scene. Interfaced with by the GameStates    
    public class UIManager : MonoBehaviour
    {
        [Header("Loading and Connecting Screens")] 
        public GameObject loadingScreenCanvas;
        public GameObject connectingScreenCanvas;
        
        public GameObject connectionErrorCanvas;
        public GameObject connectionErrorFirstSelected;
        
        [Header("Main Menu")]
        public GameObject mainMenuCanvas;
        public GameObject mainMenuFirstSelected;

        [Header("Room Search")]
        public GameObject roomSearchCanvas;
        public GameObject roomSearchCanvasFirstSelected;
        public GameObject enterNamePrompt;
        public TMP_InputField joiningPlayerNameInputField;
        
        [Header("Room List")]
        public Transform roomListParent;
        public GameObject roomListItemPrefab;
        [HideInInspector] public List<RoomListItem> currentRoomListings = new List<RoomListItem>();

        [Header("Room Creation")]
        public GameObject createRoomCanvas;
        public GameObject createRoomCanvasFirstSelected;
        public GameObject createRoomEnterNicknamePrompt;
        public GameObject createRoomEnterRoomNamePrompt;
        public TMP_InputField roomNameInputField;
        public TMP_InputField hostPlayerNameInputField;
        
        [Header("Waiting Room")]
        public GameObject waitingRoomCanvas;
        public GameObject waitingRoomFirstSelected;
        public GameObject startGameButton;
        public TextMeshProUGUI roomNameText;
        public TextMeshProUGUI roomPlayerCount;
        
        [Header("Player List")]
        public GameObject playerListParent;
        public GameObject playerListItemPrefab;
        [HideInInspector] public List<GameObject> currentPlayerList = new List<GameObject>();

        [Header("Play Scene HUD")] 
        public GameObject playSceneHUDCanvas;

        public void ActivateCanvas(GameObject canvasObject, GameObject firstSelectedButton)
        {
            if (canvasObject == null)
            {
                return;
            }
            
            canvasObject.SetActive(true);
            SetEventSystemCurrentSelection(firstSelectedButton);
        }

        private void SetEventSystemCurrentSelection(GameObject firstSelected)
        {
            EventSystem.current.SetSelectedGameObject(null);
            EventSystem.current.SetSelectedGameObject(firstSelected);
        }

        public void GenerateRoomList(List<RoomInfo> roomList)
        {
            Debug.Log("<color=yellow> GenerateRoomList() called </color>");

            foreach (RoomInfo info in roomList)
            {
                if (info.RemovedFromList)
                {
                    // Find index of removed room and destroy the game object it is attached to
                    int index = currentRoomListings.FindIndex(x => x.roomInfo.Name == info.Name);
                    if (index != -1)
                    {
                        Destroy(currentRoomListings[index].gameObject);
                        currentRoomListings.RemoveAt(index);
                    }
                }
                
                else
                {
                    int index = currentRoomListings.FindIndex(x => x.roomInfo.Name == info.Name);
                    if (index != -1)
                    {
                        Destroy(currentRoomListings[index].gameObject);
                        currentRoomListings.RemoveAt(index);
                    }
                    
                    GameObject newRoomItem = Instantiate(roomListItemPrefab, roomListParent);
                    newRoomItem.name = info.Name;
                    RoomListItem roomScript = newRoomItem.GetComponent<RoomListItem>();
                    roomScript.SetUp(info);
                    currentRoomListings.Add(roomScript);
                    Debug.Log("<color=green> Room generated: " + info.Name + "</color>");
                }
            }
        }

        public void UpdatePlayerList(Player newPlayer)
        {
            Debug.Log("UpdatePlayerList");
            
            GameObject newPlayerListItem = Instantiate(playerListItemPrefab, playerListParent.transform);
            PlayerListItem itemScript = newPlayerListItem.GetComponent<PlayerListItem>();
            itemScript.SetUp(newPlayer);
        }

        public void ShowLoadingScreen()
        {
            loadingScreenCanvas.SetActive(true);
        }

        public void HideLoadingScreen()
        {
            loadingScreenCanvas.SetActive(false);
        }

        public void ShowConnectionScreen()
        {
            connectingScreenCanvas.SetActive(true);
        }

        public void HideConnectionScreen()
        {
            connectingScreenCanvas.SetActive(false);
        }
    }
}