using Photon.Pun;
using UnityEngine;
using PixelPeeps.HeadlessChickens._Project.Scripts.Character;

public class HidingSpot : MonoBehaviourPunCallbacks, IInteractable
{
    //public ChickenBehaviour chickenInSpot;
    public bool inUse;

    public void Interact(CharacterBase character)
    {
        //character.photonView.RPC("RPC_HidingInteraction", RpcTarget.AllViaServer, this);
        character.currentHidingSpot = transform.position;
        character.HidingInteraction();
        inUse = true;
    }

    public void InteractionFocus(bool focussed)
    {

    }
}
