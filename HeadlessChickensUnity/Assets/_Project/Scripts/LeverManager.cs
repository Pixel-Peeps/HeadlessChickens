using Photon.Pun;
using PixelPeeps.HeadlessChickens.UI;
using UnityEngine;

namespace PixelPeeps.HeadlessChickens.Network
{
    public class LeverManager : MonoBehaviourPunCallbacks
    {
        public int leversPulled = 0;
        public bool allLeversPulled = false;

        public int exitIndex;
        public ExitDoor chosenExit;

        public void Start()
        {
            exitIndex = Random.Range(0, NewGameManager.Instance.exits.Count);
            chosenExit = NewGameManager.Instance.exits[exitIndex];
        }

        [PunRPC]
        public void RPC_AllLeversPulled()
        {
            Debug.Log("all levers pulled!");
            allLeversPulled = true;

            if (PhotonNetwork.IsMasterClient)
            {
                chosenExit.exitActive = true;
                // chosenExit.StartCoroutine(chosenExit.ActivateExit());
            }
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
