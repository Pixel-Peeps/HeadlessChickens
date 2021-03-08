using Photon.Pun;
using Photon.Realtime;
using PixelPeeps.HeadlessChickens.Network;
using UnityEngine.UI;

namespace PixelPeeps.HeadlessChickens.UI
{
    public class KickPlayerButton : MonoBehaviourPunCallbacks
    {
        public Player targetPlayer;
    
        private void Start()
        {
            gameObject.GetComponent<Button>().onClick.AddListener(OnClick);
        }

        private void OnClick()
        {
            NetworkManager.Instance.KickPlayer(targetPlayer);
        }
    }
}
