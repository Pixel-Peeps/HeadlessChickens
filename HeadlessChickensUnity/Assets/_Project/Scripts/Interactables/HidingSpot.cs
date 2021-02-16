using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PixelPeeps.HeadlessChickens._Project.Scripts.Character;

public class HidingSpot : MonoBehaviour, IInteractable
{
    public bool occupied = false;

    public void Interact(CharacterBase characterBase)
    {
        characterBase.HidingSpot();
    }

    public void InteractionFocus(bool focussed)
    {

    }

}
