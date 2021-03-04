using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using PixelPeeps.HeadlessChickens._Project.Scripts.Character;
using UnityEngine;
using UnityEngine.UI;

public class T_RottenEgg : MonoBehaviourPunCallbacks
{
    public float effectDuration = 5f;
    private CharacterInput victim;
    public GameObject smokeImage;
    public GameObject stinkParticles;

    public void Start()
    {
        smokeImage = GameObject.Find("RottenEggEffect");
        stinkParticles = GameObject.Find("StinkParticles");
        if (smokeImage == null || stinkParticles == null)
        {
            Debug.Log("Can't find smoke or particles!");
        }
    }
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
            smokeImage.GetComponent<ToggleSmokeTrap>().EnableDisableSmoke(true);
            photonView.RPC("RPC_ToggleParticles", RpcTarget.AllViaServer, true);
            Debug.Log("phew smelly smelly");
            gameObject.GetComponent<MeshRenderer>().enabled = false;
            StartCoroutine(RottenEggEffectCoolDown());
        }
    }

    [PunRPC]
    public void RPC_ToggleParticles(bool shouldPlay)
    {
        if (shouldPlay)
        {
            stinkParticles.GetComponent<ParticleSystem>().Play();
        }
        else
        {
            stinkParticles.GetComponent<ParticleSystem>().Stop();
        }
    }

    public IEnumerator RottenEggEffectCoolDown()
    {
        yield return new WaitForSeconds(effectDuration);

        smokeImage.GetComponent<ToggleSmokeTrap>().EnableDisableSmoke(false);
        photonView.RPC("RPC_ToggleParticles", RpcTarget.AllViaServer, false);

        Debug.Log("ok, that's enough: ");
        
        photonView.RPC("RPC_DestroySelf", RpcTarget.AllBufferedViaServer);
    }

    [PunRPC]
    public void RPC_DestroySelf()
    {
        if (gameObject.activeSelf && PhotonNetwork.IsMasterClient)
        {
            PhotonNetwork.Destroy(gameObject);
        }
    }
}
