using UnityEngine;

namespace PixelPeeps.HeadlessChickens.UI
{
    public class UITweener : MonoBehaviour
    {   
        public void ScaleUp()
        {
            gameObject.transform.localScale = Vector3.zero;
            
            Vector3 scaleTo = new Vector3(2, 2, 2);
            float scaleTime = 0.5f;

            LeanTween.scale(gameObject, scaleTo, scaleTime);
        }
        
        public void ScaleDown()
        {
            Vector3 scaleTo = new Vector3(0, 0, 0);
            float scaleTime = 0.5f;

            LeanTween.scale(gameObject, scaleTo, scaleTime).setOnComplete(DisableObject);
        }

        private void DisableObject()
        {
            gameObject.SetActive(false);
        }
    }
}