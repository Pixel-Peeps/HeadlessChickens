using System;
using System.Collections;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using PixelPeeps.HeadlessChickens.UI;
using System.Collections;

namespace PixelPeeps.HeadlessChickens._Project.Scripts.Character
{
    public class FoxBehaviour : CharacterBase
    {
        [Header( "Rotten Egg Effect" )]
        public GameObject smokeImage;
        public UITweener smokeTween;

        public GameObject stinkParticles;
        public float effectDuration = 4f;

        Animator anim;
        [SerializeField] GameObject fakeChickPrefab;
        public GameObject fakeChickInstance;

        public void Start()
        {
            anim = GetComponentInChildren<Animator>();
            isFox = true;

            smokeImage = HUDManager.Instance.smokeImage;
            smokeTween = smokeImage.GetComponent<UITweener>();
            Debug.Log( "<color=magenta> Got component: " + smokeTween + " on smokeImage </color>" );
        }

        protected override void Action()
        {
            throw new System.NotImplementedException();
        }

        public override void HidingInteraction( bool canAccessHiding, Transform hideSpot )
        {
            Debug.Log( "<color=magenta>I am searching</color>" );

            if ( !photonView.IsMine ) return;
            if ( !canAccessHiding )
            {
                currentHidingSpot = hideSpot;
                SearchHidingSpot();
            }
            else
                Debug.Log( "Nothing here" );
        }

        private void SearchHidingSpot()
        {
            foreach ( Transform t in currentHidingSpot )
            {
                if ( t.gameObject.TryGetComponent( out ChickenBehaviour chickenComponent ) )
                {
                    ChickenBehaviour chick = chickenComponent;

                    chick.photonView.RPC( "RPC_LeaveHiding", RpcTarget.AllBufferedViaServer,
                        chick.positionBeforeHiding );

                    currentHidingSpot.GetComponent<HidingSpot>().photonView
                        .RPC( "RPC_ToggleAccess", RpcTarget.AllViaServer );

                    if ( chick.hasDecoy )
                    {
                        FoundDecoy( chick );
                    }
                    else
                    {
                        chick.ChickenCaptured();
                    }
                }
                else
                {
                    Debug.Log( "404 Chicken not found" );
                }
            }
        }

        private void OnTriggerEnter( Collider other )
        {
            if ( other.gameObject.CompareTag( "Player" ) )
            {
                var chick = other.GetComponent<ChickenBehaviour>();
                if ( chick != null )
                {
                    if ( !chick.isHiding && !chick.hasDecoy )
                    {
                        //chick.photonView
                        //    .RPC("ChickenCaptured", RpcTarget.AllBufferedViaServer);
                        Debug.Log( "Surprise!" );
                        chick.ChickenCaptured();
                    }

                    if ( !chick.isHiding && chick.hasDecoy )
                    {
                        FoundDecoy( chick );
                    }
                }
            }
        }
        //rotten egg effect

        public void StartRottenEggEffect()
        {
            //effect

            if ( photonView.IsMine )
            {
                smokeImage.GetComponent<UITweener>().FadeInImage();
            }

            photonView.RPC( "RPC_ToggleParticles", RpcTarget.AllViaServer, true );
            Debug.Log( "phew smelly smelly" );
            // gameObject.GetComponent<MeshRenderer>().enabled = false;
            StartCoroutine( RottenEggEffectCoolDown() );
        }

        [PunRPC]
        public void RPC_ToggleParticles( bool shouldPlay )
        {
            if ( shouldPlay )
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
            yield return new WaitForSeconds( effectDuration );

            smokeImage.GetComponent<UITweener>().FadeOutImage();
            photonView.RPC( "RPC_ToggleParticles", RpcTarget.AllViaServer, false );

            Debug.Log( "ok, that's enough: " );

            //photonView.RPC("RPC_DestroySelf", RpcTarget.AllBufferedViaServer);
        }


        private void FoundDecoy( ChickenBehaviour chick )
        {
            Debug.Log( "DECOY DEPLOYED" );
            _controller.moveSpeed = 0;
            // anim.SetTrigger("DecoyTrigger");
            anim.SetBool( "SwipeBool", false );
            anim.SetBool( "DecoyBool", true );
            // anim.Play("DecoyFound");
            // photonView.RPC("SpawnFakeChick", RpcTarget.AllBufferedViaServer, chick.photonView.ViewID);
            SpawnFakeChick( chick.photonView.ViewID );

            StartCoroutine( DecoyToggleDelay( chick ) );
            // StartCoroutine(chick.DecoyCooldown());
            chick.hasTrap = false;
        }

        // Trying to spawn an object over the network, be nice
        // [PunRPC]
        public void SpawnFakeChick( int chickID )
        {
            Debug.Log( "SpawnFakeChick called" );
            if ( photonView.IsMine )
            {
                Transform chick = PhotonView.Find( chickID ).transform;
                fakeChickInstance = PhotonNetwork.Instantiate( fakeChickPrefab.name, chick.position, chick.rotation );
            }
        }

        IEnumerator DecoyToggleDelay( ChickenBehaviour chick )
        {
            yield return new WaitForSeconds( 1f );
            anim.SetBool( "DecoyBool", false );
            chick.photonView.RPC( "RPC_ToggleDecoy", RpcTarget.AllViaServer, false );
        }
    }
}