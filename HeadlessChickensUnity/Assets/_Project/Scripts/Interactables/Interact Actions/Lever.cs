using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using PixelPeeps.HeadlessChickens._Project.Scripts.Character;
using PixelPeeps.HeadlessChickens.Network;

public class Lever : MonoBehaviourPunCallbacks, IInteractable
{
    Interactable interactable;
    public LeverManager leverManager;

    public Animator animator;

    private void Start()
    {
        interactable = GetComponent<Interactable>();
        leverManager = GameObject.FindWithTag("LeverManager").GetComponent<LeverManager>();
    }

    public void Interact(CharacterBase characterBase)
    {
       // PhotonView gameManagerPhotonView = NewGameManager.Instance.GetComponent<PhotonView>();
       
        leverManager.photonView.RPC("RPC_IncrementLeverCount", RpcTarget.AllBufferedViaServer);
        interactable.photonView.RPC("RPC_ToggleInteractAllowed", RpcTarget.AllBufferedViaServer);

        photonView.RPC("PlayLeverAnimation", RpcTarget.AllBufferedViaServer);
        
        //interactable.interactAllowed = false;
    }

    public void InteractionFocus(bool focussed)
    {
        if (focussed && interactable.interactAllowed)
        {
            Debug.Log("InteractionFocus() true");
        }
        else
        {
            Debug.Log("InteractionFocus() false");
        }
        
    }

    [PunRPC]
    public void PlayLeverAnimation()
    {
        animator.Play("LeverOn");
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        
    }
}
