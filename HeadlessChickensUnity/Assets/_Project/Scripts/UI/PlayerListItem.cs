using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine;

namespace PixelPeeps.HeadlessChickens.UI
{
    public class PlayerListItem : MonoBehaviourPunCallbacks
    {
        Player player;
        public TextMeshProUGUI nameText;

        public void SetUp(Player _player)
        {
            player = _player;
            nameText.text = player.NickName;

            if (player == PhotonNetwork.LocalPlayer)
            {
                nameText.color = Color.cyan;
            }
            else
            {
                nameText.color = Color.white;
            }
        }

        public override void OnPlayerLeftRoom(Player otherPlayer)
        {
            if (player == otherPlayer)
            {
                Debug.Log("Player that left is equal to: " + otherPlayer.NickName);
                Destroy(gameObject);
            }
        }

        public override void OnLeftRoom()
        {
            Debug.Log("Player left. Destroy game objects");
            Destroy(gameObject);
        }
    }
}