using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PixelPeeps.HeadlessChickens._Project.Scripts.Character
{
    public class ChickAnimations : MonoBehaviour
    {
        CharacterInput characterInput;
        
        void Start()
        {
            characterInput = GetComponentInParent<CharacterInput>();
        }


        void TakeoffComplete()
        {
            // characterInput.photonView.RPC("AnimAirborneOn", Photon.Pun.RpcTarget.AllBufferedViaServer);
            characterInput.animAirborne = true;
        }
    }

}

