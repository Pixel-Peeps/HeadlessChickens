using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

namespace PixelPeeps.HeadlessChickens.Network
{
    public class PlayerAssignmentRPC : MonoBehaviourPunCallbacks
    {
        private static PlayerAssignmentRPC _instance;
        public static PlayerAssignmentRPC Instance
        {
            get => _instance;
            set => _instance = value;
        }
        
        [HideInInspector]
        public int[] chickenPlayersActorNumbers;
        public Player foxPlayer;

        public void Awake()
        {            
            DontDestroyOnLoad(gameObject);
            
            if (_instance != null && _instance != this)    
            {
                Destroy(gameObject);
            }
            else
            {    
                _instance = this;
            }
        }

        // Only master client calls this
        public void AssignPlayerRoles()
        {
            Player[] playersInRoom = PhotonNetwork.PlayerList;
            
            Debug.Log("AssignPlayerRoles");

             //for (int i = 0; i < playersInRoom.Length; i++)
             //{
             //    chickenPlayersActorNumbers[i] = playersInRoom[i].ActorNumber;
             //}
            
            int randomIndex = Random.Range(0, playersInRoom.Length);

            for (int i = 0; i < playersInRoom.Length; i++)
            {
                if (i == randomIndex)
                {
                    foxPlayer = playersInRoom[i];
                }
                else
                {
                    chickenPlayersActorNumbers[i] = playersInRoom[i].ActorNumber;
                }    
            }

            // Send roles to every player in the room
            photonView.RPC("SendRolesToRoom", RpcTarget.AllBufferedViaServer, chickenPlayersActorNumbers, foxPlayer);
        }
        
        [PunRPC]
        public void SendRolesToRoom(int[] assignedChickens, Player assignedFox)
        {
            Debug.Log("SendRolesToRoom");
            chickenPlayersActorNumbers = assignedChickens;
            foxPlayer = assignedFox;
            
            NewGameManager.Instance.DeterminePlayerRole();
            NewGameManager.Instance.Initialise();
        }
    }
}