using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using PixelPeeps.HeadlessChickens._Project.Scripts.Character;
using UnityEngine;
using UnityEngine.UI;

public class T_RottenEgg : MonoBehaviourPunCallbacks
{
    public void OnTriggerEnter(Collider other)
    {
        photonView.SetControllerInternal(other.gameObject.GetComponent<PhotonView>().Owner.ActorNumber);
        var _character = other.gameObject.GetComponent<CharacterBase>();
        if (!_character.isFox)
        {
            return;
        }

        if (_character.isFox)
        { _character.gameObject.GetComponent<FoxBehaviour>().StartRottenEggEffect();
            photonView.RPC("RPC_DestroySelf", RpcTarget.AllBufferedViaServer);
        }
    }
}
