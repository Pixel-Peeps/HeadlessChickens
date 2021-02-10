using UnityEngine;

namespace _Project.Scripts.Character
{
    [RequireComponent(typeof(CharacterController))]
    public class CharacterBase : MonoBehaviour
    {
        private CharacterController _controller;


        private void Awake()
        {
            _controller = GetComponent<CharacterController>();
        }

        void Start()
        {
        }


        void Update()
        {

        }

    }
}
