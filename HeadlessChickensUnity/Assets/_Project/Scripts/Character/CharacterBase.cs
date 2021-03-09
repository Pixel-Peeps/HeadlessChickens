using System;
using System.Collections;
using System.ComponentModel;
using Cinemachine;
using Photon.Pun;
using UnityEngine;

namespace PixelPeeps.HeadlessChickens._Project.Scripts.Character
{
    [RequireComponent(typeof(Interactor))]
    public class CharacterBase : MonoBehaviourPunCallbacks, IPunObservable
    {
        protected CharacterInput _controller;
        public Interactor interactor;
        public PhotonView photonView;
        public Rigidbody _rigidbody;
        public CinemachineFreeLook playerCam;

        public bool cooldownRunning = false;
        public bool hasBeenCaught = false;
        public bool alreadyEscaped = false;
        
        [Header("Blueprints")] public CharacterBlueprintToggle blueprintScript;
        public bool isHiding;
        public bool hasTrap;
        public bool isBlueprintActive;
        public bool isFox;
        public bool hasLever;
        public bool hasDecoy;
        public GameObject trapSlot;
        public int blueprintIndex = 0;
        public bool movementAffected;
        
        [Header("Hiding")]
        public HidingSpot hidedSpot;
        public Transform currentHidingSpot;


        public enum EStates
        {
            Idle,
            Moving,
            Hiding,
        }
        

        [SerializeField] private EStates states = EStates.Idle;
        public EStates State
        {
            get => states;
            private set
            {
                if (!Enum.IsDefined(typeof(EStates), value))
                    throw new InvalidEnumArgumentException(nameof(value), (int) value, typeof(EStates));
                states = value;
            }
        }
        protected void Awake()
        {
            _controller = GetComponent<CharacterInput>();
            _rigidbody = GetComponent<Rigidbody>();
            photonView = GetComponent<PhotonView>();
            playerCam = GetComponentInChildren<CinemachineFreeLook>(true);

            interactor.OnCanInteract += OnCanInteract;
        }

        private void OnCanInteract(Interactable obj)
        {
            //if (obj != null)
            //{

            //}
            //else
            //{

            //}
        }

        private void FixedUpdate()
        {
            if (State == EStates.Moving && _controller.isGrounded == true && !alreadyEscaped) //&& photonView.IsMine)
            {
                _controller.Move();
            }

        }

        public void SwitchState(EStates change)
        {
            State = change;
        }
        
        // cooldown timer used by interactables
        public virtual IEnumerator CooldownTimer(float time)
        {
            cooldownRunning = true;
            yield return new WaitForSeconds(time);
            cooldownRunning = false;
        }

        protected virtual void Action(){}
        
        public virtual void HidingInteraction(bool canAccessHiding, Transform hideSpot){}
        public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
        {
           // throw new NotImplementedException();
        }
        
        public void ToggleBP(bool turnOn)
        {
            if (turnOn)
            {
                Debug.Log("turning blueprint on");
                blueprintScript.turnOnBluePrint(blueprintIndex);
            }
            else if (!turnOn)
            {
                Debug.Log("turning blueprint OFF");
                blueprintScript.turnOffBlueprint(blueprintIndex);
            }
        }
    }
    
    
}
