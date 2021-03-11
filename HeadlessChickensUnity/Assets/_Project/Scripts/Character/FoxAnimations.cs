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

        [SerializeField] ParticleSystem painEffectPrefab;
        public ParticleSystem painEffect;
        [SerializeField] GameObject leftFoot;


        private void Start()
        {
            characterInput = GetComponentInParent<CharacterInput>();
            foxBehaviour = GetComponentInParent<FoxBehaviour>();

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
            yield return new WaitForSeconds(3f);
            PhotonNetwork.Destroy(explosionEffect);
        }


        void ActivateStunnedEffect()
        {
            // stunnedEffect.SetActive(true);
        }

        void DeactivateStunnedEffect()
        {
            // stunnedEffect.SetActive(false);
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

