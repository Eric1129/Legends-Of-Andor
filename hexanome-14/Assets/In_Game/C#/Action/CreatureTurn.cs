using System;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CreatureTurn : Action
{
    string[] players;
    Type type;
    List<int> creatureDiceRoll;
    

    public CreatureTurn(string[] players, List<int> creatureDiceRoll)
    {
        type = Type.CreatureTurn;
        this.players = players;
        this.creatureDiceRoll = creatureDiceRoll;
        
        
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
       
        GameController.instance.fsc.creatureTurnResponse(creatureDiceRoll);
        

    }

    public bool isLegal(GameState gs)
    {
        return true;
    }

   
}