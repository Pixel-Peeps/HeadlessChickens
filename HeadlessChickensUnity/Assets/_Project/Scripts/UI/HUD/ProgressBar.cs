using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
// ReSharper disable MemberCanBeMadeStatic.Global

namespace PixelPeeps.HeadlessChickens.UI
{
    public class ProgressBar : MonoBehaviour
    {        
        private const int MAXIMUM = 100;
        
        [Range(0, 100)]
        public int progress;
        
        [SerializeField]
        private Image mask;

        [SerializeField]
        public List<UITweener> imageTweens;

        private delegate void FadeInDelegate();
        private static FadeInDelegate fadeInDelegate;
        
        private delegate void FadeOutDelegate();
        private static FadeOutDelegate fadeOutDelegate;

        public void Start()
        {
            SubscribeFadeEvents();
            BeginFadeOut();
        }
        
        public void OnValidate()
        {
            GetCurrentFill();
        }

        public void GetCurrentFill()
        {
            float fillAmount = (float) progress / (float) MAXIMUM;
            mask.fillAmount = fillAmount;
        }

        private void SubscribeFadeEvents()
        {
            foreach ( UITweener tweener in imageTweens )
            {
                fadeOutDelegate += tweener.FadeOutImage;
                fadeInDelegate += tweener.FadeInImage;
            }
        }

        public void BeginFadeIn()
        {
            fadeInDelegate();
        }

        public void BeginFadeOut()
        {
            fadeOutDelegate();
        }
    }
}