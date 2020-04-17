using System;
using UnityEngine;

[System.Serializable]
public class RespondTrade : Action
{
    string[] players;
    Type type;
    string[] tradeType;
    bool accept;

    public RespondTrade(string[] players, string[] tradeType, bool accept)
    {
        type = Type.RespondTrade;
        this.accept = accept;
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
        String msg = "";
        Andor.Player playerFrom = gs.getPlayer(players[0]);
        Andor.Player playerTo = gs.getPlayer(players[1]);
        if (accept)
        {
            
            
            if (tradeType[0].Equals("Gold"))
            {
                playerFrom.getHero().decGold();
                playerTo.getHero().incGold();
                msg = playerTo.getHeroType() + " has accepted your gold.";

            }else if (tradeType[0].Equals("Gemstones"))
            {
                //gemstones
            }
            else
            {
                Debug.Log("responde trade TRADE");
                //trade
                playerTo.getHero().removeArticle(tradeType[2]);
                playerFrom.getHero().addArticle(tradeType[2]);
                playerFrom.getHero().removeArticle(tradeType[1]);
                playerTo.getHero().addArticle(tradeType[1]);
                msg = playerTo.getHeroType() + " has accepted your trade request!";
            }
        }
        else
        {
            msg = playerTo.getHeroType() + " has declined your request.";
        }
        Debug.Log("playerTo " + playerTo.getHero().allArticles());
        Debug.Log("playerFrom " + playerFrom.getHero().allArticles());
        GameController.instance.sendNotif(msg, 20.0f, players[0]);
        

    }

    public bool isLegal(GameState gs)
    {
        return true;
    }
}
