using UnityEngine;
using UnityEngine.InputSystem;

namespace PixelPeeps.HeadlessChickens._Project.Scripts.Character
{
    public class CharacterController : MonoBehaviour
    {
        private InputControls _controls;
        private CharacterBase _character;

        private Vector3 _newPosition = Vector3.zero;
        [SerializeField] private Vector2 _movDirection = Vector2.zero;
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
            _character = GetComponent<CharacterBase>();
            
            /*####################################
             *           INPUT KEY ACTIONS       *
             * ##################################*/
            
            #region INPUT KEY ACTIONS
            
            _controls = new InputControls();
            _controls.Enable();

            _controls.Player.Move.performed += MoveStarted;
            _controls.Player.Move.canceled += MoveCanceled;
            _controls.Player.Rotate.performed += RotateStarted;
            _controls.Player.Rotate.canceled += RotateCanceled;
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
            Move();
            Rotate();
        }

        private void Move()
        {
            
            _newPosition += _strafeActive      
                ?  transform.forward * (_movDirection.y * moveSpeed) + transform.right * (_movDirection.x * moveSpeed)      // unlock side movement when strafe is active
                : transform.forward * (_movDirection.y * moveSpeed);                                                        // only uses forward as rotation is handled in place of going sideways.
            transform.position = Vector3.Lerp(transform.position, _newPosition, Time.deltaTime * moveTime);
        }

        private void Rotate()
        {
            //NOTE: MAY NEED TO SWITCH LEFT/RIGHT IN REVERSE
             if(_strafeActive) return;                                                  // lock rotation when in strafe mode
            newRotation *= Quaternion.Euler(Vector3.up * (_rotateDirection * rotationSpeed));
            transform.rotation = Quaternion.Lerp(transform.rotation, newRotation, Time.deltaTime * rotationTime);
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
            _character.SwitchState(CharacterBase.EStates.Idle);
        }

        #endregion

    }
}
