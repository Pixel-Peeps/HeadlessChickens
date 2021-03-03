using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoxAnimations : MonoBehaviour
{
    [SerializeField] private GameObject colliderHolder;
    [SerializeField] private GameObject[] trails;

    // Start is called before the first frame update
    void Start()
    {
        
    }


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
        foreach(GameObject trail in trails)
        {
            trail.SetActive(true);
        }
    }

    void TurnTrailsOff()
    {
        foreach (GameObject trail in trails)
        {
            trail.SetActive(false);
        }
    }
}
