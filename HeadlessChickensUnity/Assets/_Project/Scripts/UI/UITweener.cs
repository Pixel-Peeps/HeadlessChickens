using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace PixelPeeps.HeadlessChickens.UI
{
    public class UITweener : MonoBehaviour
    {
        [HideInInspector] public bool currentlyTweening;

        [Header("Move Tween")] 
        public Transform targetPosition;
        public float moveTime = 0.5f;

        [Header("Fade Image Tween")] 
        public Image imageToFade; 
        
        [Header("Fade Text Tween")] 
        public TextMeshProUGUI textToFade;
        
        public Color32 targetColour;
        private Color initialColour;
        public float fadeTime = 0.5f;
        
        [Header("Scale Tween")] 
        public Vector3 scaleUpTo = new Vector3(2, 2, 2);
        public Vector3 scaleDownTo = new Vector3(1, 1, 1);
        
        [Tooltip("Time it takes to reach max scale")]
        public float scaleTime = 0.5f;
        
        [Tooltip("Time this object pauses for before beginning to scale down again")]
        public float holdTime = 0.2f;
        
        
/*
        private void EnableObject()
        {
            gameObject.SetActive(true);
        }
*/
        
        private void DisableObject()
        {
            gameObject.SetActive(false);
        }

        #region Move Tween

        public void MoveToTarget()
        {
            LeanTween.move(gameObject, targetPosition, moveTime).setEaseOutSine();
        }
        
        #endregion

        #region Fade In Tween
        
        public void FadeInImage()
        {
            
            //LeanTween.alpha(imageToFade.rectTransform, 1, fadeTime);
            //LeanTween.color(imageToFade.gameObject, targetColour, fadeTime);
            
            //imageToFade = this.gameObject.GetComponent<Image>();
            initialColour = imageToFade.color;
            initialColour = new Color(initialColour.r, initialColour.g, initialColour.b, 0);
            
            LeanTween.value(gameObject, initialColour, targetColour, fadeTime).setOnUpdate(SetImageColour);
        }
        
        public void FadeInText()
        {
            //LeanTween.alpha(textToFade.rectTransform, 1, fadeTime);
            
            //textToFade = this.gameObject.GetComponent<TextMeshProUGUI>();

            initialColour = textToFade.color;
            initialColour = new Color(initialColour.r, initialColour.g, initialColour.b, 0);
            
            LeanTween.value(gameObject, 0, 1, fadeTime).setOnUpdate(SetTextColour);
        }

        private void SetTextColour(float v)
        {
            textToFade.color = new Color(initialColour.r, initialColour.g, initialColour.b, v);
        }
        
        private void SetImageColour(float v)
        {
            imageToFade.color = new Color(initialColour.r, initialColour.g, initialColour.b, v);
        }
        
        #endregion
        
        #region Scale Tween
        
        public void ScaleUpFromZero()
        {
            StartCoroutine(CurrentTweenCoroutine());
            
            transform.localScale = Vector3.zero;

            LeanTween.scale(gameObject, scaleUpTo, scaleTime);
        }

        private void ScaleUpFromCurrent()
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
        
        #endregion
    }
}