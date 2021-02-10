using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

namespace _Project.Scripts.Character
{
    public class CharacterController : MonoBehaviour
    {
        private InputControls _controls;

        private Vector3 _newPosition;
        private Vector2 _movDirection;
        private float _rotateDirection;
        [SerializeField] private float moveSpeed;
        [SerializeField] private Quaternion newRotation;
        [SerializeField] private float rotationTime;
        [SerializeField] private float rotationSpeed;
        [SerializeField] private float moveTime;

        private void Awake()
        {
            _controls = new InputControls();
            _controls.Enable();

            _controls.Player.Move.started += MoveStarted;
            _controls.Player.Move.canceled += MoveCanceled;
            _controls.Player.Rotate.started += RotateStarted;
            _controls.Player.Rotate.canceled += RotateCanceled;
        }


        private void Start()
        {
            newRotation = transform.rotation;
            _newPosition = transform.position;
        }

        private void FixedUpdate()
        {
            Move();
            Rotate();
        }

        private void Move()
        { 
            _newPosition += transform.forward * (_movDirection.y * moveSpeed);
            transform.position = Vector3.Lerp(transform.position, _newPosition, Time.deltaTime * moveTime);
        }

        private void Rotate()
        {
            newRotation *= Quaternion.Euler(Vector3.up * _rotateDirection);
            newRotation *= Quaternion.Euler(Vector3.up * (_movDirection.x * rotationSpeed));
            transform.rotation = Quaternion.Lerp(transform.rotation, newRotation, Time.deltaTime * rotationTime);
        }

        private void RotateCanceled(InputAction.CallbackContext obj)
        {
            _rotateDirection = 0;
        }

        private void RotateStarted(InputAction.CallbackContext obj)
        {
            _rotateDirection = obj.ReadValue<float>();
        }

        private void MoveStarted(InputAction.CallbackContext obj)
        {
            _movDirection = obj.ReadValue<Vector2>();

        }

        private void MoveCanceled(InputAction.CallbackContext obj)
        {
            _movDirection = Vector2.zero;
        }

    }
}
