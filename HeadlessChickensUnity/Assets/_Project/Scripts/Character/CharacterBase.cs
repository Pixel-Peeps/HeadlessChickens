using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class CharacterBase : MonoBehaviour
{
    private CharacterController _controller;


    private void Awake()
    {
        _controller = GetComponent<CharacterController>();
    }

    void Start()
    {
        
    }
    
    
    void Update()
    {
        
    }
}
