using UnityEngine;
using UnityEngine.InputSystem;

namespace PixelPeeps.HeadlessChickens._Project.Scripts.Character
{
    [RequireComponent(typeof(Rigidbody))]
    public class CharacterInput : MonoBehaviour
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

        [Header("Jump")]
        public bool isGrounded = true;
        public float jumpForce = 1f;
        private Vector3 _characterVelocity;



        private void Awake()
        {
            _character = GetComponent<CharacterBase>();
            _rigidbody = GetComponent<Rigidbody>();

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
            
            #endregion
            
        }
        
        private void Start()
        {
            _newPosition = transform.position;
        }

        private void FixedUpdate()
        {
            _camTransform = camera.transform;
        }

        public void Move()
        {
            // Check Camera facing direction
            var forward = _camTransform.forward;
            Vector3 tempForward = new Vector3(forward.x, 0, forward.z);
            var right = _camTransform.right;
            Vector3 tempRight = new Vector3(right.x, 0, right.z);
            
            // Set movement target based on cameras direction
            _newPosition += tempForward * (_movDirection.y * moveSpeed) + tempRight * (_movDirection.x * moveSpeed);
            
            // check distance to target position
            float distanceToDestination = Vector3.Distance(transform.position, _newPosition);
            if ( distanceToDestination < stopDistance) _character.SwitchState(CharacterBase.EStates.Idle);
            
            // lock look at when in strafe mode
            if (!_strafeActive) transform.LookAt(new Vector3(_newPosition.x, transform.position.y, _newPosition.z));
            
            // move to position
            transform.position = Vector3.Lerp(transform.position, _newPosition, Time.deltaTime * moveTime);
        }

        private void Jump()
        {
            // lock jump if not grounded, set jump direction based on forward direction
            // If character is not moving jump up, if is moving jump based on forward facing direction
            if (!isGrounded) return;
            Vector3 jumpDirection = transform.forward;

            // if strafe is active set jump direction to the moving direction
            if (_strafeActive)
            {
                if (_movDirection.x < 0) jumpDirection = -transform.right;
                if (_movDirection.x > 0) jumpDirection = transform.right;
                if (_movDirection.y < 0) jumpDirection = -transform.forward;
            }

            _rigidbody.velocity = _movDirection != Vector2.zero
                ? (jumpDirection + Vector3.up) * jumpForce
                : Vector3.up * jumpForce;
            isGrounded = false;
        }

        private void Sprint()
        {
            
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
            _movDirection = obj.ReadValue<Vector2>();
            _character.SwitchState(CharacterBase.EStates.Moving);
        }

        private void MoveCanceled(InputAction.CallbackContext obj)
        {
            _movDirection = Vector2.zero;
        }

        private void RunStarted(InputAction.CallbackContext obj)
        {
            moveSpeed += 2;
        }

        private void RunCancelled(InputAction.CallbackContext obj)
        {
            moveSpeed -= 2;
        }
        
        #endregion

    }
}
