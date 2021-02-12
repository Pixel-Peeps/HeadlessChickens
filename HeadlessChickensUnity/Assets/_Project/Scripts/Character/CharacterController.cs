using UnityEngine;
using UnityEngine.InputSystem;
using Cinemachine;

namespace PixelPeeps.HeadlessChickens._Project.Scripts.Character
{
    public class CharacterController : MonoBehaviour
    {
        private InputControls _controls;
        private CharacterBase _character;
        [SerializeField] GameObject camera;
        private Transform camTransform;

        private Vector3 _newPosition = Vector3.zero;
        [SerializeField] private Vector2 _movDirection = Vector2.zero;
        private float _rotateDirection;
        private bool _strafeActive;
        
        [Header("Movement")]
        [SerializeField] private float moveSpeed = 0;
        [SerializeField] private float moveTime = 0; 

        public float stopDistance = 0.1f;
        
        [Header("Rotation")]
        [SerializeField] private float rotationTime = 0;
        [SerializeField] private float rotationSpeed = 0;
        [SerializeField] private Quaternion newRotation;
        
        
        
        private void Awake()
        {
            _character = GetComponent<CharacterBase>();
            

            /*####################################
             *           INPUT KEY ACTIONS       *
             * ##################################*/
            
            #region INPUT KEY ACTIONS
            
            _controls = new InputControls();
            _controls.Enable();

            _controls.Player.Move.performed += MoveStarted;
            _controls.Player.Move.canceled += MoveCanceled;
            //_controls.Player.Rotate.performed += RotateStarted;
            //_controls.Player.Rotate.canceled += RotateCanceled;
            _controls.Player.Strafe.performed += _ => _strafeActive = true;
            _controls.Player.Strafe.canceled += _ => _strafeActive = false;
            
            #endregion
            
        }
        private void Start()
        {
            newRotation = transform.rotation;
            _newPosition = transform.position;
        }

        private void FixedUpdate()
        {
            camTransform = camera.transform;


            
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

            if (!_strafeActive) transform.LookAt(_newPosition);
            transform.position = Vector3.Lerp(transform.position, _newPosition, Time.deltaTime * moveTime);
        }

        //private void Rotate()
        //{
        //    //NOTE: MAY NEED TO SWITCH LEFT/RIGHT IN REVERSE
        //     if(_strafeActive) return;                                                  // lock rotation when in strafe mode
        //    newRotation *= Quaternion.Euler(Vector3.up * (_rotateDirection * rotationSpeed));
        //    transform.rotation = Quaternion.Lerp(transform.rotation, newRotation, Time.deltaTime * rotationTime);
        //}
        
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
