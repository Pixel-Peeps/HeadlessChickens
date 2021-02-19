﻿using Photon.Pun;
using UnityEngine;

public class LeverManager : MonoBehaviourPunCallbacks
{
    public int leversPulled = 0;
    public bool allLeversPulled = false;
    
    
    void Update()
    {
        // if all levels are active open the exit
        if(leversPulled == 4)
        {
            photonView.RPC(" RPC_AllLeversPulled", RpcTarget.AllBufferedViaServer);
        }
    }

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
    }
}