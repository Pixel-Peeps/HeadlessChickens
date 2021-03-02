using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCam : MonoBehaviour
{
    [SerializeField] private LayerMask everything;
    [SerializeField] private LayerMask currentHidingSpotMask;

    Camera mainCam;

    // Start is called before the first frame update
    void Start()
    {
        mainCam = Camera.main;
    }

    public void CullHidingSpots()
    {
        mainCam.cullingMask = currentHidingSpotMask;

    }

    public void SeeEverything()
    {
        mainCam.cullingMask = everything;
    }
}
