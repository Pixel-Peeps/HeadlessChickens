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

        public override void HidingInteraction(HidingSpot hidingSpot)
        {
            if(hidingSpot.chickenInSpot == null)
            {
                // log position before entering hiding as a return position when leaving
                positionBeforeHiding = transform.position;

                // lock physics on entering hiding
                _collider.enabled = false;
                _rigidbody.isKinematic = true;

                transform.position = hidingSpot.transform.position;

                // lock the hiding spot while in use
                hidingSpot.chickenInSpot = this;
                currentHidingSpot = hidingSpot;
                SwitchState(EStates.Hiding);
            }
            
            // if the chicken hiding is the current chicken, leave the hiding spot.
            else if(hidingSpot.chickenInSpot ==  this)
            {
                transform.position = positionBeforeHiding;

                // unlock physics on leaving
                _collider.enabled = true;
                _rigidbody.isKinematic = false;

                // unlock the hiding spot for other chickens
                hidingSpot.chickenInSpot = null;
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
