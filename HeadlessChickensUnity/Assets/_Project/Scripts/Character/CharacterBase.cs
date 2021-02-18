using System;
using System.Collections;
using System.ComponentModel;
using Photon.Pun;
using UnityEngine;

namespace PixelPeeps.HeadlessChickens._Project.Scripts.Character
{
    [RequireComponent(typeof(CharacterInput))]
    [RequireComponent(typeof(Interactor))]
    public class CharacterBase : MonoBehaviourPunCallbacks, IPunObservable
    {
        private CharacterInput _controller;
        public Interactor interactor;
        public PhotonView photonView;
        public Rigidbody _rigidbody;
        public Collider _collider;

        public HidingSpot currentHidingSpot;
        public bool cooldownRunning = false;
        public bool hasBeenCaught = false;

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
        private void Awake()
        {
            _controller = GetComponent<CharacterInput>();
            _rigidbody = GetComponent<Rigidbody>();
            _collider = GetComponent<Collider>();
            photonView = GetComponent<PhotonView>();

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
            if (State == EStates.Moving && _controller.isGrounded == true && photonView.IsMine)
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

        public virtual void HidingInteraction(HidingSpot hidingSpot){}
        public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
        {
           // throw new NotImplementedException();
        }
    }
}
