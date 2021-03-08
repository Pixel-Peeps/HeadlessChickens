using System.Collections.Generic;
using Photon.Realtime;
using UnityEngine;

namespace PixelPeeps.HeadlessChickens.UI
{
    public class RoomList : MonoBehaviour
    {
        public GameObject listItemPrefab;
        private List<RoomInfo> serverRoomList = new List<RoomInfo>();
        private readonly List<RoomListItem> currentItems = new List<RoomListItem>();

        public void UpdateRoomList(List<RoomInfo> roomList)
        {
            serverRoomList = roomList;

            foreach (RoomInfo info in serverRoomList)
            {
                if (info.RemovedFromList)
                {
                    CheckToRemoveItem(info);
                }
                
                else
                {
                    CheckToRemoveItem(info);
                    
                    CheckToAddItem(info);
                }
            }
        }

        private void CheckToAddItem(RoomInfo info)
        {
            GameObject newRoomItem = Instantiate(listItemPrefab, transform);
            newRoomItem.name = info.Name;
            RoomListItem roomScript = newRoomItem.GetComponent<RoomListItem>();
            roomScript.SetUp(info);
            
            currentItems.Add(roomScript);
        }

        private void CheckToRemoveItem(RoomInfo roomToRemove)
        {
            int index = currentItems.FindIndex(x => x.roomInfo.Name == roomToRemove.Name);
            
            if (index != -1)
            {
                Destroy(currentItems[index].gameObject);
                currentItems.RemoveAt(index);
            }
        }
    }
}