using UnityEngine;
using UnityEngine.InputSystem;
using Cinemachine;
using System;
using Photon.Pun;
using PixelPeeps.HeadlessChickens.Network;

namespace PixelPeeps.HeadlessChickens._Project.Scripts.Character
{
    [RequireComponent(typeof(Rigidbody))]
    public class CharacterInput : MonoBehaviourPunCallbacks
    {
        [Header("Required Components/Objects")]
        private InputControls _controls;
        private CharacterBase _character;
        [Tooltip("Camera attached to the GameObject")]
        [SerializeField] private new GameObject camera;
        private Rigidbody _rigidbody;
        private Transform _camTransform;
        private CinemachineFreeLook virtualCam;
        
        [Header("Movement")]
        [Tooltip("Speed multiplier that effects the Objects movement speed")]
        [SerializeField] public float moveSpeed;
        [Tooltip("Multiplier that affects how much time is taken to move the Object")]
        [SerializeField] private float moveTime;
        [Tooltip("Distance the Object is required to be from its final move position")]
        public float stopDistance = 0.1f;
        private Vector3 _newPosition = Vector3.zero;
        public Vector2 _movDirection = Vector2.zero;
        private bool _strafeActive;
        [Tooltip("Multiplier that increase movement speed when running")]
        public float sprintMultiplier = 1.5f;
        [Tooltip("Current rotation speed")]
        [SerializeField] float turnSpeed = 50f;
        [Tooltip("Minimum rotation speed")]
        [SerializeField] float turnSpeedLow = 5f;
        [Tooltip("Maximum rotation speed")]
        [SerializeField] float turnSpeedHigh = 50f;

        [Header("Jump")]
        public bool isGrounded = true;
        [Tooltip("Velocity multiplier applied in the jump direction")]
        public float jumpForce = 4.5f;
        private Vector3 _characterVelocity;
        [Tooltip("Effects how much gravity is applied on this object")]
        public float fallMultiplier = 2.5f;
        [Tooltip("Multiplier to give a minimum jump force when button is pressed and not held")]
        public float lowJumpMultiplier = 2f;
        public bool jumpButtonPressed;

        public float movingJumpForwardBoost = 1.2f;
        public float movingJumpGeneralBoost = 0.8f;

        [Header("Animation")] 
        private Animator _anim;
        public float animSpeed;
        public float verticalForward;
        [SerializeField] float animAcceleration;
        [SerializeField] float animDeceleration;
        public bool animAirborne = false;

        [Header("Interaction")]
        public bool interactCanceled = false;

        private void Awake()
        {
            _character = GetComponent<CharacterBase>();
            _rigidbody = GetComponent<Rigidbody>();
            virtualCam = camera.GetComponent<CinemachineFreeLook>();
            _anim = GetComponentInChildren<Animator>();
            
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
            _controls.Player.Interact.started += InteractPressed;
            _controls.Player.Interact.canceled += ctx => interactCanceled = true;
            _controls.Player.TrapInteract.performed += TrapInteractPressed;
            _controls.Player.Attack.performed += MouseClicked;


            #endregion
        }



        private void Start()
        {
            _newPosition = transform.position;
            //photonView.Group = 1;
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

            if(isGrounded && animAirborne)
            {
                //_anim.SetBool("Jump", false);
                photonView.RPC("AnimAirborneOff", Photon.Pun.RpcTarget.AllBufferedViaServer);
            }
        }

        private void FixedUpdate()
        {
            _camTransform = camera.transform;
            _camTransform.LookAt(transform.position);

            float temp = 0;

            if (_character.State == CharacterBase.EStates.Moving)
            {
                verticalForward = Mathf.SmoothDamp(verticalForward, 1, ref temp, animAcceleration);
            }
            else
            {
                verticalForward = Mathf.SmoothDamp(verticalForward, 0, ref temp, animDeceleration);
            }

            _anim.SetFloat("Vertical_f", Math.Abs(verticalForward));
            _anim.SetFloat("horizontal_f", verticalForward * _movDirection.x);
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

                // If headless, auto-run
                //if (!_character.isFox && _character.hasBeenCaught)
                //{
                //    _newPosition += tempForward * (moveSpeed) + tempRight * (_movDirection.x * moveSpeed);
                //}
                //else
                //{

                //}

                Vector3 facingDirectrion = tempForward * _movDirection.y + tempRight * _movDirection.x;

                //if (!_character.isFox && _character.hasBeenCaught)
                //{
                //    facingDirectrion = _camTransform.forward;
                //}

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

            //Vector3 velocity = facingDirectrion * (moveSpeed / animSpeed);
            //Debug.Log("<color=cyan>cam forward = " + velocity + "</color>");

            // move to position
            transform.position = Vector3.Lerp(transform.position, _newPosition, Time.deltaTime * moveTime);
            }
        }
        

        public void SwapAnimator()
        {
            _anim = GetComponentInChildren<Animator>();
        }

        [PunRPC]
        public void AnimAirborneOn()
        {
            animAirborne = true;
        }

        [PunRPC]
        public void AnimAirborneOff()
        {
            animAirborne = false;
            _anim.Play("JumpLanding");
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

                //_anim.SetBool("Jump", true);
                _anim.SetTrigger("JumpTrigger");
                // animAirborne = true;

                _rigidbody.velocity = _movDirection != Vector2.zero
                    ? (jumpDirection + (Vector3.up * movingJumpForwardBoost)) * jumpForce * (moveSpeed * movingJumpGeneralBoost)
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
                interactCanceled = false;
                
                // if caught, don't do anything
                if (_character.hasBeenCaught) return;

                // if character is hiding, leave hiding spot
                if (_character.State == CharacterBase.EStates.Hiding)
                {
                    _character.HidingInteraction(true, transform);
                }
                
                int interactTypeNumber = _character.interactor.GetInteractType();

                if ( !_character.isFox && (interactTypeNumber == 1 || interactTypeNumber == 2))
                {
                    Debug.Log("calling not loop");
                    var interacted = _character.interactor.TryInteract(false);
                }
                else
                {
                    if (_character.isFox && _character.hasLever && _character.isBlueprintActive)
                    {
                        return;
                    }
                    // Loop
                    Debug.Log("calling loop");
                    var interacted = _character.interactor.TryInteract(true);
                }
                
                
                
            }
        }
        
        private void TrapInteractPressed(InputAction.CallbackContext obj)
        {
            //spaghetti for dinner boys
            if (photonView.IsMine)
            {
                if (_character.hasTrap && !_character.isBlueprintActive && !_character.isFox)
                {
                    //photonView.RPC("RPC_SpawnBluePrint", RpcTarget.AllBufferedViaServer);
                    _character.ToggleBP(true);
                    _character.isBlueprintActive = true;
                }

                if (_character.hasTrap && !_character.isBlueprintActive && _character.isFox && !_character.hasLever)
                {
                   // photonView.RPC("RPC_SpawnBluePrint", RpcTarget.AllBufferedViaServer);
                   _character.ToggleBP(true);
                   _character.isBlueprintActive = true;
                }

                if (_character.isFox && _character.hasLever && !_character.isBlueprintActive)
                {
                    Debug.Log("Showing false levers????");
                    LeverManager.Instance.IdentifyFakeLeverPositions();
                    _character.isBlueprintActive = true;
                }
                
                if ( _character.isFox && _character.hasLever && _character.isBlueprintActive)
                {
                    Debug.Log("this thing is happening");
                    var interacted = _character.interactor.TryInteract(true);
                    //fox picks a fake lever
                    //_character.isBlueprintActive = false;
                }
            }
        }

        [PunRPC]
        private void RPC_SpawnBluePrint()
        {
            if (photonView.IsMine)
            {
            //     _character.isBlueprintActive = true;
            //     var blueprint = PhotonNetwork.Instantiate(_character.trapSlot.name,
            //         new Vector3(0, 0.1f, 0.4f),
            //         Quaternion.identity, 0);
            //     blueprint.gameObject.transform.SetParent(_character.gameObject.transform, false);
            
            
            
            }
        }
        
        private void MouseClicked(InputAction.CallbackContext obj)
        {
            if (photonView.IsMine)
            {
                if (_character.isFox && !_character.isBlueprintActive)
                {
                    _anim.SetTrigger("SwipeTrigger");
                }
                
                if (_character.isFox && _character.isBlueprintActive)
                {
                    _character.isBlueprintActive = false;
                    _character.ToggleBP(false);
                }

                if (!_character.isFox && _character.isBlueprintActive)
                {
                   // ChickenBehaviour chicken = _character.GetComponent<ChickenBehaviour>();
                   // if (chicken.hasBeenCaught)
                  // {
                        Debug.Log("cancelling blueprint");
                        //photonView.RPC("RPC_DestroyBluePrint", RpcTarget.AllViaServer);
                        _character.isBlueprintActive = false;
                        _character.ToggleBP(false);
                   // }
                }

                if (_character.isFox && _character.hasLever && _character.isBlueprintActive)
                {
                    Debug.Log("unshowing false levers????");
                    LeverManager.Instance.IdentifyFakeLeverPositions();
                    _character.isBlueprintActive = false;
                }

                if (_character.isBlueprintActive && !_character.hasLever)
                {
                    if (_character.gameObject.GetComponentInChildren<TrapBlueprint>().gameObject != null)
                    {
                        Debug.Log("cancelling blueprint");
                        //PhotonNetwork.Destroy(_character.gameObject.GetComponentInChildren<TrapBlueprint>().gameObject);
                        _character.isBlueprintActive = false;
                        _character.ToggleBP(false);
                    }
                }
            }
        }

        // [PunRPC]
        // public void RPC_DestroyBluePrint()
        // {
        //     if (photonView.IsMine)
        //     {
        //         _character.gameObject.GetComponentInChildren<TrapBlueprint>().gameObject.GetPhotonView()
        //             .isRuntimeInstantiated = false;
        //         PhotonNetwork.Destroy(_character.gameObject.GetComponentInChildren<TrapBlueprint>().gameObject);
        //         _character.isBlueprintActive = false;
        //     }
        // }
        #endregion
    }
}
