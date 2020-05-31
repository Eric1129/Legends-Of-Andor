using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[System.Serializable]
public class InstantiateMedicinalHerb : Action
{
    string[] players;
    Type type;
    int diceRoll;
   

    public InstantiateMedicinalHerb(string[] players, int roll)
    {
        type = Type.InstantiateMedicinalHerb;

        this.players = players;
        this.diceRoll = roll;
        
    }

    public string[] playersInvolved()
    {
        return this.players;
    }
    public Type getType()
    {
        return this.type;
    }

    public void execute(GameState gs)
    {
        GameController.instance.instantiateMedicinalHerb(diceRoll);

    }

    public bool isLegal(GameState gs)
    {
        return true;
    }
}
