using Photon.Pun;
using System.Collections.Generic;
using PixelPeeps.HeadlessChickens.UI;
using UnityEngine;
using Random = UnityEngine.Random;

namespace PixelPeeps.HeadlessChickens.Network
{
    public class LeverManager : MonoBehaviourPunCallbacks
    {
        
        public static LeverManager Instance { get; private set; }
        
        public int leversPulled = 0;
        public bool allLeversPulled = false;

        public int exitIndex;
        public ExitDoor chosenExit;
        [SerializeField] public List<RoomTile> leverPosList;

        void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
            }
            else
            {
                Instance = this;
            }
        }

        public void Initialise()
        {
            leversPulled = 0;
            allLeversPulled = false;
        }
        
        public void Start()
        {
            exitIndex = Random.Range(0, NewGameManager.Instance.exits.Count);
            chosenExit = NewGameManager.Instance.exits[exitIndex];
        }

        public void SetLeverPosList(List<RoomTile> roomsWLevers)
        {
            leverPosList = roomsWLevers;
            Debug.Log("I'm sending the lever pos list to lever mamanger");
        }
        
        public void IdentifyFakeLeverPositions()
        {
            Debug.Log("Identifying fake lever positions, list count: "+leverPosList.Count);
            for (int i = 0; i < leverPosList.Count; i++)
            {
                Debug.Log("inside loop");
                RoomTile room = leverPosList[i];

                foreach (var leverPos in room.leverPositions)
                {
                    Debug.Log("inside for each");
                    if (leverPos.isChildFake)
                    {
                        Debug.Log("I'm setting up a fake lever right!! now!!!"); 
                        //leverPos.GetComponent<Lever>().gameObject.SetActive(true);
                        //leverPos.GetComponent<Lever>().photonView.RPC("RPC_SetUp", RpcTarget.AllViaServer, true);
                       leverPos.ShowBlueprintOnLever();
                    }
                }
            }
        }

        [PunRPC]
        public void RPC_AllLeversPulled()
        {
            Debug.Log("all levers pulled!");
            allLeversPulled = true;

            if (PhotonNetwork.IsMasterClient)
            {
                chosenExit.gameObject.GetComponent<PhotonView>().RPC("RPC_ActivateExit", RpcTarget.AllBufferedViaServer);
                // chosenExit.StartCoroutine(chosenExit.ActivateExit());
            }
        }

        [PunRPC]
        public void RPC_IncrementLeverCount()
        {
            leversPulled++;

            // if all levels are active open the exit
            if (leversPulled == NewGameManager.Instance.maxNumberOfLevers)
            {
                photonView.RPC("RPC_AllLeversPulled", RpcTarget.AllBufferedViaServer);
            }
            
            HUDManager.Instance.UpdateLeverCounter(leversPulled);
        }
    }
}
