using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using PixelPeeps.HeadlessChickens._Project.Scripts.Character;

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

    public int GetInteractionType()
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
