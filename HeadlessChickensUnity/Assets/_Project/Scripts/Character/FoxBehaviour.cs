using System;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

namespace PixelPeeps.HeadlessChickens._Project.Scripts.Character
{
    public class FoxBehaviour : CharacterBase
    {
        //public bool hasLever;

        public void Start()
        {
            isFox = true;
        }

        protected override void Action()
        {
            throw new System.NotImplementedException();
        }

        public override void HidingInteraction(bool canAccessHiding, Transform hideSpot)
        {
            Debug.Log("<color=magenta>I am searching</color>");
 
            if (!photonView.IsMine) return;
            if (!canAccessHiding)
            {
                currentHidingSpot = hideSpot;
                SearchHidingSpot();
            }
            else
                Debug.Log("Nothing here");
        }
 
        private void SearchHidingSpot()
        {
            foreach (Transform t in currentHidingSpot)
            {
                if (t.gameObject.TryGetComponent(out ChickenBehaviour chickenComponent))
                {
<<<<<<< HEAD
                    ChickenBehaviour chicken = chickenComponent;
 
                    chicken.photonView.RPC("RPC_LeaveHiding", RpcTarget.AllBufferedViaServer,
                        chicken.positionBeforeHiding);
 
                    currentHidingSpot.GetComponent<HidingSpot>().photonView
                        .RPC("RPC_ToggleAccess", RpcTarget.AllViaServer);
 
                    chicken.ChickenCaptured();
                }
                else
                {
                    Debug.Log("404 Chicken not found");
                    continue;
=======
                    case true:

                        Debug.Log("Nothing here");
                        break;

                    case false:

                        foreach (Transform t in hideSpot)
                        {
                            if(t.gameObject.TryGetComponent(out ChickenBehaviour chickenComponent) == true)
                            {
                                ChickenBehaviour chicken = chickenComponent;

                                chicken.ChickenCaptured();

                                chicken.photonView.RPC("RPC_LeaveHiding", RpcTarget.AllBufferedViaServer, chicken.positionBeforeHiding);

                                hideSpot.GetComponent<HidingSpot>().photonView.RPC("RPC_ToggleAccess", RpcTarget.AllViaServer);

                                
                                // chicken.photonView.RPC("ChickenCaptured", RpcTarget.AllBufferedViaServer);
                            }
                            else
                            {
                                Debug.Log("404 Chicken not found");
                                continue;
                            }
                        }
                    
                        break;
>>>>>>> main
                }
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.CompareTag("Player"))
            {
                var chick = other.GetComponent<ChickenBehaviour>();
                if (chick != null)
                {
                    if (!chick.isHiding && !chick.hasDecoy)
                    {
                        //chick.photonView
                        //    .RPC("ChickenCaptured", RpcTarget.AllBufferedViaServer);
                        chick.ChickenCaptured();
                    }
                    
                    if (!chick.isHiding && chick.hasDecoy)
                    {
                        Debug.Log("DECOY DEPLOYED");
                        StartCoroutine(chick.DecoyCooldown());
                        chick.hasTrap = false;
                    }
                }
            }
        }
    }
}
