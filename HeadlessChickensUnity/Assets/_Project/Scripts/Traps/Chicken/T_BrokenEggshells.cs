using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using PixelPeeps.HeadlessChickens._Project.Scripts.Character;
using UnityEngine;

public class T_BrokenEggshells : MonoBehaviourPunCallbacks
{
    public float effectDuration = 3f;
    private CharacterInput victim;
    private float origSpeed;

    public void OnTriggerEnter(Collider other)
    {
        photonView.SetControllerInternal(other.gameObject.GetComponent<PhotonView>().Owner.ActorNumber);
        var _character = other.gameObject.GetComponent<CharacterBase>();
        if (!_character.isFox)
        {
            return;
        }

        if (_character.isFox)
        {
            //effect
            victim = other.gameObject.GetComponent<CharacterInput>();
            origSpeed = victim.moveSpeed; 
            Debug.Log("STOP OUCH OH NO");
            gameObject.GetComponent<MeshRenderer>().enabled = false;
            victim.moveSpeed = 0;
            victim._anim.SetTrigger("EggshellTrigger");
            StartCoroutine(EggShellEffectCoolDown());
        }
    }

    public IEnumerator EggShellEffectCoolDown()
    {
        yield return new WaitForSeconds(effectDuration);

        // victim.moveSpeed = origSpeed;
        // Debug.Log("ok, that's enough: "+victim.moveSpeed);
        
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