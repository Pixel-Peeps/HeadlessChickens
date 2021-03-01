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
        }


        [PunRPC]
        public void UpdateEscapedList(int chickenID)
        {
            escapedChicks.Add(PhotonView.Find(chickenID).GetComponent<ChickenBehaviour>());
        }

        public void UpdateEscapedChickCam()
        {
<<<<<<< Updated upstream
            int lastChickToEscapseID = escapedChicks[escapedChicks.Count - 1].photonView.ViewID;
=======
            int chickID = escapedChicks[escapedChicks.Count - 1].photonView.ViewID;
>>>>>>> Stashed changes

            foreach (var chicken in escapedChicks.Where(chicken => chickID == chicken.chickToFollowID))
            {
<<<<<<< Updated upstream
                if (lastChickToEscapseID == chicken.chickToFollowID)
                {
                    chicken.SwitchToObserverCam();
                }
=======
                chicken.SwitchToObserverCam();
>>>>>>> Stashed changes
            }
        }
    }

}
