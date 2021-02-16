﻿using System.Collections.Generic;
using System.Linq;
using Photon.Pun;
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
        public GameObject roomListParent;
        public GameObject roomListItemPrefab;
        [HideInInspector] public List<GameObject> currentRoomList = new List<GameObject>();

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
            if (currentRoomList.Any())
            {
                foreach (GameObject item in currentRoomList)
                {
                    Destroy(item);
                }
            
                currentRoomList.Clear();
            }

            foreach (RoomInfo t in roomList)
            {
                GameObject newRoomItem = Instantiate(roomListItemPrefab, roomListParent.transform);
                currentRoomList.Add(newRoomItem);
                RoomListItem roomScript = newRoomItem.GetComponent<RoomListItem>();
                roomScript.SetUp(t);
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