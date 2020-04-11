using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.UI;

public class RoomController : MonoBehaviourPunCallbacks
{

    public Transform roomListingPanel;
    public bool publicListing;

    public GameObject roomButtonPrefab;
    // Start is called before the first frame update
    void Start()
    {
        if(NetworkController.roomList != null)
        {
            OnRoomListUpdate(NetworkController.roomList);
        }
    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        base.OnRoomListUpdate(roomList);
        removePrevRooms();


        foreach(RoomInfo ri in roomList)
        {
            Debug.Log(ri.Name + " - Capacitiy: " + ri.PlayerCount + "/" + ri.MaxPlayers);
            listRoom(ri);
        }
    }

    private void removePrevRooms()
    {
        while(roomListingPanel.childCount != 0)
        {
            Destroy(roomListingPanel.GetChild(0).gameObject);
        }
    }
    private void listRoom(RoomInfo roomInfo)
    {
        if(roomInfo.IsOpen && roomInfo.MaxPlayers != 0)
        {
            if (publicListing)
            {
                if (roomInfo.IsVisible)
                {
                    GameObject temp = Instantiate(roomButtonPrefab, roomListingPanel);
                    RoomButton roomButton = temp.GetComponent<RoomButton>();

                    roomButton.roomNameText.text = roomInfo.Name;
                    roomButton.roomCapacityText.text = roomInfo.PlayerCount + "/" + roomInfo.MaxPlayers;
                }
            }
            else
            {
                if (!roomInfo.IsVisible)
                {
                    GameObject temp = Instantiate(roomButtonPrefab, roomListingPanel);
                    RoomButton roomButton = temp.GetComponent<RoomButton>();

                    roomButton.roomNameText.text = roomInfo.Name;
                    roomButton.roomCapacityText.text = roomInfo.PlayerCount + "/" + roomInfo.MaxPlayers;
                }
            }
        }
        
    }

    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        base.OnJoinRoomFailed(returnCode, message);
        Debug.Log("Could not join room!");
    }
}
