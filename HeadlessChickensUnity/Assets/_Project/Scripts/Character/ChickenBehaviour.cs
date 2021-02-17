using UnityEngine;

namespace PixelPeeps.HeadlessChickens._Project.Scripts.Character

{
    public class ChickenBehaviour : CharacterBase
    {
        Vector3 positionBeforeHiding;

        protected override void Action()
        {
            throw new System.NotImplementedException();
        }

        public override void HidingInteraction(HidingSpot hidingSpot)
        {
            if(hidingSpot.chickenInSpot == null)
            {
                positionBeforeHiding = transform.position;

                _collider.enabled = false;
                _rigidbody.isKinematic = true;

                transform.position = hidingSpot.transform.position;

                hidingSpot.chickenInSpot = this;
                currentHidingSpot = hidingSpot;
                SwitchState(EStates.Hiding);
            }
            else if(hidingSpot.chickenInSpot ==  this)
            {
                transform.position = positionBeforeHiding;

                _collider.enabled = true;
                _rigidbody.isKinematic = false;

                hidingSpot.chickenInSpot = null;
                currentHidingSpot = null;
                SwitchState(EStates.Moving);
            }
            else
            {
                Debug.Log("A chicken is already in there!");
            }
        }
    }
}
