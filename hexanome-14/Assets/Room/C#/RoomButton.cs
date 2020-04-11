using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.UI;

public class RoomButton : MonoBehaviour
{
    public Text roomNameText;
    public Text roomCapacityText;

    public void joinRoomClick()
    {
        PhotonNetwork.JoinRoom(roomNameText.text);
    }
    
}
