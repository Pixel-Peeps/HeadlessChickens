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

            if (photonView.IsMine)
            {
                switch (canAccessHiding)
                {
                    case true:

                        Debug.Log("Nothing here");
                        break;

                    case false:

                        foreach (Transform t in hideSpot)
                        {
                            if(t.gameObject.TryGetComponent(out ChickenBehaviour chickenComponent) == true)
                            {
                                ChickenBehaviour chicken = chickenComponent;

                                chicken.photonView.RPC("RPC_LeaveHiding", RpcTarget.AllBufferedViaServer, chicken.positionBeforeHiding);

                                chicken.photonView.RPC("ChickenCaptured", RpcTarget.AllBufferedViaServer);
                            }
                            else
                            {
                                Debug.Log("404 Chicken not found");
                                continue;
                            }
                        }
                    
                        break;
                }
            }
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
