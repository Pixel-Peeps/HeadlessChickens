﻿using System.Collections.Generic;
using ExitGames.Client.Photon.Encryption;
using Photon.Pun;
using PixelPeeps.HeadlessChickens.Network;
using UnityEngine;
// ReSharper disable ForCanBeConvertedToForeach

namespace PixelPeeps.HeadlessChickens.UI
{
    public class ChickCounter : MonoBehaviourPunCallbacks
    {
        public GameObject chickImagePrefab;
        
        private List<ChickCounterImage> imagesInCounter = new List<ChickCounterImage>();
        
        public void GenerateChickIcons()
        {
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

        public void UpdateCounter()
        {
            UpdateDeadChicks();

            UpdateEscapedChicks();
        }
        
        private void UpdateDeadChicks()
        {
            int deadChickCount = NewGameManager.Instance.chickensCaught;

            for (int i = 0; i < deadChickCount; i++)
            {
                ChickCounterImage t = imagesInCounter[i];
                
                if (t.currentState != ChickState.Escaped)
                {
                    t.ChangeState(ChickState.Dead);
                }
            }
        }

        private void UpdateEscapedChicks()
        {
            int escapedChickCount = NewGameManager.Instance.chickensEscaped;
            int loopIncrement = 0;
            
            // Reverse for loop so that escaped chickens start counting from the opposite end
            for (int i = imagesInCounter.Count - 1; i >= 0; i--)
            {
                loopIncrement++;
                if (loopIncrement > escapedChickCount)
                {
                    break;
                }
                
                if (imagesInCounter[i].currentState != ChickState.Dead)
                {
                    imagesInCounter[i].ChangeState(ChickState.Escaped);
                }
            }
        }
    }
}