using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using PixelPeeps.HeadlessChickens._Project.Scripts.Character;

/// <summary>
/// Gathers all IInteractable interfaces and relays notifications
/// </summary>
public class Interactable : MonoBehaviourPunCallbacks
{
    private List<IInteractable> interfaces = new List<IInteractable>();
    public enum eInteractType { Lever, Hide, Shortcut }
    [SerializeField] private eInteractType _interactionType;

    public bool interactAllowed = true;

    private void Awake()
    {
        GetComponents<IInteractable>(interfaces);
    }

    [PunRPC]
    public void RPC_ToggleInteractAllowed()
    {
        interactAllowed = false;
        Debug.Log("Interact allowed: "+interactAllowed);
    }
    
    public int GetInteractionType()
    {
        return (int)_interactionType;
    }

    public void Focus(bool state, CharacterBase character)
    {
        for (int i = interfaces.Count - 1; i >= 0; i--)
        {
            if (interfaces[i] == null)
            {
                interfaces.RemoveAt(i);
            }

            interfaces[i].InteractionFocus(state, character);
        }
    }

    public void Interact(CharacterBase character)
    {
        for (int i = interfaces.Count - 1; i >= 0; i--)
        {
            if (interfaces[i] == null)
            {
                interfaces.RemoveAt(i);
            }

            interfaces[i].Interact(character);
        }
    }
}
