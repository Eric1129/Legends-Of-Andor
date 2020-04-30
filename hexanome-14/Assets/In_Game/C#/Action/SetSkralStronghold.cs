using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SetSkralStronghold : Action
{
     string[] players;
    Type type;
    int loc;

    public SetSkralStronghold(string playerId, int dice)
    {
        type = Type.SetSkralStronghold;

       players = new string[] { playerId };

       loc = dice;        
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
        
        GameController.instance.loadSkralOnTower(loc);  

    }

    public bool isLegal(GameState gs)
    {
        return true;
    }

   
   
}
