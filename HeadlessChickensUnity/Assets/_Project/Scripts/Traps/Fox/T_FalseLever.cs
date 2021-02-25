using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class T_FalseLever : Traps
{
    public override void PlaceTrap()
    {
        // TODO
        // Code that puts an outline in the world in front of the character
        // that defines where the trap will be placed
        
        // collider check to ensure area is placeable
        // different colour indicating placeable and obstructed
    }

    public override void InitiateTrap()
    {
        // TODO
        // Code that activates the trap once an area is acceptable to place the trap.
    }
    
    public override void ActivateTrap()
    {
        // TODO
        // Implement the actions this trap does on the corresponding player that it effects
    }
}
