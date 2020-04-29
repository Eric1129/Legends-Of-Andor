using System;
using UnityEngine;

[System.Serializable]
public class InviteFighter: Action
{
    private Type type;
    private string[] players;

    public InviteFighter(string[] players)
    {
        this.players = players;
        this.type = Type.InviteFighter;
    }

    

    public Type getType()
    {
        return type;
    }

    public string[] playersInvolved()
    {
        return players;
    }


    public bool isLegal(GameState gs)
    {

        return players[0].Equals(gs.turnManager.currentPlayerTurn());
    }

    public void execute(GameState gs)
    {
        GameController.instance.sendFightRequest(players);
    }
}

