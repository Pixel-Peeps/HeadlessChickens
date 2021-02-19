﻿using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using PixelPeeps.HeadlessChickens._Project.Scripts.Character;

public class HidingSpot : MonoBehaviourPunCallbacks, IInteractable
{
    //public ChickenBehaviour chickenInSpot;
    public bool canAccessHiding = true;

    public void Interact(CharacterBase character)
    {
        photonView.SetControllerInternal(character.photonView.Owner.ActorNumber);
        if (photonView.IsMine)
        {
            if (canAccessHiding)
            {
                Debug.Log("Interact() method called, inUse false");
                character.currentHidingSpot = transform;
                character.HidingInteraction(canAccessHiding);
                photonView.RPC("RPC_ToggleAccess", RpcTarget.AllViaServer);
            }
            //else
            //{
            //    // character.currentHidingSpot.position = null;
            //    character.HidingInteraction(canAccessHiding);
            //    photonView.RPC("RPC_ToggleAccess", RpcTarget.AllViaServer);
            //}
        }
    }

    [PunRPC]
    public void RPC_ToggleAccess()
    {
        canAccessHiding = !canAccessHiding;
    }

    public void InteractionFocus(bool focussed)
    {

    }
}
