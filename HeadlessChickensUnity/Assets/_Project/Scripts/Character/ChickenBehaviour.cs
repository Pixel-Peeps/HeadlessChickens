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

        
        [PunRPC]
        public override void RPC_HidingInteraction()
        {
            if(currentlyInteractingHidingSpot.chickenInSpot == null)
            {
                // log position before entering hiding as a return position when leaving
                positionBeforeHiding = transform.position;

                // lock physics on entering hiding
                _collider.enabled = false;
                _rigidbody.isKinematic = true;

                transform.position = currentlyInteractingHidingSpot.transform.position;

                // lock the hiding spot while in use
                currentlyInteractingHidingSpot.chickenInSpot = this;
                currentHidingSpot = currentlyInteractingHidingSpot;
                SwitchState(EStates.Hiding);
            }
            
            // if the chicken hiding is the current chicken, leave the hiding spot.
            else if(currentlyInteractingHidingSpot.chickenInSpot ==  this)
            {
                transform.position = positionBeforeHiding;

                // unlock physics on leaving
                _collider.enabled = true;
                _rigidbody.isKinematic = false;

                // unlock the hiding spot for other chickens
                currentlyInteractingHidingSpot.chickenInSpot = null;
                currentHidingSpot = null;
                SwitchState(EStates.Moving);
            }
            else
            {
                Debug.Log("<color=cyan> A chicken is already in there!</color>");
            }
        }
    }
}
