using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class ToggleSmokeTrap : MonoBehaviour
{
    private GameObject smokeImage;
    
    // Start is called before the first frame update
    void Start()
    {
        smokeImage = gameObject.transform.GetChild(0).gameObject;
        smokeImage.SetActive(false);
    }

    public void EnableDisableSmoke(bool shouldShow)
    {
        smokeImage.SetActive(shouldShow);
    }
}
