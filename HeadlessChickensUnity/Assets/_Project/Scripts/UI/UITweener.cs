using System.Collections;
using UnityEngine;

namespace PixelPeeps.HeadlessChickens.UI
{
    public class UITweener : MonoBehaviour
    {
        [HideInInspector] public bool currentlyTweening = false;
        
        [Header("Scale Tween")] 
        public Vector3 scaleUpTo = new Vector3(2, 2, 2);
        public Vector3 scaleDownTo = new Vector3(1, 1, 1);
        [Tooltip("Time it takes to reach max scale")]
        public float scaleTime = 0.5f;
        [Tooltip("Time this object pauses for before beginning to scale down again")]
        public float holdTime = 0.2f;
        
        public void ScaleUpFromZero()
        {
            StartCoroutine(CurrentTweenCoroutine());
            
            transform.localScale = Vector3.zero;

            LeanTween.scale(gameObject, scaleUpTo, scaleTime);
        }

        public void ScaleUpFromCurrent()
        {
            StartCoroutine(CurrentTweenCoroutine());
            
            LeanTween.scale(gameObject, scaleUpTo, scaleTime);
        }
        
        public void ScaleDown(bool disableAfterScale)
        {
            StartCoroutine(CurrentTweenCoroutine());
            
            if (disableAfterScale)
            {
                LeanTween.scale(gameObject, scaleDownTo, scaleTime).setOnComplete(DisableObject);
            }
            else
            {
                LeanTween.scale(gameObject, scaleDownTo, scaleTime);
            }
            
        }

        public void ScaleUpAndDown()
        {
            StartCoroutine(CurrentTweenCoroutine());
            
            ScaleUpFromCurrent();
            
            StartCoroutine(HoldTimeCoroutine());
        }

        private void EnableObject()
        {
            gameObject.SetActive(true);
        }
        
        private void DisableObject()
        {
            gameObject.SetActive(false);
        }

        private IEnumerator CurrentTweenCoroutine()
        {
            currentlyTweening = true;
            yield return new WaitForSecondsRealtime(scaleTime);
            currentlyTweening = false;
        }
        
        private IEnumerator HoldTimeCoroutine()
        {
            yield return new WaitForSecondsRealtime(holdTime + scaleTime);
            ScaleDown(false);
        }
    }
}