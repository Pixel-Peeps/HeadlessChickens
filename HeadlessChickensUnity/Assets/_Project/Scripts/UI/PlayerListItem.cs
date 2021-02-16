using ExitGames.Client.Photon.Encryption;
using Photon.Pun;
using Photon.Realtime;
using PixelPeeps.HeadlessChickens.Network;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

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
                Destroy(gameObject);
            }
        }

        public override void OnLeftRoom()
        {
            Destroy(gameObject);
        }
    }
}