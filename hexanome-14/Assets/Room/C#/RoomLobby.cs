using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class RoomLobby : MonoBehaviour
{


    public void leaveRoomClick()
    {
        PhotonNetwork.LeaveRoom();
    }
    public void startGameClick()
    {

    }


}
