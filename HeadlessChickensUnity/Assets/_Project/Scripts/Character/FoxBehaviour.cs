using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

namespace PixelPeeps.HeadlessChickens._Project.Scripts.Character
{
    public class FoxBehaviour : CharacterBase
    {

        protected override void Action()
        {
            throw new System.NotImplementedException();
        }

        public override void HidingInteraction(bool canAccessHiding, Transform hideSpot)
        {
            Debug.Log("<color=magenta>I am searching</color>");
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.CompareTag("Player"))
            {
                if (other.GetComponent<ChickenBehaviour>() != null)
                {
                    if (other.GetComponent<ChickenBehaviour>().isHiding == false)
                    {
                        other.GetComponent<ChickenBehaviour>().photonView
                            .RPC("ChickenCaptured", RpcTarget.AllBufferedViaServer);
                    }
                }
            }
        }
    }
}
