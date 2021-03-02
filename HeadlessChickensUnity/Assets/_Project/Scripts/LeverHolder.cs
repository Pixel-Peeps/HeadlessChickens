using System;
using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeverHolder : MonoBehaviourPunCallbacks
{
    GameObject lever;
    public bool isChildActive;

    // Start is called before the first frame update
    private void Awake()
    {
        lever = transform.GetChild(0).gameObject;
        
    }

    private void Start()
    {
        isChildActive = lever.activeSelf;
    }


    [PunRPC]
    public void RPC_EnableLever()
    {
        lever.gameObject.SetActive(true);
    }
}
