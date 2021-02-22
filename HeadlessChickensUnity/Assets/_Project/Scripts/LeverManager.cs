using Photon.Pun;
using PixelPeeps.HeadlessChickens.UI;
using UnityEngine;

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
        HUDManager.Instance.UpdateLeverCount();

        // if all levels are active open the exit
        if (leversPulled == 4)
        {
            photonView.RPC(" RPC_AllLeversPulled", RpcTarget.AllBufferedViaServer);
        }
    }
}
