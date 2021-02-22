using Photon.Pun;
using PixelPeeps.HeadlessChickens.UI;
using UnityEngine;

namespace PixelPeeps.HeadlessChickens.Network
{
    public class LeverManager : MonoBehaviourPunCallbacks
    {
        public int leversPulled = 0;
        public bool allLeversPulled = false;

        [SerializeField] GameObject exits;

        [PunRPC]
        public void RPC_AllLeversPulled()
        {
            Debug.Log("all levers pulled!");
            allLeversPulled = true;

            // select random exit & open it
            int randomIndex = Random.Range(0, transform.childCount);
            ExitDoor chosenExit = exits.transform.GetChild(randomIndex).GetComponent<ExitDoor>();

            chosenExit.StartCoroutine(chosenExit.ActivateExit());
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
