using Photon.Pun;
using UnityEngine;
using PixelPeeps.HeadlessChickens._Project.Scripts.Character;

public class HidingSpot : MonoBehaviourPunCallbacks, IInteractable
{
    public ChickenBehaviour chickenInSpot;

    public void Interact(CharacterBase character)
    {
        character.photonView.RPC("RPC_HidingInteraction", RpcTarget.AllViaServer, this);
    }

    public void InteractionFocus(bool focussed)
    {

    }
}
