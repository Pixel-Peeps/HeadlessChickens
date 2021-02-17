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
            Debug.Log("Ontrigger cube");
            if (cubeMaterialIndex < cubeMaterials.Count-1)
            {
              //  photonView.RPC("ChangeColour", RpcTarget.All, cubeMaterialIndex+1);
              cubeMaterialIndex++;
              gameObject.GetComponent<MeshRenderer>().material = cubeMaterials[cubeMaterialIndex];
            }
            else
            {
                //photonView.RPC("ChangeColour", RpcTarget.AllBuffered, 0);
                cubeMaterialIndex = 0;
                gameObject.GetComponent<MeshRenderer>().material = cubeMaterials[cubeMaterialIndex];
            }
        }

      public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
        {
            if (stream.IsWriting)
            {
                // We own this player: send the others our data
                Debug.Log("running on local (cube)");
                stream.SendNext(cubeMaterialIndex);
                gameObject.GetComponent<MeshRenderer>().material = cubeMaterials[cubeMaterialIndex];
            }
            else
            {
                // Network player, receive data
                Debug.Log("running on remote (cube)");
                this.cubeMaterialIndex = (int)stream.ReceiveNext();
                gameObject.GetComponent<MeshRenderer>().material = cubeMaterials[cubeMaterialIndex];
            }
        }
        
      
        [PunRPC]
        void ChangeColour (int newIndex)
        {
            Debug.Log("change colour rpc");
            cubeMaterialIndex = newIndex;
            gameObject.GetComponent<MeshRenderer>().material = cubeMaterials[newIndex];
        }
    }
}
