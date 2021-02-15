using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace PixelPeeps.HeadlessChickens._Project.Scripts.Character
{
    public class AlternativeCharacterInput : MonoBehaviour
    {
        // OBJECTS
        private InputControls _controls;
        CharacterController mover;
        Animator animator;
        [SerializeField] Transform mainCamera;

        // CAMERA
        Vector3 camF;
        Vector3 camR;

        // INPUT
        Vector2 input;

        // PHYSICS
        Vector3 intent;
        Vector3 velocityXZ;
        public Vector3 velocity;
        [SerializeField] float walkingSpeed = 16f;
        [SerializeField] float accel = 16f;
        [SerializeField] float turnSpeed = 50f;
        [SerializeField] float turnSpeedLow = 5f;
        [SerializeField] float turnSpeedHigh = 50f;
        [SerializeField] float sprintMultiplier = 2;

        // GRAVITY
        public float raycastDistance = 0.12f;
        public float gravity = 10f;

        public float fallMultiplier = 1.08f;
        public float lowJumpMultiplier = 1.01f;

        public bool grounded = false;
        [SerializeField] float jumpPower = 6f;
        public LayerMask groundLayerMask;

        public bool jumpButtonPressed;

        // ANIMATION
        float speed;
        float lastSpeed;
        public float timeSinceLastMoved;

        // OTHER
        public bool controlLocked = false;
        RaycastHit hit;

        public bool justJumped = false;


        void Awake()
        {
            mover = GetComponent<CharacterController>();
            animator = GetComponent<Animator>();

            #region INPUT KEY ACTIONS

            _controls = new InputControls();
            _controls.Enable();

            _controls.Player.Move.performed += MoveStarted;
            _controls.Player.Move.canceled += MoveCanceled;

            _controls.Player.Jump.performed += _ => DoJump();
            _controls.Player.Jump.started += ctx => jumpButtonPressed = true;
            _controls.Player.Jump.canceled += ctx => jumpButtonPressed = false;

            _controls.Player.Run.performed += RunStarted;
            _controls.Player.Run.canceled += RunCancelled;

            #endregion
        }

        void Update()
        {
            // DoInput();
            CalculateCamera();
            CalculateGround();

            if (!controlLocked)
            {
                DoMove();
            }

            DoGravity();
            // DoJump();

            if (justJumped)
            {
                print(velocity);
                justJumped = false;
            }



            mover.Move(velocity * Time.deltaTime);

            if (velocity.y < 0)
            {
                velocity += Vector3.up * -gravity * (fallMultiplier - 1);
            }
            else if (velocity.y > 0 && !jumpButtonPressed)
            {
                velocity += Vector3.up * -gravity * (lowJumpMultiplier - 1);
            }

            // UpdateAnimator();


        }

        private void MoveStarted(InputAction.CallbackContext obj)
        {
            input = obj.ReadValue<Vector2>();
            input = Vector2.ClampMagnitude(input, 1);
            // _character.SwitchState(CharacterBase.EStates.Moving);
        }

        private void MoveCanceled(InputAction.CallbackContext obj)
        {
            input = Vector2.zero;
        }

        //private void DoInput()
        //{
        //    input = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        //    input = Vector2.ClampMagnitude(input, 1);
        //}

        private void CalculateCamera()
        {
            camF = mainCamera.forward;
            camR = mainCamera.right;

            camF.y = 0;
            camR.y = 0;
            camF = camF.normalized;
            camR = camR.normalized;
        }

        //private void OnTriggerStay(Collider other)
        //{
        //    if (other.gameObject.CompareTag("Ground"))
        //    {
        //        grounded = true;
        //    }
        //    else
        //    {
        //        grounded = false;
        //    }
        //}

        private void CalculateGround()
        {
            RaycastHit hit;
            if (Physics.Raycast(transform.position + Vector3.up * 0.15f, -Vector3.up, out hit, raycastDistance, groundLayerMask, QueryTriggerInteraction.Ignore))
            {
                grounded = true;
            }
            else
            {
                grounded = false;
            }
            Debug.DrawRay(transform.position + Vector3.up * 0.15f, -Vector3.up * raycastDistance, Color.red);
        }

        private void DoMove()
        {
            intent = camF * input.y + camR * input.x;

            float tS = velocity.magnitude / walkingSpeed;
            turnSpeed = Mathf.Lerp(turnSpeedHigh, turnSpeedLow, tS);
            if (input.magnitude > 0)
            {
                Quaternion rot = Quaternion.LookRotation(intent);
                transform.rotation = Quaternion.Lerp(transform.rotation, rot, turnSpeed * Time.deltaTime);
            }

            velocityXZ = velocity;
            velocityXZ.y = 0;

            // Quick-turn to rotate on spot
            if (input.magnitude < 0.15)
            {
                velocity = new Vector3(0, velocity.y, 0);
                turnSpeedHigh = 50f;
            }

            else
            {
                velocityXZ = Vector3.Lerp(velocityXZ, transform.forward * input.magnitude * walkingSpeed, accel * Time.deltaTime);
                velocity = new Vector3(velocityXZ.x, velocity.y, velocityXZ.z);
            }
        }

        private void DoGravity()
        {
            if (grounded & !justJumped)
            {
                velocity.y = -0f;
            }
            else
            {
                velocity.y -= gravity * Time.deltaTime;
            }
            velocity.y = Mathf.Clamp(velocity.y, -jumpPower, jumpPower);
        }

        private void DoJump()
        {
            if (grounded && !controlLocked)
            {
                velocity.y = jumpPower;
                justJumped = true;
            }
        }


        private void RunStarted(InputAction.CallbackContext obj)
        {
            walkingSpeed *= sprintMultiplier;
        }

        private void RunCancelled(InputAction.CallbackContext obj)
        {
            walkingSpeed /= sprintMultiplier;
        }

        public void PlayerHalt()
        {
            velocity = new Vector3(0f, 0f, 0f);
        }




        // Animation Methods

        //private void UpdateAnimator()
        //{
        //    CalculateSpeed();
        //    CalculateLastSpeed();

        //    if (!scriptedWalk)
        //    {
        //        animator.SetFloat("forwardSpeed", speed);

        //        DetectMovement();
        //        HandleStretch();
        //    }
        //}

        //private void CalculateSpeed()
        //{
        //    Vector3 localVelocity = transform.InverseTransformDirection(velocity);
        //    speed = localVelocity.z;
        //}

        //private void CalculateLastSpeed()
        //{
        //    if (speed <= 0)
        //    {
        //        speed = lastSpeed * 0.9f;
        //    }

        //    lastSpeed = speed;

        //    if (lastSpeed < 0.01)
        //    {
        //        lastSpeed = 0f;
        //    }
        //}

        //private void DetectMovement()
        //{
        //    if (speed > 0 || !grounded)
        //    {
        //        animator.SetTrigger("movementDetected");
        //        timeSinceLastMoved = 0f;
        //    }
        //    else
        //    {
        //        animator.ResetTrigger("movementDetected");
        //        timeSinceLastMoved += Time.deltaTime;
        //    }
        //}

        //private void HandleStretch()
        //{
        //    if (timeSinceLastMoved >= 7f)
        //    {
        //        Random.seed = System.DateTime.Now.Millisecond;
        //        float num = Random.Range(0f, 1f);
        //        if (num > 0.997)
        //        {
        //            Stretch();
        //        }
        //    }
        //    if (timeSinceLastMoved >= 25f)
        //    {
        //        Stretch();
        //    }
        //}

        //public void Stretch()
        //{
        //    animator.SetTrigger("stretchTrigger");
        //    timeSinceLastMoved = 0f;
        //}
    }
}
