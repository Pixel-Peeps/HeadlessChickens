using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PixelPeeps.HeadlessChickens._Project.Scripts.Character
{
    public class FoxAnimations : MonoBehaviour
    {
        [SerializeField] private GameObject colliderHolder;
        CharacterInput characterInput;
        FoxBehaviour foxBehaviour;

        public float originalSpeed;
        

        [SerializeField] ParticleSystem[] trails;

        [SerializeField] GameObject explosionEffect;
        [SerializeField] GameObject stunnedEffect;

        private AudioSource _audioSource;



        [SerializeField] ParticleSystem painEffectPrefab;
        public ParticleSystem painEffect;
        [SerializeField] GameObject leftFoot;
        



        private void Start()
        {
            characterInput = GetComponentInParent<CharacterInput>();
            foxBehaviour = GetComponentInParent<FoxBehaviour>();

            _audioSource = Camera.main.GetComponent<AudioSource>();

            originalSpeed = characterInput.moveSpeed;

            // painEffect = Instantiate(painEffectPrefab, leftFoot.transform.position, Quaternion.identity, leftFoot.transform);
            //painEffect.gameObject.transform.position = leftFoot.transform.position;
            //painEffect.gameObject.transform.parent = leftFoot.transform;
        }


        // Swipe
        void TurnColliderOn()
        {
            colliderHolder.SetActive(true);
        }

        void TurnColliderOff()
        {
            colliderHolder.SetActive(false);
        }
        
        
        void TurnTrailsOn()
        {
            foreach (ParticleSystem trail in trails)
            {
                var em = trail.emission;
                em.enabled = true;
            }
        }

        void TurnTrailsOff()
        {
            foreach (ParticleSystem trail in trails)
            {
                var em = trail.emission;
                em.enabled = false;
            }

            // foxBehaviour.photonView.RPC("RPC_PlaySwipeEffect", RpcTarget.AllViaServer, foxBehaviour.photonView.ViewID);
            // foxBehaviour.PlaySwipeEffect();
        }

        void ResetBool()
        {
            if (characterInput._anim.GetBool("SwipeBool") == true)
            {
                characterInput._anim.SetBool("SwipeBool", false);
            }

            if (characterInput._anim.GetBool("DecoyBool") == true)
            {
                characterInput._anim.SetBool("DecoyBool", false);
            }

            if (characterInput._anim.GetBool("EggshellBool") == true)
            {
                characterInput._anim.SetBool("EggshellBool", false);
            }
        }


        // Trap Aftermath
        void DestroyDecoyChick()
        {
            if (foxBehaviour.fakeChickInstance == null) return;

            PhotonNetwork.Destroy(foxBehaviour.fakeChickInstance);
            // Destroy(foxBehaviour.fakeChickInstance);
        }

        void Explosion()
        {
            if (foxBehaviour.fakeChickInstance == null) return;

            StartCoroutine(ExplosionRoutine());
            //GameObject explosionPrefab = Instantiate(explosionEffect, foxBehaviour.fakeChickInstance.transform.position, Quaternion.identity);
            //Destroy(explosionPrefab, 3f);
        }

        IEnumerator ExplosionRoutine()
        {
            GameObject explosionParticle = PhotonNetwork.Instantiate(explosionEffect.name, foxBehaviour.fakeChickInstance.transform.position, Quaternion.identity);

            foxBehaviour.PlayExplosionSoundEffect();
            foxBehaviour.photonView.RPC("RPC_PlayExplosionSoundEffect", RpcTarget.Others);

            yield return new WaitForSeconds(3f);
            PhotonNetwork.Destroy(explosionEffect);
        }


        void ActivateStunnedEffect()
        {
            stunnedEffect.SetActive(true);
        }

        void DeactivateStunnedEffect()
        {
            stunnedEffect.SetActive(false);
        }


        void PainFlash()
        {
            painEffect.Play();
        }

        void RestoreSpeed()
        {
            Debug.Log("RestoreSpeed called");
            characterInput.moveSpeed = originalSpeed;
        }
    }
}

