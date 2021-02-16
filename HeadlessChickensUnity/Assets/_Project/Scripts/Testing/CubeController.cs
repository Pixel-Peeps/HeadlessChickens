using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

namespace com.pixelpeeps.headlesschickens
{
    public class CubeController : MonoBehaviourPunCallbacks, IPunObservable
    {
        [SerializeField] private List<Material> cubeMaterials;

        public int cubeMaterialIndex = 0;
        public Material currentCubeMaterial;
        
        // Start is called before the first frame update
        void Start()
        {
            gameObject.GetComponent<MeshRenderer>().material = cubeMaterials[cubeMaterialIndex];
        }

        private void OnTriggerEnter(Collider other)
        {
            if (cubeMaterialIndex < cubeMaterials.Count-1)
            {
                cubeMaterialIndex++; 
                gameObject.GetComponent<MeshRenderer>().material = cubeMaterials[cubeMaterialIndex];
            }
            else
            {
                cubeMaterialIndex = 0;
                gameObject.GetComponent<MeshRenderer>().material = cubeMaterials[cubeMaterialIndex];
            }
        }

        public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
        {
            if (stream.IsWriting)
            {
                // We own this player: send the others our data
                Debug.Log("running on local (health)");
                stream.SendNext(cubeMaterialIndex);
                gameObject.GetComponent<MeshRenderer>().material = cubeMaterials[cubeMaterialIndex];
            }
            else
            {
                // Network player, receive data
                Debug.Log("running on remote (health)");
                this.cubeMaterialIndex = (int)stream.ReceiveNext();
                gameObject.GetComponent<MeshRenderer>().material = cubeMaterials[cubeMaterialIndex];
            }
        }
    }
}
