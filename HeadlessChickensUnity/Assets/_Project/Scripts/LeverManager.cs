using Photon.Pun;
using PixelPeeps.HeadlessChickens.UI;
using UnityEngine;

namespace PixelPeeps.HeadlessChickens.Network
{
    public class LeverManager : MonoBehaviourPunCallbacks
    {
        public int leversPulled = 0;
        public bool allLeversPulled = false;
        

        [PunRPC]
        public void RPC_AllLeversPulled()
        {
            Debug.Log("all levers pulled!");
            allLeversPulled = true;
        }

        [PunRPC]
        public void RPC_IncrementLeverCount()
        {
            leversPulled++;

            // if all levels are active open the exit
            if (leversPulled == NewGameManager.Instance.maxNumberOfLevers)
            {
                photonView.RPC("RPC_AllLeversPulled", RpcTarget.AllBufferedViaServer);
            }

            HUDManager.Instance.UpdateLeverCount(leversPulled);

        }
    }
}
