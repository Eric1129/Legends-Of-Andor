using System;
using UnityEngine;

[System.Serializable]
public class JoinNextRound : Action
{
    string[] players;

    Type type;
    bool accept;
    

    public JoinNextRound(string[] players, bool accept)
    {
        type = Type.JoinNextRound;
        this.players = players;
        this.accept = accept;
        
        
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

        if (accept)
        {
            GameController.instance.fsc.joinFightLobby2(players[0]);
        }
        else
        {
            GameController.instance.fsc.leaveBattleExecute(players[0]);
        }
    }

    public bool isLegal(GameState gs)
    {
        return true;
    }

    
}