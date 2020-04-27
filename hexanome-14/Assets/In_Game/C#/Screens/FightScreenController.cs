using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FightScreenController : MonoBehaviour
{
    // Start is called before the first frame update
    public Transform fightChoice;

    private List<string> playersInvolved;

    private int round;
    public FightScreenController()
    {
        
    }

    public void typeOfFight()
    {
        fightChoice.gameObject.SetActive(true);
    }

    public void setTypeOfFight(int type)
    {
        //solo is 0
        //collab is 1
    }

    public void invitePlayers()
    {
        //loop through all players on same space
        //check if there is a player on adjacent space with a bow
        //add them to the
    }
}

