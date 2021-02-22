using System;
using System.Collections;
using System.Runtime.CompilerServices;
using UnityEngine;
using Photon.Pun;

namespace PixelPeeps.HeadlessChickens._Project.Scripts.Character

{
    public class ChickenBehaviour : CharacterBase
    {
        public Vector3 positionBeforeHiding;
        
        [SerializeField] Renderer chickenMesh;
        [SerializeField] Material caughtMat;
        public HidingSpot hidedSpot;
        public Transform currentHidingSpot;

        private void Start()
        {
            chickenMesh = GetComponentInChildren<Renderer>();
            
        }

        [PunRPC] 
        public void ChickenCaptured()
        {
            this.hasBeenCaught = true;
            this.chickenMesh.GetComponent<Renderer>().sharedMaterial = caughtMat;
        }

        protected override void Action()
        {
            throw new System.NotImplementedException();
        }
        
        public override void HidingInteraction(bool canAccessHiding, Transform hideSpot)
        {
            if (photonView.IsMine)
            {
                if (!canAccessHiding && !isHiding)
                {
                    Debug.Log("Someone else is already in there!");
                    return;
                }
                
                switch (isHiding)
                {
                    case true:
                        photonView.RPC("RPC_LeaveHiding", RpcTarget.AllViaServer, positionBeforeHiding);
                        photonView.RPC("RPC_SetParent", RpcTarget.AllViaServer);
                        hidedSpot.photonView.RPC("RPC_ToggleAccess", RpcTarget.AllViaServer);
                        break;
                    case false:
                        currentHidingSpot = hideSpot;
                        photonView.RPC("RPC_EnterHiding", RpcTarget.AllViaServer, currentHidingSpot.position);
                        photonView.RPC("RPC_SetParent", RpcTarget.AllViaServer);
                        
                        break;
                }
            }
        }

        [PunRPC]
        private void RPC_EnterHiding(Vector3 hidingPos)
        {
            // log position before entering hiding as a return position when leaving
            positionBeforeHiding = transform.position;

            // Disable Mesh
            chickenMesh.enabled = false;

            // lock physics on entering hiding
            _rigidbody.isKinematic = true;

            //photonView.transform.SetParent(currentHidingSpot);
            gameObject.transform.position = currentHidingSpot.position;
            Debug.Log("after gameObject.transform.position = currentHidingSpot.position");
            // lock the hiding spot while in use
            isHiding = true;
            Debug.Log("after isHiding = true");
            SwitchState(EStates.Hiding);
            Debug.Log("after switch state to hiding, end of RPC");
        }

        [PunRPC]
        private void RPC_LeaveHiding(Vector3 leavePos)
        {
            

            
            //photonView.transform.SetParent(null);
            photonView.transform.position = positionBeforeHiding; 
            
            // unlock physics on leaving
            _rigidbody.isKinematic = false;

            // Re-enable Mesh
            chickenMesh.enabled = true;

            isHiding = false;
            
            
            SwitchState(EStates.Moving);
        }

        [PunRPC]
        private void RPC_SetParent()
        {
            switch (isHiding)
            {
                case true: 
                    transform.SetParent(currentHidingSpot);
                    hidedSpot = transform.GetComponentInParent<HidingSpot>();
                    break;
                case false:
                    transform.SetParent(null);
                    break;
            }
        }
    }
}
