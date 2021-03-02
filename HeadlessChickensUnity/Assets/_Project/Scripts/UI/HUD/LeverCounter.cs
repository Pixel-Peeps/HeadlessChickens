using System.Collections.Generic;
using Photon.Pun;
using PixelPeeps.HeadlessChickens.Network;
using UnityEngine;

namespace PixelPeeps.HeadlessChickens.UI
{
    public class LeverCounter : MonoBehaviourPunCallbacks
    {        
        public GameObject leverImagePrefab;
        
        private List<LeverCounterImage> imagesInCounter = new List<LeverCounterImage>();
        
        public void GenerateLeverIcons()
        {
            int leverCount = NewGameManager.Instance.maxNumberOfLevers;

            for (int i = 0; i < leverCount; i++)
            {
                GenerateIcon();
            }
        }

        private void GenerateIcon()
        {
            DestroyExistingIcons();
            
            GameObject newLeverImage = Instantiate(leverImagePrefab, transform);
            
            LeverCounterImage counterImageScript = newLeverImage.GetComponent<LeverCounterImage>();
            imagesInCounter.Add(counterImageScript);
            
            counterImageScript.Setup();
        }
        
        private void DestroyExistingIcons()
        {
            foreach (Transform t in transform)
            {
                Destroy(t.gameObject);
            }
            
            imagesInCounter.Clear();
        }

        public void UpdateCounter(int leverCount)
        {
            for (int i = 0; i < leverCount; i++)
            {
                if (imagesInCounter[i].currentState != LeverState.Activated)
                {
                    UITweener leverTween = imagesInCounter[i].GetComponent<UITweener>();
                    leverTween.ScaleUpAndDown();
                }
                imagesInCounter[i].ChangeState(LeverState.Activated);
            }
        }
    }
}