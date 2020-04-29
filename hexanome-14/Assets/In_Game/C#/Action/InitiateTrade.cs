using System;
using UnityEngine;

[System.Serializable]
public class InitiateTrade : Action
{
    private Type type;
    private string[] players;

    private string[] tradeType;
    private bool usingFalcon;

    public InitiateTrade(string[] players, string[] tradeType)
    {
        type = Type.InitiateTrade;
        this.players = new string[2];
        this.tradeType = new string[3];
        usingFalcon = false;


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
        

        //NOW NEED TO CHECK IF TRADE IS POSSIBLE

        ////////////////////////////////////////////////////////////////////////////////////////////////////////
        //////////////////////////////////PLAYER 1///////////////////////////////////////////////////////////////

        int small_articles1 = 0;
        foreach (String key in Game.myPlayer.getHero().allArticlesAsStringList())
        {
            if (key == "Wineskin")
            {
                small_articles1 += Game.myPlayer.getHero().getAllArticles()["Wineskin"].Count;
            }
            if (key == "Telescope")
            {
                small_articles1 += Game.myPlayer.getHero().getAllArticles()["Telescope"].Count;
            }
            if (key == "WitchBrew")
            {
                small_articles1 += Game.myPlayer.getHero().getAllArticles()["WitchBrew"].Count;
            }
        }

        int helm1 = 0;
        if (Game.myPlayer.getHero().hasArticle("Helm"))
        {
            helm1 = Game.myPlayer.getHero().getAllArticles()["Helm"].Count;
        }

        int large_articles1 = 0;
        foreach (String key in Game.myPlayer.getHero().allArticlesAsStringList())
        {
            if (key == "Shield")
            {
                large_articles1 += Game.myPlayer.getHero().getAllArticles()["Shield"].Count;
            }
            if (key == "Falcon")
            {
                large_articles1 += Game.myPlayer.getHero().getAllArticles()["Falcon"].Count;
            }
            if (key == "Bow")
            {
                large_articles1 += Game.myPlayer.getHero().getAllArticles()["Bow"].Count;
            }
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////////
        //////////////////////////////////PLAYER 2///////////////////////////////////////////////////////////////
        ///

        int small_articles2 = 0;
        foreach (String key in gs.getPlayer(players[1]).getHero().allArticlesAsStringList())
        {
            if (key == "Wineskin")
            {
                small_articles2 += gs.getPlayer(players[1]).getHero().getAllArticles()["Wineskin"].Count;
            }
            if (key == "Telescope")
            {
                small_articles2 += gs.getPlayer(players[1]).getHero().getAllArticles()["Telescope"].Count;
            }
            if (key == "WitchBrew")
            {
                small_articles2 += gs.getPlayer(players[1]).getHero().getAllArticles()["WitchBrew"].Count;
            }
        }

        int helm2 = 0;
        if (Game.myPlayer.getHero().hasArticle("Helm"))
        {
            helm2 = gs.getPlayer(players[1]).getHero().getAllArticles()["Helm"].Count;
        }

        int large_articles2 = 0;
        foreach (String key in gs.getPlayer(players[1]).getHero().allArticlesAsStringList())
        {
            if (key == "Shield")
            {
                large_articles2 += gs.getPlayer(players[1]).getHero().getAllArticles()["Shield"].Count;
            }
            if (key == "Falcon")
            {
                large_articles2 += gs.getPlayer(players[1]).getHero().getAllArticles()["Falcon"].Count;
            }
            if (key == "Bow")
            {
                large_articles2 += gs.getPlayer(players[1]).getHero().getAllArticles()["Bow"].Count;
            }
        }

        /////////////////////////////////////////////////////////////////////////////////////////////////////////

        if (tradeType[0] == "trade")
        {
           

            if (tradeType[1] == "Falcon" || tradeType[1] == "Bow" || tradeType[1] == "Shield")
            {
                if (tradeType[2] == "Falcon" || tradeType[2] == "Bow" || tradeType[2] == "Shield")
                {
                    return true;
                }
                else
                {
                    if(tradeType[2] == "Helm")
                    {
                        if(helm1 >= 1 || large_articles2 >=1)
                        {
                            GameController.instance.invalidTradeNotify(gs.getPlayer(players[0]));

                            return false;
                        }
                        return true;
                    }

                    if (tradeType[2] == "Wineskin" || tradeType[2] == "WitchBrew" || tradeType[2] == "Telescope")
                    {
                        if (small_articles1 >= 3 || large_articles2 >= 1)
                        {
                            GameController.instance.invalidTradeNotify(gs.getPlayer(players[0]));

                            return false;
                        }
                        return true;
                    }

                }
                return true;
            }

            //////////now if trading small
            if (tradeType[1] == "Telescope" || tradeType[1] == "Wineskin" || tradeType[1] == "WitchBrew")
            {
                if (tradeType[2] == "Telescope" || tradeType[2] == "Wineskin" || tradeType[2] == "WitchBrew")
                {
                    return true;
                }
                else
                {
                    if (tradeType[2] == "Helm")
                    {
                        if (helm1 >= 1 || small_articles2 >= 3)
                        {
                            GameController.instance.invalidTradeNotify(gs.getPlayer(players[0]));

                            return false;
                        }
                        return true;
                    }

                    if (tradeType[2] == "Falcon" || tradeType[2] == "Bow" || tradeType[2] == "Shield")
                    {
                        if (large_articles1 >= 1 || small_articles2 >= 3)
                        {
                            GameController.instance.invalidTradeNotify(gs.getPlayer(players[0]));

                            return false;
                        }
                        return true;
                    }

                }
                return true;
            }


            if (tradeType[1] == "Helm")
            {
                if (tradeType[2] == "Helm")
                {
                    return true;
                }
                else
                {
                    if (tradeType[2] == "Wineskin" || tradeType[2] == "WitchBrew" || tradeType[2] == "Telescope")
                    {
                        if (small_articles1 >= 3 || helm2 >= 1)
                        {
                            GameController.instance.invalidTradeNotify(gs.getPlayer(players[0]));

                            return false;
                        }
                        return true;
                    }

                    if (tradeType[2] == "Falcon" || tradeType[2] == "Bow" || tradeType[2] == "Shield")
                    {
                        if (large_articles1 >= 1 || helm2 >=1)
                        {
                            GameController.instance.invalidTradeNotify(gs.getPlayer(players[0]));

                            return false;
                        }
                        return true;
                    }

                }
                return true;
            }


            //////////////////////////////////////////////CHECK IF VALID FOR PLAYER 2/////////////////////////////////////////////


            if (tradeType[2] == "Falcon" || tradeType[2] == "Bow" || tradeType[2] == "Shield")
            {
                if (tradeType[1] == "Falcon" || tradeType[1] == "Bow" || tradeType[1] == "Shield")
                {
                    return true;
                }
                else
                {
                    if (tradeType[1] == "Helm")
                    {
                        if (helm2 >= 1 || large_articles1 >= 1)
                        {
                            GameController.instance.invalidTradeNotify(gs.getPlayer(players[0]));

                            return false;
                        }
                        return true;
                    }

                    if (tradeType[1] == "Wineskin" || tradeType[1] == "WitchBrew" || tradeType[1] == "Telescope")
                    {
                        if (small_articles2 >= 3 || large_articles1 >= 1)
                        {
                            GameController.instance.invalidTradeNotify(gs.getPlayer(players[0]));

                            return false;
                        }
                        return true;
                    }

                }
                return true;
            }

            //////////now if trading small
            if (tradeType[2] == "Telescope" || tradeType[2] == "Wineskin" || tradeType[2] == "WitchBrew")
            {
                if (tradeType[1] == "Telescope" || tradeType[1] == "Wineskin" || tradeType[1] == "WitchBrew")
                {
                    return true;
                }
                else
                {
                    if (tradeType[1] == "Helm")
                    {
                        if (helm2 >= 1 || small_articles1 >= 3)
                        {
                            GameController.instance.invalidTradeNotify(gs.getPlayer(players[0]));

                            return false;
                        }
                        return true;
                    }

                    if (tradeType[1] == "Falcon" || tradeType[1] == "Bow" || tradeType[1] == "Shield")
                    {
                        if (large_articles2 >= 1 || small_articles1 >= 3)
                        {
                            GameController.instance.invalidTradeNotify(gs.getPlayer(players[0]));

                            return false;
                        }
                        return true;
                    }

                }
                return true;
            }


            if (tradeType[2] == "Helm")
            {
                if (tradeType[1] == "Helm")
                {
                    return true;
                }
                else
                {
                    if (tradeType[1] == "Wineskin" || tradeType[1] == "WitchBrew" || tradeType[1] == "Telescope")
                    {
                        if (small_articles2 >= 3 || helm1 >= 1)
                        {
                            GameController.instance.invalidTradeNotify(gs.getPlayer(players[0]));

                            return false;
                        }
                        return true;
                    }

                    if (tradeType[1] == "Falcon" || tradeType[1] == "Bow" || tradeType[1] == "Shield")
                    {
                        if (large_articles2 >= 1 || helm1 >= 1)
                        {
                            GameController.instance.invalidTradeNotify(gs.getPlayer(players[0]));
                            return false;
                        }
                        return true;
                    }

                }
                return true;
            }

   }

        int location0 = 0;
        //Game.gameState.playerLocations.TryGetValue(players[0], out location0);
        gs.playerLocations.TryGetValue(players[0], out location0);
        int location1 = -1;
        gs.playerLocations.TryGetValue(players[1], out location1);
        if (location0 == location1)
        {
            return true;
        }

        //cannot use falcon in battle
        if (!Game.gameState.getPlayer(players[0]).getHero().inBattle && Game.gameState.getPlayer(players[1]).getHero().inBattle && checkPlayersCanUseFalcon(gs))
        {
            usingFalcon = true;
            Debug.Log("removing falcon ye");
            //checkPlayersCanUseFalcon(gs);
            return true;
        }

       return false;

}

    public void execute(GameState gs)
    {
        GameController.instance.sendTradeRequest(tradeType, players[0], players[1], usingFalcon);
        Debug.Log("using falcon: " + usingFalcon);
    }


    //check valid use of falcon
    public bool checkPlayersCanUseFalcon(GameState gs)
    {
        if (Game.gameState.getPlayer(players[0]).getHero().hasArticle("Falcon"))
        {
            foreach (Falcon f in gs.getPlayer(players[0]).getHero().heroArticles["Falcon"])
            {
                if (!f.checkUsedToday())
                {
                    //f.useArticle();
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
                    //f.useArticle();
                    Debug.Log("falcon is valid: " + f.checkUsedToday());
                    return true;
                }
            }
        }
        return false;

    }
}

