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

        private float originalSpeed;

        [SerializeField] ParticleSystem[] trails;

        [SerializeField] ParticleSystem explosionEffect;
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
            
            Destroy(foxBehaviour.fakeChickInstance);
        }

        void Explosion()
        {
            if (foxBehaviour.fakeChickInstance == null) return;

            ParticleSystem explosionPrefab = Instantiate(explosionEffect, foxBehaviour.fakeChickInstance.transform.position, Quaternion.identity);
            Destroy(explosionPrefab, 3f);
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
            characterInput.moveSpeed = originalSpeed;
        }
    }
}

