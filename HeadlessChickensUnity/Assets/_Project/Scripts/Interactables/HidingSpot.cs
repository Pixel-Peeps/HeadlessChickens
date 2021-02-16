using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PixelPeeps.HeadlessChickens._Project.Scripts.Character;

public class HidingSpot : MonoBehaviour, IInteractable
{
    public ChickenBehaviour chickenInSpot;

    public void Interact(CharacterBase character)
    {
        character.HidingInteraction(this);
    }

    public void InteractionFocus(bool focussed)
    {

    }

}
