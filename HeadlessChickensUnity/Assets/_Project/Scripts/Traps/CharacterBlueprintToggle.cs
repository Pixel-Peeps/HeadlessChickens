using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterBlueprintToggle : MonoBehaviour
{
    [SerializeField] public List<GameObject> blueprints;

    public void Update()
    {
       
    }

    public void turnOnBluePrint(int blueprintNo)
    {
        blueprints[blueprintNo].SetActive(true);
    }

    public void turnOffBlueprint(int blueprintNo)
    {
        blueprints[blueprintNo].SetActive(false);
    }
}
