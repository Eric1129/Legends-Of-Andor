using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System;

namespace Andor{
public class PlayerController : MonoBehaviour
{
    private PhotonView pv;
    protected Player player;
    private Vector3 newPos;
    // Start is called before the first frame update
    void Start()
    {
        player = GetComponent<Player>();
        pv = GetComponent<PhotonView>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!pv.IsMine)
            return;

        checkClick();
    }


    private void checkClick()
    {
        // if (newPos != player.sphere.transform.position)
        // {

            // loop over button tags and then check if player.currSelected == player.selectedBoardPos

            // GameObject gameObj = GameObject.FindWithTag("Master");
            // masterClass master = GetComponent<masterClass>();
            // master.notifyClick();
        // }
    }
}
}
