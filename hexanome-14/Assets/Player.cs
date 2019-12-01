using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{

    private int heroNumber;
    private string playerID;
    private bool isMyTurn;


    public void setParameters(int num, string ID, bool true_or_false)
    {
        heroNumber = num;
        playerID = ID;
        isMyTurn = true_or_false;
    }


    void Update()
    {
        // ie left-click
        if (Input.GetMouseButtonDown(0))
        {
            masterClass master = gameObject.GetComponentInParent<masterClass>();
            master.notifyClick();
        }
    }

}
