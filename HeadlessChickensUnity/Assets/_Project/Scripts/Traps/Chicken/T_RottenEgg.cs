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

    public void Start()
    {
        smokeImage = GameObject.Find("RottenEggEffect");
        if (smokeImage == null)
        {
            Debug.Log("Can't find smoke!");
        }
    }
    public void OnTriggerEnter(Collider other)
    {
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
            Debug.Log("phew smelly smelly");
            
            StartCoroutine(RottenEggEffectCoolDown());
        }
    }

    public IEnumerator RottenEggEffectCoolDown()
    {
        yield return new WaitForSeconds(effectDuration);

        smokeImage.GetComponent<ToggleSmokeTrap>().EnableDisableSmoke(false);
        Debug.Log("ok, that's enough: ");
        
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
