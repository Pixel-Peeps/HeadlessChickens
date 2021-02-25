using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeverHolder : MonoBehaviour
{
    Lever lever;
    
    // Start is called before the first frame update
    void Start()
    {
        lever = transform.GetChild(0).GetComponent<Lever>();
    }


    [PunRPC]
    public void RPC_EnableLever()
    {
        lever.gameObject.SetActive(true);
    }
}
