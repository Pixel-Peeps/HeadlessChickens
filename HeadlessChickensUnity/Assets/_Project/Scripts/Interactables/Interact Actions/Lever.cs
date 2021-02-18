﻿using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using PixelPeeps.HeadlessChickens._Project.Scripts.Character;
using PixelPeeps.HeadlessChickens.Network;

public class Lever : MonoBehaviourPunCallbacks, IInteractable
{
    Interactable interactable;

    private void Awake()
    {
        interactable = GetComponent<Interactable>();
    }

    public void Interact(CharacterBase characterBase)
    {
        PhotonView gameManagerPhotonView = NewGameManager.Instance.GetComponent<PhotonView>();
       
        gameManagerPhotonView.RPC("RPC_IncrementLeverCount", RpcTarget.AllBufferedViaServer);
        gameManagerPhotonView.RPC("RPC_ToggleInteractAllowed", RpcTarget.AllBufferedViaServer);
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

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        
    }
}
