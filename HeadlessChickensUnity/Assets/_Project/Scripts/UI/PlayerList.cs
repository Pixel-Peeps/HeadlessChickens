﻿using Photon.Realtime;
using UnityEngine;

namespace PixelPeeps.HeadlessChickens.UI
{
    public class PlayerList : MonoBehaviour
    {
        public GameObject listItemPrefab;
        
        public void GeneratePlayerList(Player newPlayer)
        {
            DestroyCurrentList();
            
            GameObject newPlayerListItem = Instantiate(listItemPrefab, this.transform);
            PlayerListItem itemScript = newPlayerListItem.GetComponent<PlayerListItem>();
            itemScript.SetUp(newPlayer);
        }

        private void DestroyCurrentList()
        {
            foreach (Transform t in transform)
            {
                Destroy(t.gameObject);
            }
        }
    }
}