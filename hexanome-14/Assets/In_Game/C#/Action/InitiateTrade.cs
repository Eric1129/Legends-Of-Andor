using System;
using UnityEngine;

[System.Serializable]
public class InitiateTrade : Action
{
    private Type type;
    private string[] players;

    private string[] tradeType;

    public InitiateTrade(string[] players, string[] tradeType)
    {
        type = Type.InitiateTrade;
        this.players = new string[2];
        this.tradeType = new string[3];


        this.players = players;
        this.tradeType = tradeType;
        for (int i = 0; i < 3; i++)
        {

            this.tradeType[i] = tradeType[i];
        }
        for (int i = 0; i < 2; i++)
        {
            this.players[i] = players[i];

        }


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
        int location0 = 0;
        //Game.gameState.playerLocations.TryGetValue(players[0], out location0);
        gs.playerLocations.TryGetValue(players[0], out location0);
        int location1 = -1;
        gs.playerLocations.TryGetValue(players[1], out location1);
        if (location0 == location1)
        {
            return true;
        }

        if (Game.gameState.getPlayer(players[0]).getHero().hasArticle("Falcon")
            || Game.gameState.getPlayer(players[1]).getHero().hasArticle("Falcon"))
        {
            return true;
        }
        return false;
    }

    public void execute(GameState gs)
    {
        GameController.instance.sendTradeRequest(tradeType, players[0], players[1]);
    }
}
