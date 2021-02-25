﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Traps : MonoBehaviour
{
    
    // Who is this trap for?
    public enum EOwner { Chick, Fox }
    public EOwner tCharacter;

    public virtual void PlaceTrap()
    {
        // TODO
        // Code that puts an outline in the world in front of the character
        // that defines where the trap will be placed
        
        // collider check to ensure area is placeable
        // different colour indicating placeable and obstructed
    }

    public virtual void InitiateTrap()
    {
        // TODO
        // Code that activates the trap once an area is acceptable to place the trap.
    }

    public virtual void ActivateTrap()
    {
        // TODO
        // Override what each trap does in child classes.
    }

    public void RPC_TrapPlacement()
    {
        // TODO
        // inform the network that a trap has be placed and initiated.
    }
}
