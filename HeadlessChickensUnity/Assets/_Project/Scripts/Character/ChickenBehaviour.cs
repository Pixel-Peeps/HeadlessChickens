using System.Collections;
using UnityEngine;
using Photon.Pun;

namespace PixelPeeps.HeadlessChickens._Project.Scripts.Character

{
    public class ChickenBehaviour : CharacterBase
    {
        Vector3 positionBeforeHiding;
        
        [SerializeField] GameObject chickenMesh;
        [SerializeField] Material caughtMat;

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
        
        public override void HidingInteraction(bool canAccessHiding)
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
                        photonView.RPC("RPC_LeaveHiding", RpcTarget.AllViaServer, positionBeforeHiding, chickenMesh, transform.parent);
                        break;
                    case false:
                        photonView.RPC("RPC_EnterHiding", RpcTarget.AllViaServer, currentHidingSpot.position, chickenMesh, transform.parent);
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
            chickenMesh.GetComponent<Renderer>().enabled = false;

            // lock physics on entering hiding
            _rigidbody.isKinematic = true;

            transform.SetParent(currentHidingSpot);
            transform.position = currentHidingSpot.position;

            // lock the hiding spot while in use
            isHiding = true;
            SwitchState(EStates.Hiding);
        }

        [PunRPC]
        private void RPC_LeaveHiding(Vector3 leavePos)
        {
            transform.GetComponentInParent<HidingSpot>().photonView.RPC("RPC_ToggleAccess", RpcTarget.AllViaServer);

            transform.SetParent(null);
            transform.position = positionBeforeHiding; 
            
            // unlock physics on leaving
            _rigidbody.isKinematic = false;

            // Re-enable Mesh
            chickenMesh.GetComponent<Renderer>().enabled = true;

            isHiding = false;
            
            
            SwitchState(EStates.Moving);
        }
    }
}
