using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using PixelPeeps.HeadlessChickens._Project.Scripts.Character;

public class Lever : MonoBehaviourPunCallbacks, IInteractable
{
    public LeverManager leverManager;
    Interactable interactable;

    private void Awake()
    {
        interactable = GetComponent<Interactable>();
    }

    public void Interact(CharacterBase characterBase)
    {
        leverManager.IncrementLeverCount();
        
        photonView.RPC("RPC_ToggleInteractAllowed", RpcTarget.AllBufferedViaServer);
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
