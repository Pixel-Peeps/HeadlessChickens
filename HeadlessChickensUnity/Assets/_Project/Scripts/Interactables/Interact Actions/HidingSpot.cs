using Photon.Pun;
using Photon.Realtime;
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
<<<<<<< Updated upstream
                photonView.TransferOwnership(character.photonView.Owner);
=======
                photonView.RequestOwnership();
>>>>>>> Stashed changes
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
