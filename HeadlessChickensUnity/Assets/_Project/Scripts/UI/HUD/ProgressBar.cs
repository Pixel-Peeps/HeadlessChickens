using UnityEngine;
using UnityEngine.UI;

namespace PixelPeeps.HeadlessChickens.UI
{
    [ExecuteAlways]
    public class ProgressBar : MonoBehaviour
    {        
        private const int MAXIMUM = 100;
        
        [Range(0, 100)]
        public int progress;
        
        [SerializeField]
        private Image mask;

        public void OnValidate()
        {
            GetCurrentFill();
        }

        public void GetCurrentFill()
        {
            Debug.Log( "calling GetCurrentFill", this );
            float fillAmount = (float) progress / (float) MAXIMUM;
            mask.fillAmount = fillAmount;
        }
    }
}