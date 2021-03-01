using System;
using UnityEngine;
using UnityEngine.UI;

namespace PixelPeeps.HeadlessChickens.UI
{
    public class LeverCounterImage : MonoBehaviour
    {
        [Header("Image Component")] 
        private Image thisImage;
        
        [Header("State")] 
        public LeverState currentState;
        
        [Header("Sprites")]
        public Sprite defaultLever;
        public Sprite activatedLever;
        
        public void Setup()
        {
            thisImage = GetComponent<Image>();
            ChangeState(LeverState.Default);
        }

        public void ChangeState(LeverState state)
        {
            currentState = state;

            switch (currentState)
            {
                case LeverState.Default:
                    thisImage.sprite = defaultLever;
                    break;
                
                case LeverState.Activated:
                    thisImage.sprite = activatedLever;
                    break;
                
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }

    public enum LeverState
    {
        Default,
        Activated
    }
}