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

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.CompareTag("Player"))
            {
                if (other.GetComponent<ChickenBehaviour>() != null)
                {
                    other.GetComponent<ChickenBehaviour>().ChickenCaptured();
                }
            }
        }
    }
}
