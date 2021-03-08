using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using PixelPeeps.HeadlessChickens.Network;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace PixelPeeps.HeadlessChickens.UI
{
    // Handles all the different canvas that are present in a scene. Interfaced with by the GameStates    
    public class UIManager : MonoBehaviourPun
    {
        [Header("Menus")] 
        public Menu mainMenu;
        public Menu howToPlay;
        public Menu roomSearch;
        public Menu createRoom;
        public Menu waitingRoom;
        
        [Header("Waiting Room Info")]
        public GameObject startGameButton;
        public TextMeshProUGUI roomNameText;
        public TextMeshProUGUI roomPlayerCount;
        public GameSettings gameSettings;
        
        [Header("Player List")]
        public PlayerList playerList;
        
        [Header("Room List")]
        public RoomList roomList;

        public static void ActivateMenu(Menu menu)
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

        public void UpdateRoomInfo()
        {
            string roomName = PhotonNetwork.CurrentRoom.Name;
            int playerCount = PhotonNetwork.CurrentRoom.PlayerCount;
            int maxPlayers = PhotonNetwork.CurrentRoom.MaxPlayers;
            Player[] playersInRoom = PhotonNetwork.PlayerList;
            
            // Update room name and player count
            roomNameText.text = roomName;
            roomPlayerCount.text =
                string.Format("In Room: {0} / {1}", playerCount , maxPlayers);
            
            // Generate player list
            playerList.GeneratePlayerList(playersInRoom);
            
            // Start game button
            startGameButton.SetActive(PhotonNetwork.IsMasterClient);
            startGameButton.GetComponent<Button>().enabled = (playerCount >= NetworkManager.minPlayersPerRoom);
            
            gameSettings.UpdateSettingsScreen();
        }
    }
}