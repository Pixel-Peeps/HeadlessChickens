using UnityEngine;
using UnityEngine.InputSystem;

namespace PixelPeeps.HeadlessChickens._Project.Scripts.Character
{
    public class CharacterController : MonoBehaviour
    {
        private InputControls _controls;

        private Vector3 _newPosition = Vector3.zero;
        private Vector2 _movDirection = Vector2.zero;
        private float _rotateDirection;
        private bool _strafeActive;
        
        [Header("Movement")]
        [SerializeField] private float moveSpeed;
        [SerializeField] private float moveTime; 
        
        [Header("Rotation")]
        [SerializeField] private float rotationTime;
        [SerializeField] private float rotationSpeed;
        [SerializeField] private Quaternion newRotation;
        
        
        
        private void Awake()
        {
            _controls = new InputControls();
            _controls.Enable();

            _controls.Player.Move.performed += MoveStarted;
            _controls.Player.Move.canceled += MoveCanceled;
            _controls.Player.Rotate.performed += RotateStarted;
            _controls.Player.Rotate.canceled += RotateCanceled;
            _controls.Player.Strafe.performed += _ => _strafeActive = true;
            _controls.Player.Strafe.canceled += _ => _strafeActive = false;
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
            if (_strafeActive)
                _newPosition += new Vector3(_movDirection.x, 0, _movDirection.y) * moveSpeed;
            
            else
                _newPosition += transform.forward * (_movDirection.y * moveSpeed);
            Debug.Log(_movDirection);
            transform.position = Vector3.Lerp(transform.position, _newPosition, Time.deltaTime * moveTime);
        }

        private void Rotate()
        {
            if(_strafeActive) return;
            newRotation *= Quaternion.Euler(Vector3.up * _rotateDirection);
            newRotation *= Quaternion.Euler(Vector3.up * (_movDirection.x * rotationSpeed));
            transform.rotation = Quaternion.Lerp(transform.rotation, newRotation, Time.deltaTime * rotationTime);
        }
        
        /*#################################
         *          INPUT CALLBACKS         *
         ##################################*/
        
        #region INPUT BEHAVIOUR
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

        #endregion

    }
}
