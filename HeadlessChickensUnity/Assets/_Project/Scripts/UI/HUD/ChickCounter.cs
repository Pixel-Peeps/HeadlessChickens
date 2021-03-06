﻿using System.Collections.Generic;
using Photon.Pun;
using PixelPeeps.HeadlessChickens.Network;
using UnityEngine;
// ReSharper disable ForCanBeConvertedToForeach

namespace PixelPeeps.HeadlessChickens.UI
{
    public class ChickCounter : MonoBehaviourPunCallbacks
    {
        public GameObject chickImagePrefab;
        public ChickenManager chickenManager;
        
        private readonly List<ChickCounterImage> imagesInCounter = new List<ChickCounterImage>();
        
        public void GenerateChickIcons()
        {
            DestroyExistingIcons();

            
            int chickenCount = PhotonNetwork.CurrentRoom.PlayerCount - 1;

            for (int i = 0; i < chickenCount; i++)
            {
                GenerateIcon();
            }
        }

        private void GenerateIcon()
        {                
            GameObject newChickImage = Instantiate(chickImagePrefab, transform);
            
            ChickCounterImage counterImageScript = newChickImage.GetComponent<ChickCounterImage>();
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

        public void UpdateCounter()
        {
            UpdateDeadChicks();

            UpdateEscapedChicks();
        }
        
        private void UpdateDeadChicks()
        {
            int deadChickCount = chickenManager.deadChicks.Count;

            for (int i = 0; i < deadChickCount; i++)
            {
                ChickCounterImage t = imagesInCounter[i];
                UITweener chickTween = t.GetComponent<UITweener>();
                
                if (t.currentState != ChickState.Escaped)
                {
                    if (t.currentState != ChickState.Dead)
                    {
                        chickTween.ScaleUpAndDown();
                    }
                    
                    t.ChangeState(ChickState.Dead);
                }
            }
        }

        private void UpdateEscapedChicks()
        {
            int escapedChickCount = chickenManager.escapedChicks.Count;
            int loopIncrement = 0;

            // Reverse for loop so that escaped chickens start counting from the opposite end
            for (int i = imagesInCounter.Count - 1; i >= 0; i--)
            {
                UITweener chickTween =  imagesInCounter[i].GetComponent<UITweener>();
                
                loopIncrement++;
                if (loopIncrement > escapedChickCount)
                {
                    break;
                }
                
                if (imagesInCounter[i].currentState != ChickState.Dead)
                {
                    // Tweening it if it hadn't already escaped
                    TweenIcon(i, chickTween);
                    
                    imagesInCounter[i].ChangeState(ChickState.Escaped);
                }
            }
        }

        private void TweenIcon(int i, UITweener chickTween)
        {
            if (imagesInCounter[i].currentState != ChickState.Escaped)
            {
                chickTween.ScaleUpAndDown();
            }
        }
    }
}