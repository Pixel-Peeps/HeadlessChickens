using System;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

namespace PixelPeeps.HeadlessChickens._Project.Scripts.Character
{
    public class FoxBehaviour : CharacterBase
    {
        //public bool hasLever;

        Animator anim;
        [SerializeField] GameObject fakeChickPrefab;
        public GameObject fakeChickInstance;

        public void Start()
        {
            anim = GetComponentInChildren<Animator>();
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
                    ChickenBehaviour chick = chickenComponent;
 
                    chick.photonView.RPC("RPC_LeaveHiding", RpcTarget.AllBufferedViaServer,
                        chick.positionBeforeHiding);
 
                    currentHidingSpot.GetComponent<HidingSpot>().photonView
                        .RPC("RPC_ToggleAccess", RpcTarget.AllViaServer);

                    if (chick.hasDecoy)
                    {
                        FoundDecoy(chick);
                    }
                    else
                    {
                        chick.ChickenCaptured();
                    }
                }
                else
                {
                    Debug.Log("404 Chicken not found");
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
                        FoundDecoy(chick);
                    }
                }
            }
        }

        private void FoundDecoy(ChickenBehaviour chick)
        {
            Debug.Log("DECOY DEPLOYED");
            _controller.moveSpeed = 0;
            anim.SetTrigger("DecoyTrigger");
            // photonView.RPC("SpawnFakeChick", RpcTarget.AllBufferedViaServer, chick.photonView.ViewID);
            SpawnFakeChick(chick.photonView.ViewID);

            chick.photonView.RPC("RPC_ToggleDecoy", RpcTarget.AllViaServer, false);
            // StartCoroutine(chick.DecoyCooldown());
            chick.hasTrap = false;
        }

        // Trying to spawn an object over the network, be nice
        // [PunRPC]
        public void SpawnFakeChick(int chickID)
        {
            if (photonView.IsMine)
            {
                Transform chick = PhotonView.Find(chickID).transform;
                GameObject fakeChickInstance = PhotonNetwork.Instantiate(fakeChickPrefab.name, chick.position, chick.rotation);
            }
        }
    }
}
