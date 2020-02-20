using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fight : Command
{
    private Hero[] fightingHeros;
    private Monster monster;

    public void execute()
    {
    }

    public bool isLegal()
    {
        // this is what will be here later
        // GameObject masterObj = GameObject.FindWithTag("Master");
        // masterClass master = masterObj.GetComponent<masterClass>();
        // Hero[] heros = master.whoIsFighting();
        return true;
    }
}
