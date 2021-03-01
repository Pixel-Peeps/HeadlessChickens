using System.Collections;
using System.Collections.Generic;
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
        }


        [PunRPC]
        public void UpdateEscapedList(int chickenID)
        {
            escapedChicks.Add(PhotonView.Find(chickenID).GetComponent<ChickenBehaviour>());
        }

        public void UpdateEscapedChickCam()
        {
            int chickID = escapedChicks[escapedChicks.Count - 1].GetComponent<PhotonView>().ViewID;

            foreach (ChickenBehaviour chicken in escapedChicks)
            {
                if (chickID == chicken.chickToFollowID)
                {
                    chicken.SwitchToObserverCam();
                }
            }
        }
    }

}
