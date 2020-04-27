using System;
using UnityEngine;

[System.Serializable]
public class RespondFight : Action
{
    string[] players;

    Type type;
    bool accept;
    bool hostPlayer;

    public RespondFight(string[] players, bool accept, bool hostPlayer)
    {
        type = Type.RespondFight;
        this.players = players;
        this.accept = accept;
        this.hostPlayer = hostPlayer;
        
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
        Debug.Log("Executing respond fight");
        if (hostPlayer)
        {
            GameController.instance.fsc.addHostPlayer(players[0]);
        }
        else
        {
            if (accept)
            {
                GameController.instance.fsc.joinFightLobby(players[0]);
            }
            else
            {
                GameController.instance.fsc.respondToFight(players[0]);
            }
            
        }
        
    }

    public bool isLegal(GameState gs)
    {
        return true;
    }

    
}