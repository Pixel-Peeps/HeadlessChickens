using System;
using System.Collections.Generic;
using Photon.Realtime;
using TMPro;
using UnityEngine;

namespace PixelPeeps.HeadlessChickens.UI
{
    // Handles all the different canvas that are present in a scene. Interfaced with by the GameStates    
    public class UIManager : MonoBehaviour
    {
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

        public void Awake()
        {
            
        }

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

        public void GenerateRoomList(List<RoomInfo> serverRoomList)
        {
            Debug.Log("<color=yellow> GenerateRoomList() called </color>");
            roomList.UpdateRoomList(serverRoomList);
        }

        public void UpdatePlayerList(Player newPlayer)
        {
            playerList.GeneratePlayerList(newPlayer);
        }
    }
}