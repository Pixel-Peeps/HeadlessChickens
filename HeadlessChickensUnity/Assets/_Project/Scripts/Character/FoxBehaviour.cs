﻿using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

namespace PixelPeeps.HeadlessChickens._Project.Scripts.Character
{
    public class FoxBehaviour : CharacterBase
    {

        protected override void Action()
        {
            throw new System.NotImplementedException();
        }

        public override void HidingInteraction(bool canAccessHiding)
        {
            Debug.Log("<color=magenta>I am searching</color>");
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.CompareTag("Player"))
            {
                if (other.GetComponent<ChickenBehaviour>() != null)
                {
                    
                    Debug.Log("calling chicken caught RPC");
                    other.GetComponent<ChickenBehaviour>().photonView.RPC("ChickenCaptured", RpcTarget.AllBufferedViaServer);
                }
            }
        }
    }
}
