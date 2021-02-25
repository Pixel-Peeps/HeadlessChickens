using UnityEngine;
using UnityEngine.InputSystem;
using Cinemachine;
using System;
using Photon.Pun;

namespace PixelPeeps.HeadlessChickens._Project.Scripts.Character
{
    [RequireComponent(typeof(Rigidbody))]
    public class CharacterInput : MonoBehaviourPunCallbacks
    {
        [Header("Required Components/Objects")]
        private InputControls _controls;
        private CharacterBase _character;
        [SerializeField] private new GameObject camera;
        private Rigidbody _rigidbody;
        private Transform _camTransform;
        
        [Header("Movement")]
        [SerializeField] private float moveSpeed;
        [SerializeField] private float moveTime;
        public float stopDistance = 0.1f;
        private Vector3 _newPosition = Vector3.zero;
        private Vector2 _movDirection = Vector2.zero;
        private bool _strafeActive;
        public float sprintMultiplier = 1.5f;
        [SerializeField] float turnSpeed = 50f;
        [SerializeField] float turnSpeedLow = 5f;
        [SerializeField] float turnSpeedHigh = 50f;

        [Header("Jump")]
        public bool isGrounded = true;
        public float jumpForce = 1f;
        private Vector3 _characterVelocity;
        public float fallMultiplier = 2.5f;
        public float lowJumpMultiplier = 2f;
        public bool jumpButtonPressed;

        private CinemachineFreeLook virtualCam;

        private void Awake()
        {
            _character = GetComponent<CharacterBase>();
            _rigidbody = GetComponent<Rigidbody>();
            virtualCam = camera.GetComponent<CinemachineFreeLook>();
            
            
             /*####################################
              *           INPUT KEY ACTIONS       *
              * ##################################*/
            
            #region INPUT KEY ACTIONS
            
            _controls = new InputControls();
            _controls.Enable();

            _controls.Player.Move.performed += MoveStarted;
            _controls.Player.Move.canceled += MoveCanceled;
            _controls.Player.Jump.performed += _ => Jump();
            _controls.Player.Run.performed += RunStarted;
            _controls.Player.Run.canceled += RunCancelled;
            _controls.Player.Strafe.performed += _ => _strafeActive = true;
            _controls.Player.Strafe.canceled += _ => _strafeActive = false;
            _controls.Player.Jump.started += ctx => jumpButtonPressed = true;
            _controls.Player.Jump.canceled += ctx => jumpButtonPressed = false;
            _controls.Player.Interact.performed += InteractPressed;
            _controls.Player.TrapInteract.performed += TrapInteractPressed;
            _controls.Player.Attack.performed += MouseClicked;


            #endregion


        }



        private void Start()
        {
            _newPosition = transform.position;
        }

        private void Update()
        {
            // calculate how fast the character will fall && how how the player will jump based on how long button is pressed
            if (_rigidbody.velocity.y < 0)
            {
                _rigidbody.velocity += Vector3.up * (Physics.gravity.y * (fallMultiplier - 1) * Time.deltaTime);
            }
            else if (_rigidbody.velocity.y > 0 && !jumpButtonPressed)
            {
                _rigidbody.velocity += Vector3.up * (Physics.gravity.y * (lowJumpMultiplier - 1) * Time.deltaTime);
            }

            if (_strafeActive)
            {
                // other camera on
            }
            else
            {
                // normal camera on
            }
        }

        private void FixedUpdate()
        {
            _camTransform = camera.transform;
            _camTransform.LookAt(transform.position);
        }

        public void Move()
        {
            if (photonView.IsMine)
            {
                // Check Camera facing direction
                var forward = _camTransform.forward;
                Vector3 tempForward = new Vector3(forward.x, 0, forward.z);
                var right = _camTransform.right;
                Vector3 tempRight = new Vector3(right.x, 0, right.z);

                // Set movement target based on cameras direction
                _newPosition += tempForward * (_movDirection.y * moveSpeed) + tempRight * (_movDirection.x * moveSpeed);
                Vector3 facingDirectrion = tempForward * _movDirection.y + tempRight * _movDirection.x;

                // check distance to target position
                float distanceToDestination = Vector3.Distance(transform.position, _newPosition);
                if (distanceToDestination < stopDistance) _character.SwitchState(CharacterBase.EStates.Idle);

                // lock look at when in strafe mode
                if (!_strafeActive)
                {
                    float tS = _rigidbody.velocity.magnitude / moveSpeed;
                    turnSpeed = Mathf.Lerp(turnSpeedHigh, turnSpeedLow, tS);
                    if (_movDirection.magnitude > 0)
                    {
                        Quaternion rot = Quaternion.LookRotation(facingDirectrion);
                        transform.rotation = Quaternion.Lerp(transform.rotation, rot, turnSpeed * Time.deltaTime);
                    }

                    turnSpeedHigh = 10f;


                    // Quick-turn to rotate on spot
                    if (_movDirection.magnitude < 0.15)
                    {
                        _rigidbody.velocity = new Vector3(0, _rigidbody.velocity.y, 0);
                        turnSpeedHigh = 40f;
                    }
                }

                // move to position
                transform.position = Vector3.Lerp(transform.position, _newPosition, Time.deltaTime * moveTime);
            }
        }

        private void Jump()
        {
            if (photonView.IsMine)
            {
                // lock jump if not grounded, set jump direction based on forward direction
                // If character is not moving jump up, if is moving jump based on forward facing direction
                if (!isGrounded || _character.State == CharacterBase.EStates.Hiding) return;
                Vector3 jumpDirection = transform.forward;

                // if strafe is active set jump direction to the moving direction
                if (_strafeActive)
                {
                    if (_movDirection.x < 0) jumpDirection = -transform.right;
                    if (_movDirection.x > 0) jumpDirection = transform.right;
                    if (_movDirection.y < 0) jumpDirection = -transform.forward;
                }

                _rigidbody.velocity = _movDirection != Vector2.zero
                    ? (jumpDirection + (Vector3.up * 0.62f)) * jumpForce * (moveSpeed * 0.8f)
                    : Vector3.up * jumpForce;
                isGrounded = false;


            }
        }

        /*##################################
         *            COLLIDERS            *
         ##################################*/
        
        #region COLLIDERS
        private void OnTriggerStay(Collider other)
        {
            if (!other.gameObject.CompareTag("Ground")) return;
            _newPosition = transform.position;
            isGrounded = true;
        }

         private void OnTriggerExit(Collider other)
         {
             isGrounded = false;
         }
#endregion

         /*##################################
         *          INPUT CALLBACKS        *
         ##################################*/

        #region INPUT CALLBACKS
        private void MoveStarted(InputAction.CallbackContext obj)
        {
            if (_character.State == CharacterBase.EStates.Hiding) return;

            _movDirection = obj.ReadValue<Vector2>();
            _character.SwitchState(CharacterBase.EStates.Moving);
        }

        private void MoveCanceled(InputAction.CallbackContext obj)
        {
            _movDirection = Vector2.zero;
        }

        private void RunStarted(InputAction.CallbackContext obj)
        {
            moveSpeed *= sprintMultiplier;
        }

        private void RunCancelled(InputAction.CallbackContext obj)
        {
            moveSpeed /= sprintMultiplier;
        }

        private void InteractPressed(InputAction.CallbackContext obj)
        {
            if (photonView.IsMine)
            {
                // if caught, don't do anything
                if (_character.hasBeenCaught) return;

                // if character is hiding, leave hiding spot
                if (_character.State == CharacterBase.EStates.Hiding)
                {

                    _character.HidingInteraction(true, transform);
                }

                var interacted = _character.interactor.TryInteract();
                int interactTypeNumber = _character.interactor.GetInteractType();
            }
        }
        
        private void TrapInteractPressed(InputAction.CallbackContext obj)
        {
            if (photonView.IsMine)
            {
                //if character has trap && blueprint is not active
                //instantiate correct blueprint && set bool to true
               
                
                //if character has a goddamn lever && bp is not active
                //god idk show all the lever blueprints
            }
        }
        
        private void MouseClicked(InputAction.CallbackContext obj)
        {
            if (photonView.IsMine)
            {
                //if blueprint active, cancel blueprint
                //else idk eat a chicken
               
            }
        }

        #endregion

    }
}
