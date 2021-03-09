using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using PixelPeeps.HeadlessChickens._Project.Scripts.Character;
using TMPro;
using UnityEngine;

public class T_GlueTub : MonoBehaviourPunCallbacks

{
    public int speedDivideBy = 2;
    public float effectDuration = 4f;
    public float origChickenSpeed;
    private CharacterInput victim;
    private CharacterBase _character;
 
    public void OnTriggerEnter(Collider other)
    {
        photonView.SetControllerInternal(other.gameObject.GetComponent<PhotonView>().Owner.ActorNumber);
        _character = other.gameObject.GetComponent<CharacterBase>();
        if (_character.isFox)
        {
            return;
        }

        if (!_character.isFox && !_character.movementAffected)
        {
            //effect
            victim = other.gameObject.GetComponent<CharacterInput>();
            _character.movementAffected = true;
            Debug.Log("Let's slow this baby down: " + victim.moveSpeed);
            origChickenSpeed = victim.moveSpeed;
            victim.moveSpeed /= 2;
            StartCoroutine(GlueEffectCoolDown());
        }
    }

    public IEnumerator GlueEffectCoolDown()
    {
        yield return new WaitForSeconds(effectDuration);

        victim.moveSpeed = origChickenSpeed;
        _character.movementAffected = false;
        
        Debug.Log("ok, that's enough: "+victim.moveSpeed);
        
        photonView.RPC("RPC_DestroySelf", RpcTarget.AllBufferedViaServer);
    }

    [PunRPC]
    public void RPC_DestroySelf()
    {
        if (gameObject.activeSelf)
        {
            PhotonNetwork.Destroy(gameObject);
        }
    }
}
