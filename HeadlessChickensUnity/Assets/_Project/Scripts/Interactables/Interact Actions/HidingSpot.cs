using Photon.Pun;
using UnityEngine;
using PixelPeeps.HeadlessChickens._Project.Scripts.Character;

public class HidingSpot : MonoBehaviourPunCallbacks, IInteractable
{
    public ChickenBehaviour chickenInSpot;

    public void Interact(CharacterBase character)
    {
        character.currentlyInteractingHidingSpot = this;
        character.photonView.RPC("RPC_HidingInteraction", RpcTarget.AllViaServer);
    }

    public void InteractionFocus(bool focussed)
    {

    }
}
