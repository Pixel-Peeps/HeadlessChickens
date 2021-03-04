using Photon.Pun;
using PixelPeeps.HeadlessChickens.Network;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedMember.Local
// ReSharper disable ConvertIfStatementToConditionalTernaryExpression

namespace PixelPeeps.HeadlessChickens.UI
{
    public class GameSettings : MonoBehaviourPunCallbacks
    {
        [Header("UI Elements")]
        public GameObject plusTimeButton;
        public GameObject minusTimeButton;
        public TextMeshProUGUI gameTimeText;
        
        public Button privateButton;
        
        [Header("Private Button Colours")]
        public ColorBlock privateButtonColours;
        
        [Header("Public Button Colours")]
        public ColorBlock publicButtonColours;
        
        private TextMeshProUGUI privateBtnText;

        [Header("Settings")] 
        private readonly float[] gameTimeOptions =
        {
            30f,
            300f,
            600f,
            900f
        };

        private int gameTimeIndex = 2;
        private float gameTime;    
        private bool privateRoom;
        
        public void UpdateSettingsScreen()
        {            
            if (PhotonNetwork.IsMasterClient)
            {
                plusTimeButton.SetActive(true);
                minusTimeButton.SetActive(true);
                privateButton.enabled = true;
            }
            else
            {
                plusTimeButton.SetActive(false);
                minusTimeButton.SetActive(false);
                
                privateButton.enabled = false;
            }

            privateBtnText = privateButton.gameObject.GetComponentInChildren<TextMeshProUGUI>();

            if (PhotonNetwork.IsMasterClient)
            {
                gameTime = gameTimeOptions[gameTimeIndex];
            
                photonView.RPC("UpdateTimeText", RpcTarget.All, gameTime, gameTimeIndex);
                photonView.RPC("UpdateAccessButton", RpcTarget.All, privateRoom);
            }
        }

        public void IncreaseGameTime()
        {
            if (!PhotonNetwork.IsMasterClient) return;

            int newIndex = gameTimeIndex + 1;

            if (newIndex == gameTimeOptions.Length - 1)
            {
                plusTimeButton.SetActive(false);
                gameTimeIndex = gameTimeOptions.Length - 1;
            }

            if (newIndex > 0)
            {
                minusTimeButton.SetActive(true);
                gameTimeIndex = newIndex;
            }

            gameTime = gameTimeOptions[gameTimeIndex];
            NetworkManager.Instance.UpdateGameTime(gameTime);
            photonView.RPC("UpdateTimeText", RpcTarget.All, gameTime, gameTimeIndex);
        }
        
        public void DecreaseGameTime()
        {
            if (!PhotonNetwork.IsMasterClient) return;

            int newIndex = gameTimeIndex - 1;

            if (newIndex < gameTimeOptions.Length - 1)
            {
                plusTimeButton.SetActive(true);
                gameTimeIndex = newIndex;
            }
            
            if (newIndex == 0)
            {
                minusTimeButton.SetActive(false);
                gameTimeIndex = 0;
            }
            
            gameTime = gameTimeOptions[gameTimeIndex];
            NetworkManager.Instance.UpdateGameTime(gameTime);
            photonView.RPC("UpdateTimeText", RpcTarget.All, gameTime, gameTimeIndex);
        }
        
        public void UpdateRoomAccess()
        {
            if (!PhotonNetwork.IsMasterClient) return;

            privateRoom = !privateRoom;

            switch (privateRoom)
            {
                case true:
                    PhotonNetwork.CurrentRoom.IsOpen = false;
                    PhotonNetwork.CurrentRoom.IsVisible = false;
                    break;
                
                case false:
                    PhotonNetwork.CurrentRoom.IsOpen = true;
                    PhotonNetwork.CurrentRoom.IsVisible = true;
                    break;
            }
            
            photonView.RPC("UpdateAccessButton", RpcTarget.All, privateRoom);
        }

        [PunRPC]
        public void UpdateAccessButton(bool _privateRoom)
        {
            switch (_privateRoom)
            {
                case true:
                    privateBtnText.text = "Private";
                    privateButton.colors = privateButtonColours;
                    break;
                
                case false:
                    privateBtnText.text = "Public";
                    privateButton.colors = publicButtonColours;
                    break;
            }
        }
        
        [PunRPC]
        public void UpdateTimeText(float time, int index)
        {
            gameTime = time;
            gameTimeIndex = index;
            
            float minutes = Mathf.FloorToInt(time / 60); 
            float seconds = Mathf.FloorToInt(time % 60);
            
            gameTimeText.text = $"{minutes:00}:{seconds:00}";
        }
    }
}