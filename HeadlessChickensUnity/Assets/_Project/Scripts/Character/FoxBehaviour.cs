using UnityEngine;

namespace PixelPeeps.HeadlessChickens._Project.Scripts.Character
{
    public class FoxBehaviour : CharacterBase
    {

        protected override void Action()
        {
            throw new System.NotImplementedException();
        }

        public override void HidingInteraction(HidingSpot hidingSpot)
        {
            Debug.Log("<color=magenta>I am searching</color>");
        }
    }
}
