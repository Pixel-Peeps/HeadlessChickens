using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoxAnimations : MonoBehaviour
{
    [SerializeField] private GameObject colliderHolder;

    [SerializeField] private ParticleSystem[] trails;

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
}
