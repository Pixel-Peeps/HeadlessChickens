using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine;
// ReSharper disable ConvertIfStatementToConditionalTernaryExpression

namespace PixelPeeps.HeadlessChickens.UI
{
    public class PlayerListItem : MonoBehaviourPunCallbacks
    {
        private Player player;
        public TextMeshProUGUI nameText;
        public KickPlayerButton kickButton;

        public void SetUp(Player _player)
        {
            player = _player;
            nameText.text = player.NickName;

            if (Equals(player, PhotonNetwork.LocalPlayer))
            {
                nameText.color = Color.cyan;
            }
            else
            {
                nameText.color = Color.white;
            }

            kickButton.targetPlayer = player;
        }

        public override void OnPlayerLeftRoom(Player otherPlayer)
        {
            if (Equals(player, otherPlayer))
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