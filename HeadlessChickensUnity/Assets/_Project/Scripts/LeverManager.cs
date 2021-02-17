using UnityEngine;

public class LeverManager : MonoBehaviour
{
    public int leversPulled = 0;
    public bool allLeversPulled = false;
    
    
    void Update()
    {
        // if all levels are active open the exit
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
