using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

public class CharacterController : MonoBehaviour
{
    private InputControls _controls;

    private Vector3 _newPosition;
    private Vector2 _movDirection;
    [SerializeField] private float moveSpeed;
    [SerializeField] private Quaternion _newRotation;
    [SerializeField] private float rotationTime;
    [SerializeField] private float rotationSpeed;
    [SerializeField] private float moveTime;

    private void Awake()
    {
        _controls = new InputControls();
        _controls.Enable();

        _controls.Player.Move.performed += MoveStarted;
        _controls.Player.Move.canceled += MoveCanceled;
    }

    private void MoveStarted(InputAction.CallbackContext obj)
    {
        _movDirection = obj.ReadValue<Vector2>(); 
        
    }

    private void MoveCanceled(InputAction.CallbackContext obj)
    {
        _movDirection = Vector2.zero;
    }

    private void Start()
    {
        _newRotation = transform.rotation;
        _newPosition = transform.position;
    }

    private void FixedUpdate()
    {
        Move();
    }

    private void Move()
    {
        if (_movDirection.x != 0 && _movDirection.y != 0)
        {
            _newPosition += transform.forward * (_movDirection.y * moveSpeed);
            _newRotation *= Quaternion.Euler(Vector3.up * _movDirection.x);
        }
        if (_movDirection.y != 0)
            _newPosition += transform.forward * (_movDirection.y * moveSpeed);
        if(_movDirection.x != 0)
            _newRotation *= Quaternion.Euler(Vector3.up * (_movDirection.x * rotationSpeed));
        
        transform.position = Vector3.Lerp(transform.position, _newPosition, Time.deltaTime * moveTime);
        transform.rotation = Quaternion.Lerp(transform.rotation, _newRotation, Time.deltaTime * rotationTime);
            
    }
}
