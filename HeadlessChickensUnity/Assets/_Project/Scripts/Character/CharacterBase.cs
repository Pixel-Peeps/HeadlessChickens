using System;
using System.ComponentModel;
using UnityEngine;

namespace PixelPeeps.HeadlessChickens._Project.Scripts.Character
{
    [RequireComponent(typeof(CharacterInput))]
    public class CharacterBase : MonoBehaviour
    {
        private CharacterInput _controller;
        public Interactor interactor;

        public enum EStates
        {
            Idle,
            Moving,
            Hiding
        }

        [SerializeField] private EStates states = EStates.Idle;
        private EStates State
        {
            get => states;
            set
            {
                if (!Enum.IsDefined(typeof(EStates), value))
                    throw new InvalidEnumArgumentException(nameof(value), (int) value, typeof(EStates));
                states = value;
            }
        }
        private void Awake()
        {
            _controller = GetComponent<CharacterInput>();

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
            if(State == EStates.Moving && _controller.isGrounded == true)
            {
                _controller.Move();
            }
        }

        public void SwitchState(EStates change)
        {
            State = change;
        }

        protected virtual void Interact(){}

        protected virtual void Action(){}
    }
}
