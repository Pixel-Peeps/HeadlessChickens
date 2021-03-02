using System;
using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeverHolder : MonoBehaviourPunCallbacks
{
    public GameObject lever;
    public bool isChildFake = true;
    

    // Start is called before the first frame update
    private void Awake()
    {
        isChildFake = true;
        lever = transform.GetChild(0).gameObject;
        
    }

    private void Start()
    {
       // isChildActive = lever.activeSelf;
    }

    public void ShowBlueprintOnLever()
    {
        Debug.Log("asking child to show blueprint");
        
            //lever.gameObject.SetActive(true);
            lever.gameObject.GetComponentInChildren<Lever>().ShowBlueprints();
    }

    [PunRPC]
    public void RPC_EnableLever()
    {
        //lever.gameObject.SetActive(true);
        lever.gameObject.GetComponentInChildren<Lever>().isFake = false;
        lever.gameObject.GetComponentInChildren<Lever>().photonView.RPC("RPC_SetUp", RpcTarget.AllViaServer,false);
    }
}
