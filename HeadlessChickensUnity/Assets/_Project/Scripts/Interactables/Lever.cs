using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lever : MonoBehaviour, IInteractable
{
    
    

    public void Interact()
    {
        Debug.Log("Interact() called");
    }

    public void InteractionFocus(bool focussed)
    {
        if (focussed)
        {
            Debug.Log("InteractionFocus() true");
        }
        else
        {
            Debug.Log("InteractionFocus() false");
        }
        
    }
}
