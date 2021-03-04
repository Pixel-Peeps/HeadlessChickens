using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeverAnimations : MonoBehaviour
{
    [SerializeField] private GameObject redLight;
    [SerializeField] private GameObject greenLight;

    [SerializeField] private GameObject lightObject;
    [SerializeField] private Material greenLightMat;

    private void Start()
    {
        redLight.SetActive(true);
    }

    void LeverActivated()
    {
        redLight.SetActive(false);
        greenLight.SetActive(true);

        SwapLightMaterial();
    }

    private void SwapLightMaterial()
    {
        lightObject.GetComponent<MeshRenderer>().materials[1].SetColor("_Color", Color.green);
        lightObject.GetComponent<MeshRenderer>().materials[1].SetColor("_EmissionColor", Color.green);

    }
}
