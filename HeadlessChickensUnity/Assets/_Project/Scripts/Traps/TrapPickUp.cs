﻿using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using PixelPeeps.HeadlessChickens._Project.Scripts.Character;
using UnityEngine;


public class TrapPickUp : MonoBehaviour
{
    
    //not using these anymore, but keeping JIC
    enum eTrapType
    {
        FalseLever,
        TubOfGlue,
        RottenEgg,
        BrokenEggshells,
        DecoyChick
    }
    
    [SerializeField] private List <GameObject> chickenTraps;

    [Header("Trap BLUEPRINTS")] 
    public GameObject tubGluePrefab;
    public GameObject rottenEggPrefab;
    public GameObject eggShellPrefab;
    public GameObject decoyChickPrefab;
  
    
    // Start is called before the first frame update
    void Start()
    {
        chickenTraps.Add(rottenEggPrefab);
        chickenTraps.Add(eggShellPrefab);
        chickenTraps.Add(decoyChickPrefab);
    }

    public void OnTriggerEnter(Collider other)
    {
        
        if (!other.gameObject.GetComponent<CharacterBase>().isFox)
        {
            Debug.Log("Trap: it's a chicken");
            int random = Random.Range(0, chickenTraps.Count+1);
            other.gameObject.GetComponent<ChickenBehaviour>().trapSlot = chickenTraps[random];
            other.gameObject.GetComponent<ChickenBehaviour>().hasTrap = true;
            Debug.Log("Assigning chick trap: "+chickenTraps[random].name);
            PhotonNetwork.Destroy(gameObject);
        }
        else
        {
            /*Debug.Log("Assigning FOX trap");
            int random = Random.Range(0, 2);

            if (random == 0)
            {
                Debug.Log("Oh lord he has the lever");
                //fox has lever
                other.gameObject.GetComponent<CharacterBase>().hasLever = true;
                other.gameObject.GetComponent<CharacterBase>().hasTrap = true;
                PhotonNetwork.Destroy(gameObject);
            }
            else
            {
            */
                Debug.Log("Oh lord he has the glue");
                //it has the tub of glue
                other.gameObject.GetComponent<FoxBehaviour>().hasTrap = true;
                other.gameObject.GetComponent<FoxBehaviour>().trapSlot = tubGluePrefab;
                PhotonNetwork.Destroy(gameObject);
            //}
        }
    }
}
