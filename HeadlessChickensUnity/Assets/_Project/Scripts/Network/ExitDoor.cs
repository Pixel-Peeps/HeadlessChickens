﻿using System.Collections;
using UnityEngine;
using PixelPeeps.HeadlessChickens._Project.Scripts.Character;
using Photon.Pun;
using PixelPeeps.HeadlessChickens.GameState;
using PixelPeeps.HeadlessChickens.UI;

namespace PixelPeeps.HeadlessChickens.Network
{
    public class ExitDoor : MonoBehaviour
    {
        public bool exitActive = false;
        [SerializeField] private GameObject[] doorClosedMeshes;

        private AudioSource _audioSource;
        public AudioClip exitOpenSFX;
        [Range(0, 1f)] public float exitOpenVolume;

        // Start is called before the first frame update
        void Start()
        {
            _audioSource = Camera.main.GetComponent<AudioSource>();
        }

        // Update is called once per frame
        void Update()
        {

        }

        [PunRPC] public void RPC_ActivateExit()
        {
            Debug.Log("RPC_ActivateExit called");
            NewGameManager.Instance.ExitDoorOpened();
            exitActive = true;

            _audioSource.PlayOneShot(exitOpenSFX, exitOpenVolume);

            foreach (GameObject door in doorClosedMeshes)
            {
                door.SetActive(false);
            }
            
            Debug.Log("RPC_ActivateExit finished. " + gameObject.name + " has been activated");
        }

        private void OnTriggerEnter(Collider other)
        {
            if (exitActive && other.CompareTag("Player"))
            {
                if (other.gameObject.GetComponent<ChickenBehaviour>() == null) return;

                other.gameObject.GetComponent<ChickenBehaviour>().ChickenEscaped();
                //other.gameObject.GetComponent<ChickenBehaviour>().photonView
                //                .RPC("ChickenEscaped", RpcTarget.AllBufferedViaServer);
            }
        }

        public IEnumerator ActivateExit()
        {
            exitActive = true;
            Debug.Log(gameObject.name + " is active");

            yield return new WaitForSeconds(NewGameManager.Instance.exitTime);

            exitActive = false;
            Debug.Log("The game has finished");
        }
    }

}