using UnityEngine;

namespace PixelPeeps.HeadlessChickens.UI
{
    public class UITweener : MonoBehaviour
    {
        [Header("Scale Tween")] 
        public Vector3 scaleTo = new Vector3(2, 2, 2);
        public float scaleTime = 0.5f;
        
        public void ScaleUpFromZero()
        {
            gameObject.transform.localScale = Vector3.zero;

            LeanTween.scale(gameObject, scaleTo, scaleTime);
        }

        public void ScaleUpFromCurrent()
        {
            LeanTween.scale(gameObject, scaleTo, scaleTime);
        }
        
        public void ScaleDown()
        {
            LeanTween.scale(gameObject, scaleTo, scaleTime).setOnComplete(DisableObject);
        }

        private void DisableObject()
        {
            gameObject.SetActive(false);
        }
    }
}