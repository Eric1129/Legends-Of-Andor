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
        
    }

    // Update is called once per frame
    void Update()
    {
        
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
        if (publicListing)
        {
            if (roomInfo.IsOpen && roomInfo.IsVisible)
            {
                GameObject temp = Instantiate(roomButtonPrefab, roomListingPanel);
                RoomButton roomButton = temp.GetComponent<RoomButton>();

                roomButton.roomNameText.text = roomInfo.Name;
                roomButton.roomCapacityText.text = roomInfo.PlayerCount + "/" + roomInfo.MaxPlayers;
            }
        }
        else
        {
            if (roomInfo.IsOpen && !roomInfo.IsVisible)
            {
                GameObject temp = Instantiate(roomButtonPrefab, roomListingPanel);
                RoomButton roomButton = temp.GetComponent<RoomButton>();

                roomButton.roomNameText.text = roomInfo.Name;
                roomButton.roomCapacityText.text = roomInfo.PlayerCount + "/" + roomInfo.MaxPlayers;
            }
        }
    }

    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        base.OnJoinRoomFailed(returnCode, message);
        Debug.Log("Could not join room!");
    }
}
