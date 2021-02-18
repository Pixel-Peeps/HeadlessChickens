using Photon.Pun;
using UnityEngine;
using PixelPeeps.HeadlessChickens._Project.Scripts.Character;

public class HidingSpot : MonoBehaviourPunCallbacks, IInteractable
{
    public ChickenBehaviour chickenInSpot;

    public void Interact(CharacterBase character)
    {
        character.HidingInteraction(this);
    }

    public void InteractionFocus(bool focussed)
    {

    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
       
    }
}
