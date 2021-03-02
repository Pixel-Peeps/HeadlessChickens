using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Photon.Pun;
using PixelPeeps.HeadlessChickens.Network;
using PixelPeeps.HeadlessChickens.UI;

namespace PixelPeeps.HeadlessChickens._Project.Scripts.Character
{
    public class ChickenManager : MonoBehaviourPunCallbacks
    {
        public List<ChickenBehaviour> activeChicks;
        public List<ChickenBehaviour> escapedChicks;
        
        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }
        

        [PunRPC]
        public void UpdateActiveList(int chickenID)
        {
            activeChicks.Remove(PhotonView.Find(chickenID).GetComponent<ChickenBehaviour>());
            Debug.Log("<color=green> Chicken removed from active</color>");
        }


        [PunRPC]
        public void UpdateEscapedList(int chickenID)
        {
            escapedChicks.Add(PhotonView.Find(chickenID).GetComponent<ChickenBehaviour>());
            Debug.Log("<color=green> Chicken added to escaped</color>");
        }

        [PunRPC]
        public void UpdateEscapedChickCam(int iD)
        {
            //int lastChickToEscapseID = escapedChicks[escapedChicks.Count - 1].photonView.ViewID;

            Debug.Log("<color=cyan>"+ escapedChicks[escapedChicks.Count - 1].photonView.Owner.NickName + " called UpdateEscapedChickCam</color>");

            if (!PhotonView.Find(iD).GetComponent<ChickenBehaviour>().alreadyEscaped) return;

            foreach (var chicken in escapedChicks.Where(chicken => iD == chicken.chickToFollowID))
            {
                int randomInt = UnityEngine.Random.Range(0, activeChicks.Count);

                var chickToFollowID = activeChicks[randomInt].photonView.ViewID;
                chicken.photonView.RPC("UpdateChickToFollow", RpcTarget.AllViaServer, chickToFollowID);
                chicken.photonView.RPC("RPC_CamSwitch", RpcTarget.AllViaServer, chicken.photonView.ViewID);
                Debug.Log("<color=green>" + chicken.photonView.Owner.NickName + "did the foreach loop</color>");
            }
        }
    }

}
