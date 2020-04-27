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
            if(gs.getPlayer(players[0]).getHeroType() == "Male Archer" || gs.getPlayer(players[0]).getHeroType() == "Female Archer")
            {
                GameController.instance.archerBuysBrew();
                if (gs.getPlayer(players[0]).getHero().getGold() - (gs.brewCost-1) >= 0)
                {
                    gs.getPlayer(players[0]).getHero().decreaseGold(gs.brewCost-1);
                    gs.getPlayer(players[0]).getHero().addArticle(new WitchBrew());
                    Game.gameState.removeFromEquimentBoard("WitchBrew");
                    GameController.instance.updateGameConsoleText(gs.getPlayer(players[0]).getHeroType() + " has purchased the brew!");

                }
                else
                {
                    GameController.instance.updateGameConsoleText(gs.getPlayer(players[0]).getHeroType() + " does not have enough gold to purchase the brew");
                }
            }
            else
            {
                if (gs.getPlayer(players[0]).getHero().getGold() - gs.brewCost >= 0)
                {
                    gs.getPlayer(players[0]).getHero().decreaseGold(gs.brewCost);
                    gs.getPlayer(players[0]).getHero().addArticle(new WitchBrew());
                    Game.gameState.removeFromEquimentBoard("WitchBrew");

                    GameController.instance.updateGameConsoleText(gs.getPlayer(players[0]).getHeroType() + " has purchased the brew!");

                }
                else
                {
                    GameController.instance.updateGameConsoleText(gs.getPlayer(players[0]).getHeroType() + " does not have enough gold to purchase the brew");
                }
            }
            
        }

        GameController.instance.buyBrewButton.gameObject.SetActive(false);

    }

}
