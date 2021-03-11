using System;
using System.Collections;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using PixelPeeps.HeadlessChickens.UI;

namespace PixelPeeps.HeadlessChickens._Project.Scripts.Character
{
    public class FoxBehaviour : CharacterBase
    {
        [Header("Rotten Egg Effect")]
        public GameObject smokeImage;
        public GameObject stinkParticles;
        public float effectDuration = 4f;

        public void Start()
        {
            isFox = true;
        }

        protected override void Action()
        {
            throw new System.NotImplementedException();
        }

        public override void HidingInteraction(bool canAccessHiding, Transform hideSpot)
        {
            Debug.Log("<color=magenta>I am searching</color>");

            if (!photonView.IsMine) return;
            if (!canAccessHiding)
            {
                currentHidingSpot = hideSpot;
                SearchHidingSpot();
            }
            else
                Debug.Log("Nothing here");
        }

        private void SearchHidingSpot()
        {
            foreach (Transform t in currentHidingSpot)
            {
                if (t.gameObject.TryGetComponent(out ChickenBehaviour chickenComponent))
                {
                    ChickenBehaviour chicken = chickenComponent;

                    chicken.photonView.RPC("RPC_LeaveHiding", RpcTarget.AllBufferedViaServer,
                        chicken.positionBeforeHiding);

                    currentHidingSpot.GetComponent<HidingSpot>().photonView
                        .RPC("RPC_ToggleAccess", RpcTarget.AllViaServer);

                    Debug.Log("<color=green>Before chicken caught call</color>");
                    chicken.ChickenCaptured();
                    Debug.Log("<color=green>After chicken caught call</color>");
                }
                else
                {
                    Debug.Log("404 Chicken not found");
                }
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.CompareTag("Player"))
            {
                var chick = other.GetComponent<ChickenBehaviour>();
                if (chick != null)
                {
                    if (!chick.isHiding && !chick.hasDecoy)
                    {
                        //chick.photonView
                        //    .RPC("ChickenCaptured", RpcTarget.AllBufferedViaServer);
                        chick.ChickenCaptured();
                    }

                    if (!chick.isHiding && chick.hasDecoy)
                    {
                        Debug.Log("DECOY DEPLOYED");
                        StartCoroutine(chick.DecoyCooldown());
                        chick.hasTrap = false;
                    }
                }
            }

        }
        //rotten egg effect

            public void StartRottenEggEffect()
            {
                smokeImage = GameObject.Find("RottenEggEffect");
                //effect

                if (photonView.IsMine)
                {
                    smokeImage.GetComponent<UITweener>().FadeInImage();
                }

                photonView.RPC("RPC_ToggleParticles", RpcTarget.AllViaServer, true);
                Debug.Log("phew smelly smelly");
                // gameObject.GetComponent<MeshRenderer>().enabled = false;
                StartCoroutine(RottenEggEffectCoolDown());
            }

            [PunRPC]
            public void RPC_ToggleParticles(bool shouldPlay)
            {
                if (shouldPlay)
                {
                    stinkParticles.GetComponent<ParticleSystem>().Play();
                }
                else
                {
                    stinkParticles.GetComponent<ParticleSystem>().Stop();
                }
            }

            public IEnumerator RottenEggEffectCoolDown()
            {
                yield return new WaitForSeconds(effectDuration);

                smokeImage.GetComponent<UITweener>().FadeOutImage();
                photonView.RPC("RPC_ToggleParticles", RpcTarget.AllViaServer, false);

                Debug.Log("ok, that's enough: ");

                //photonView.RPC("RPC_DestroySelf", RpcTarget.AllBufferedViaServer);
            }
 
           
        
    }
}