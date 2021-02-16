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
                transform.position = hidingSpot.transform.position;

                hidingSpot.chickenInSpot = this;
                SwitchState(EStates.Hiding);
            }
            else if(hidingSpot.chickenInSpot ==  this)
            {
                transform.position = positionBeforeHiding;

                hidingSpot.chickenInSpot = null;
                SwitchState(EStates.Moving);
            }
            else
            {
                Debug.Log("A chicken is already in there!");
            }
        }
    }
}
