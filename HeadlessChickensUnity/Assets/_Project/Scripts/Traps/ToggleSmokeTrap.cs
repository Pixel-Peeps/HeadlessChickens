using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
        StartCoroutine(FadeImage(shouldShow));
    }
    
 
    IEnumerator FadeImage(bool fadeAway)
    {
        var img = smokeImage.GetComponentInChildren<Image>();
        // fade from opaque to transparent
        if (fadeAway)
        {
            // loop over 1 second backwards
            for (float i = 1; i >= 0; i -= Time.deltaTime)
            {
                // set color with i as alpha
                img.color = new Color(1, 1, 1, i);
                yield return null;
            }
        }
        // fade from transparent to opaque
        else
        {
            // loop over 1 second
            for (float i = 0; i <= 1; i += Time.deltaTime)
            {
                // set color with i as alpha
                img.color = new Color(1, 1, 1, i);
                yield return null;
            }
        }
        smokeImage.SetActive(fadeAway);
    }
}
