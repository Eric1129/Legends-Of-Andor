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
                playerFrom.getHero().decreaseGold(1);
                playerTo.getHero().increaseGold(1);
                msg = playerTo.getHeroType() + " has accepted your gold.";

            }
            else if (tradeType[0].Equals("Gemstones"))
            {
                //gemstones
            }
            else
            {
                Debug.Log("responde trade TRADE");
                if (GameController.instance.checkFalconUse())
                {
                    //mark falcon as used
                    useFalcon(gs);
                }
                //trade
                Article playerTosArticle = playerTo.getHero().removeArticle(tradeType[2]);
                playerFrom.getHero().addArticle(playerTosArticle);
                Article playerFromsArticle = playerFrom.getHero().removeArticle(tradeType[1]);
                playerTo.getHero().addArticle(playerFromsArticle);
                msg = playerTo.getHeroType() + " has accepted your trade request!";
            }
        }
        else
        {
            msg = playerTo.getHeroType() + " has declined your request.";
        }
        Debug.Log(playerTo.getHeroType() + " " + playerTo.getHero().allArticlesAsString());
        Debug.Log(playerFrom.getHeroType() + "playerFrom " + playerFrom.getHero().allArticlesAsString());
        //GameController.instance.sendNotif(msg, 20.0f, players[0]);
        string[] playersToNotify = new string[1];
        playersToNotify[0] = players[0];
        GameController.instance.updateGameConsoleText(msg, playersToNotify);
        GameController.instance.clearTrade();


    }

    public bool isLegal(GameState gs)
    {
        return true;
    }

    //check valid use of falcon
    public bool useFalcon(GameState gs)
    {
        if (Game.gameState.getPlayer(players[0]).getHero().hasArticle("Falcon"))
        {
            foreach (Falcon f in gs.getPlayer(players[0]).getHero().heroArticles["Falcon"])
            {
                if (!f.checkUsedToday())
                {
                    f.useArticle();
                    Debug.Log("falcon is valid: " + f.checkUsedToday());
                    return true;
                }
            }
        }
        else if (Game.gameState.getPlayer(players[1]).getHero().hasArticle("Falcon"))
        {
            foreach (Falcon f in gs.getPlayer(players[1]).getHero().heroArticles["Falcon"])
            {
                if (!f.checkUsedToday())
                {
                    f.useArticle();
                    Debug.Log("falcon is valid: " + f.checkUsedToday());
                    return true;
                }
            }
        }
        return false;

    }
}