using System.Collections.Generic;
using Photon.Realtime;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

namespace PixelPeeps.HeadlessChickens.UI
{
    // Handles all the different canvas that are present in a scene. Interfaced with by the GameStates    
    public class UIManager : MonoBehaviour
    {
        [Header("Loading / Connecting Screens")] 
        public GameObject loadingScreenCanvas;
        public GameObject connectingScreenCanvas;

        [Header("Menus")] 
        public Menu mainMenu;
        public Menu howToPlay;
        public Menu roomSearch;
        public Menu createRoom;
        public Menu waitingRoom;
        public Menu connectionError;
        
        [Header("Waiting Room Info")]
        public GameObject startGameButton;
        public TextMeshProUGUI roomNameText;
        public TextMeshProUGUI roomPlayerCount;
        
        [Header("Player List")]
        public PlayerList playerList;
        
        [Header("Room List")]
        public RoomList roomList;

        public void ActivateMenu(Menu menu)
        {
            if (menu == null)
            {
                return;
            }
            
            menu.ActivateMenu();
        }

        public void DeactivateMenu(Menu menu)
        {
            if (menu == null)
            {
                return;
            }
            
            menu.DeactivateMenu();
        }

        private void SetEventSystemCurrentSelection(GameObject firstSelected)
        {
            EventSystem.current.SetSelectedGameObject(null);
            EventSystem.current.SetSelectedGameObject(firstSelected);
        }

        public void GenerateRoomList(List<RoomInfo> serverRoomList)
        {
            Debug.Log("<color=yellow> GenerateRoomList() called </color>");
            roomList.UpdateRoomList(serverRoomList);
        }

        public void UpdatePlayerList(Player newPlayer)
        {
            playerList.GeneratePlayerList(newPlayer);
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