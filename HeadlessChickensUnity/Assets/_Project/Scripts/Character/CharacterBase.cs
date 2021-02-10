using System;
using System.ComponentModel;
using UnityEngine;

namespace PixelPeeps.HeadlessChickens._Project.Scripts.Character
{
    [RequireComponent(typeof(CharacterController))]
    public class CharacterBase : MonoBehaviour
    {
        private CharacterController _controller;

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
            _controller = GetComponent<CharacterController>();
        }

        public void SwitchState(EStates change)
        {
            State = change;
        }

        protected virtual void Interact(){}

        protected virtual void Action(){}
    }
}
