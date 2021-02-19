using Photon.Pun;
using UnityEngine;
using PixelPeeps.HeadlessChickens._Project.Scripts.Character;

public class HidingSpot : MonoBehaviourPunCallbacks, IInteractable
{
    //public ChickenBehaviour chickenInSpot;
    public bool inUse = false;

    public void Interact(CharacterBase character)
    {
        if (photonView.IsMine)
        {
            if (!inUse)
            {
                Debug.Log("Interact() method called, inUse false");
                character.currentHidingSpot = transform;
                character.HidingInteraction();
                inUse = true;
            }
            else
            {
                // character.currentHidingSpot.position = null;
                character.HidingInteraction();
                inUse = false;
            }
        }
    }

    public void InteractionFocus(bool focussed)
    {

    }
}
