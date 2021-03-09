using Photon.Pun;
using PixelPeeps.HeadlessChickens._Project.Scripts.Character;
using PixelPeeps.HeadlessChickens.Network;
using PixelPeeps.HeadlessChickens.UI;
using UnityEngine;

public class Shortcut : MonoBehaviourPunCallbacks, IInteractable
{
    [SerializeField] private Shortcut otherEnd;
    public float shortcutCooldown = 2f;
    private CharacterBase characterBase;
    
    public void Interact(CharacterBase _characterBase)
    {
        characterBase = _characterBase;
        
        if (characterBase.cooldownRunning) return;
        
        characterBase.transform.position = otherEnd.transform.position;

        characterBase.StartCoroutine(characterBase.CooldownTimer(shortcutCooldown));

    }

    public void InteractionFocus(bool focussed, CharacterBase character)
    {
        photonView.SetControllerInternal(character.photonView.Owner.ActorNumber);
        if (!photonView.IsMine) return;
        
        if ( character.cooldownRunning )
        {
            HUDManager.Instance.UpdateInteractionText();
            return;
        }
        
        if (focussed)
        {
            
            HUDManager.Instance.UpdateInteractionText("SHORTCUT");
        }

        if (!focussed)
        {
            HUDManager.Instance.UpdateInteractionText();
        }
    }
}
