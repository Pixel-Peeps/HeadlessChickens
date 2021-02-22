using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PixelPeeps.HeadlessChickens._Project.Scripts.Character;
using Photon.Pun;

namespace PixelPeeps.HeadlessChickens.Network
{
    public class ExitDoor : MonoBehaviour
    {
        public bool exitActive = false;

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        [PunRPC] public void RPC_ActivateExit()
        {
            Debug.Log("RPC_ActivateExit called");
            exitActive = true;
            Debug.Log("RPC_ActivateExit finished. " + gameObject.name + " has been activated");
        }

        private void OnTriggerEnter(Collider other)
        {
            if (exitActive && other.CompareTag("Player"))
            {
                if (other.gameObject.GetComponent<ChickenBehaviour>() == null) return;

                other.gameObject.GetComponent<ChickenBehaviour>().photonView
                                .RPC("ChickenEscaped", RpcTarget.AllBufferedViaServer);
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