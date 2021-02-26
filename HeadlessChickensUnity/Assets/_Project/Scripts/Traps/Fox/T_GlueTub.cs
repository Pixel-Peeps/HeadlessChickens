using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using PixelPeeps.HeadlessChickens._Project.Scripts.Character;
using UnityEngine;

public class T_GlueTub : Traps
{
    public int speedDivideBy = 2;
    public float effectDuration = 4f;
    public float origChickenSpeed;
    private CharacterInput victim;
    
    public override void ActivateTrap()
    {
        // TODO
        // Implement the actions this trap does on the corresponding player that it effects
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<CharacterBase>().isFox)
        {
            return;
        }
        
        //effect
        victim = other.gameObject.GetComponent<CharacterInput>();
        Debug.Log("Let's slow this baby down: "+victim.moveSpeed);
        origChickenSpeed = victim.moveSpeed;
        victim.moveSpeed /= 2;
        StartCoroutine(GlueEffectCoolDown());

    }

    public IEnumerator GlueEffectCoolDown()
    {
        yield return new WaitForSeconds(effectDuration);

        victim.moveSpeed = origChickenSpeed;
        Debug.Log("ok, that's enough: "+victim.moveSpeed);
        photonView.RPC("RPC_DestroySelf", RpcTarget.AllBufferedViaServer);
    }

    [PunRPC]
    public void RPC_DestroySelf()
    {
        PhotonNetwork.Destroy(gameObject);
    }
}
