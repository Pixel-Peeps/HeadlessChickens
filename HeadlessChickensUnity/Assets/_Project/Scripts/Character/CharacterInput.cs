using UnityEngine;
using UnityEngine.InputSystem;
using Cinemachine;
using System;

namespace PixelPeeps.HeadlessChickens._Project.Scripts.Character
{
    public class CharacterInput : MonoBehaviour
    {
        private InputControls _controls;
        private CharacterBase _character;
        private CharacterController _controller;
        [SerializeField] GameObject camera;
        private Rigidbody rigidbody;
        private Transform camTransform;

        [SerializeField] private Vector3 _newPosition = Vector3.zero;
        [SerializeField] private Vector2 _movDirection = Vector2.zero;
        private float _rotateDirection;
        private bool _strafeActive;

        
        [Header("Movement")]
        [SerializeField] private float moveSpeed = 0;
        [SerializeField] private float moveTime = 0;
        public float stopDistance = 0.1f;


        [Header("Jump")]
        public bool _isGrounded = true;
        public float jumpForce = 1f;
        private float _gravity = -9.8f;
        private Vector3 _characterVelocity;



        private void Awake()
        {
            _character = GetComponent<CharacterBase>();
            _controller = GetComponent<CharacterController>();
            rigidbody = GetComponent<Rigidbody>();

             /*####################################
              *           INPUT KEY ACTIONS       *
              * ##################################*/
            
            #region INPUT KEY ACTIONS
            
            _controls = new InputControls();
            _controls.Enable();

            _controls.Player.Move.performed += MoveStarted;
            _controls.Player.Move.canceled += MoveCanceled;

            _controls.Player.Jump.performed += _ => Jump();

            _controls.Player.Strafe.performed += _ => _strafeActive = true;
            _controls.Player.Strafe.canceled += _ => _strafeActive = false;
            
            #endregion
            
        }

        private void Jump()
        {
            if (!_isGrounded) return;
            //_newPosition.y += jumpForce;
            Vector3 jumpMove = transform.forward + new Vector3(_movDirection.x, 1, _movDirection.y).normalized;
            rigidbody.velocity = (transform.forward + Vector3.up) * jumpForce;
            
            _isGrounded = false;
        }

        private void Start()
        {
            _newPosition = transform.position;
        }

        private void FixedUpdate()
        {
            camTransform = camera.transform;
            //_isGrounded = _controller.isGrounded;



            // Rotate();
        }

        public void Move()
        {
            Vector3 tempForward = new Vector3(camTransform.forward.x, 0, camTransform.forward.z);
            Vector3 tempRight = new Vector3(camTransform.right.x, 0, camTransform.right.z);
            _newPosition += tempForward * (_movDirection.y * moveSpeed) + tempRight * (_movDirection.x * moveSpeed);   // new Vector3(_movDirection.x, 0, _movDirection.y) * moveSpeed;
            float distanceToDestination = Vector3.Distance(transform.position, _newPosition);
            if ( distanceToDestination < stopDistance)
            {
                _character.SwitchState(CharacterBase.EStates.Idle);
            }
            if (!_strafeActive) transform.LookAt(new Vector3(_newPosition.x, transform.position.y, _newPosition.z));
            transform.position = Vector3.Lerp(transform.position, _newPosition, Time.deltaTime * moveTime);
        }

        //private void Rotate()
        //{
        //    //NOTE: MAY NEED TO SWITCH LEFT/RIGHT IN REVERSE
        //     if(_strafeActive) return;                                                  // lock rotation when in strafe mode
        //    newRotation *= Quaternion.Euler(Vector3.up * (_rotateDirection * rotationSpeed));
        //    transform.rotation = Quaternion.Lerp(transform.rotation, newRotation, Time.deltaTime * rotationTime);
        //}

         private void OnTriggerStay(Collider other)
         {
             if (other.gameObject.CompareTag("Ground"))
             {
                 _newPosition = transform.position;
                 _isGrounded = true;
                 // rigidbody.isKinematic = true;
             }
         }

         private void OnTriggerExit(Collider other)
         {
             _isGrounded = false;
         }


         /*##################################
         *          INPUT CALLBACKS        *
         ##################################*/

        #region INPUT CALLBACKS
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
            _character.SwitchState(CharacterBase.EStates.Moving);
        }

        private void MoveCanceled(InputAction.CallbackContext obj)
        {
            _movDirection = Vector2.zero;
        }

        #endregion

    }
}
