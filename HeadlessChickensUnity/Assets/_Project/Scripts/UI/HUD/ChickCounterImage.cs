using System;
using UnityEngine;
using UnityEngine.UI;

namespace PixelPeeps.HeadlessChickens.UI
{
    public class ChickCounterImage : MonoBehaviour
    {
        [Header("Image Component")] 
        private Image thisImage;

        [Header("State")] 
        public ChickState currentState;
        
        [Header("Sprites")]
        public Sprite defaultChick;
        public Sprite deadChick;
        public Sprite escapedChick;
        
        public void Setup()
        {
            thisImage = GetComponent<Image>();
            ChangeState(ChickState.Default);
        }
        
        public void ChangeState(ChickState state)
        {
            currentState = state;
            switch (currentState)
            {
                case ChickState.Default:
                    thisImage.sprite = defaultChick;
                    break;
                
                case ChickState.Dead:
                    thisImage.sprite = deadChick;
                    break;
                
                case ChickState.Escaped:
                    thisImage.sprite = escapedChick;
                    break;
                
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }

    public enum ChickState
    {
        Default,
        Dead,
        Escaped
    }
}