using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

namespace PixelPeeps.HeadlessChickens.Network
{
    public class PlayerAssignment : MonoBehaviourPunCallbacks
    {
        private static PlayerAssignment _instance;
        public static PlayerAssignment Instance
        {
            get => _instance;
            set => _instance = value;
        }

        public List<Player> chickenPlayers = new List<Player>();
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

        public void AssignPlayerRoles(Player[] playersInRoom)
        {
            int randomIndex = Random.Range(0, playersInRoom.Length);

            for (int i = 0; i < playersInRoom.Length; i++)
            {
                if (i == randomIndex)
                {
                    foxPlayer = playersInRoom[i];
                }
                else
                {
                    chickenPlayers.Add(playersInRoom[i]);
                }
            }
        }
    }
}