using UnityEngine;

namespace PixelPeeps.HeadlessChickens._Project.Scripts.Character
{
    [RequireComponent(typeof(CharacterController))]
    public abstract class CharacterBase : MonoBehaviour
    {
        private CharacterController _controller;

        public enum EStates { Idle, Moving, Hiding }

        public EStates State { get; protected set; }


        private void Awake()
        {
            _controller = GetComponent<CharacterController>();
        }

        protected abstract void Interact();

        protected abstract void Action();
    }
}
