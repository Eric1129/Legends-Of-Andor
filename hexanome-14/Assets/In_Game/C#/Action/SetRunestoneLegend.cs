using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SetRunestoneLegend : Action
{
     string[] players;
    Type type;
    int roll;

    public SetRunestoneLegend(string playerId, int dice)
    {
        type = Type.SetRunestoneLegend;

       players = new string[] { playerId };

       roll = dice;        
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
        
        gs.runestoneLegend = roll;   

    }

    public bool isLegal(GameState gs)
    {
        return true;
    }

   
   
}
