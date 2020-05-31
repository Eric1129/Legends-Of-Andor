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

        if(dice == 1)
        {
            roll = 2;
        }
        if(dice == 2)
        {
            roll = 4;
        }
        if(dice == 3)
        {
            roll = 5;
        }
        if(dice == 4 || dice == 5)
        {
            roll = 6;
        }
        if(dice == 6)
        {
            roll = 8;
        }
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
        LegendCard.instance.runestone = roll;
        GameController.instance.setStar(roll);
    }

    public bool isLegal(GameState gs)
    {
        return true;
    }
}
