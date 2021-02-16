using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Gathers all IInteractable interfaces and relays notifications
/// </summary>
public class Interactable : MonoBehaviour
{
    private List<IInteractable> interfaces = new List<IInteractable>();
    public enum eInteractType { Lever, Hide, Shortcut }
    [SerializeField] private eInteractType _interactionType;

    public bool interactAllowed = true;

    private void Awake()
    {
        GetComponents<IInteractable>(interfaces);
    }

    public int InteractionType()
    {
        return (int)_interactionType;
    }

    public void Focus(bool state)
    {
        for (int i = interfaces.Count - 1; i >= 0; i--)
        {
            if (interfaces[i] == null)
            {
                interfaces.RemoveAt(i);
            }

            interfaces[i].InteractionFocus(state);
        }
    }

    public void Interact()
    {
        for (int i = interfaces.Count - 1; i >= 0; i--)
        {
            if (interfaces[i] == null)
            {
                interfaces.RemoveAt(i);
            }

            interfaces[i].Interact();
        }
    }
}
