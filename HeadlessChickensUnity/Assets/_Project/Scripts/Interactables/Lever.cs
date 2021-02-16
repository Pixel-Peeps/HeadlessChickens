using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lever : MonoBehaviour, IInteractable
{
    public LeverManager leverManager;
    Interactable interactable;

    private void Awake()
    {
        interactable = GetComponent<Interactable>();
    }

    public void Interact()
    {
        leverManager.IncrementLeverCount();
        interactable.interactAllowed = false;
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
}
