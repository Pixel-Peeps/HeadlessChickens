using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeverManager : MonoBehaviour
{
    public int leversPulled = 0;
    public bool allLeversPulled = false;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(leversPulled == 4)
        {
            allLeversPulled = true;
        }
    }

    public void IncrementLeverCount()
    {
        leversPulled++;
    }
}
