using System;
using UnityEngine;

[System.Serializable]
public class FightTurn : Action
{
    string[] players;
    Type type;
    int fightIndex;
    

    public FightTurn(string[] players, int fightIndex)
    {
        type = Type.FightTurn;
        this.players = players;
        this.fightIndex = fightIndex;
        
        
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
        Debug.Log("sending fight turn");

        
        if(fightIndex == 0)
        {
            GameController.instance.fsc.creatureTurn();
        }
        else
        {
            GameController.instance.fsc.nextPlayerTurnToRoll();
        }
    }

    public bool isLegal(GameState gs)
    {
        return true;
    }

   
}