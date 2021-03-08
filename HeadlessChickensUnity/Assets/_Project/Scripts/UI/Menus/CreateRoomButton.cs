using Photon.Pun;
using PixelPeeps.HeadlessChickens.Network;
using UnityEngine;
using UnityEngine.UI;
using WebSocketSharp;

namespace PixelPeeps.HeadlessChickens.UI
{
    public class CreateRoomButton : MonoBehaviour
    {
        private Button thisButton;
        private UIManager uiManager;

        private void Start()
        {
            uiManager = GameObject.FindGameObjectWithTag("MenuManager").GetComponent<UIManager>();
            thisButton = this.GetComponent<Button>();
            
            thisButton.onClick.AddListener(OnClick);
        }

        private void OnClick()
        {
            Menu createRoomMenu = uiManager.createRoom;
            bool roomNameEmpty = createRoomMenu.GetInputFieldText().IsNullOrEmpty();
            
            if (roomNameEmpty)
            {
                createRoomMenu.DisplayErrorMessage();
            }
            else
            {
                if (PhotonNetwork.CurrentRoom != null)
                {
                    NetworkManager.Instance.LeaveRoom();
                }
                
                createRoomMenu.HideErrorMessage();
                NetworkManager.Instance.CreateRoom();
                Debug.Log("created room");
            }
        }
    }
}