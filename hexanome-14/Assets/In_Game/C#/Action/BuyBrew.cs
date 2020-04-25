using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BuyBrew : Action
{
    string[] players;
    Type type;

    public BuyBrew(string playerID)
    {
        type = Type.BuyBrew;
        players = new string[] { playerID };
    }

    public Type getType()
    {
        return this.type;
    }
    public string[] playersInvolved()
    {
        return players;
    }

    public bool isLegal(GameState gs)
    {
        //only if they are on the same space as a merchant
        return true;
    }

    public void execute(GameState gs)
    {
        Dictionary<string, int> playerss = new Dictionary<string, int>();
        playerss = gs.getPlayerLocations();
        int loc = playerss[players[0]];
        Debug.Log(loc);
        if(loc == gs.witchLocation && gs.witchLocation != -1)
        {
            if (gs.getPlayer(players[0]).getHero().getStrength() - gs.brewCost >= 0)
            {
                gs.getPlayer(players[0]).getHero().decreaseStrength(gs.brewCost);
                gs.getPlayer(players[0]).getHero().addArticle("brew");
                GameController.instance.updateGameConsoleText(gs.getPlayer(players[0]).getHeroType() + " has purchased the brew!");

            }
            else
            {
                GameController.instance.updateGameConsoleText(gs.getPlayer(players[0]).getHeroType() + " does not have enough strength points to purchase brew");
            }
        }

        GameController.instance.buyBrewButton.gameObject.SetActive(false);

    }

}
