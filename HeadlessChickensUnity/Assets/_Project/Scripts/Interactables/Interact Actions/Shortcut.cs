using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using PixelPeeps.HeadlessChickens._Project.Scripts.Character;
using UnityEngine;

public class Shortcut : MonoBehaviourPunCallbacks, IInteractable
{
    [SerializeField] Shortcut otherEnd;
    public float shortcutCooldown = 2f;

    public void Interact(CharacterBase characterBase)
    {
        if (characterBase.cooldownRunning) return;

        characterBase.transform.position = otherEnd.transform.position;

        characterBase.StartCoroutine(characterBase.CooldownTimer(shortcutCooldown));

    }

    public void InteractionFocus(bool focussed)
    {
        
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        
    }
}
