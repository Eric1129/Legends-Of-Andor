using System.Collections.Generic;
using UnityEngine;
using System.Threading;

[System.Serializable]
public class Move : Action
{
    public int from;
    public int to;
    private Type type;
    private string[] players;
    public bool usedWineskin;

    public Move(string playerID, int from, int to)
    {
        type = Type.Move;
        this.from = from;
        this.to = to;
        players = new string[] { playerID };

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
        return players[0].Equals(gs.turnManager.currentPlayerTurn()) && gs.getPlayer(players[0]).getHero().getHour() < gs.TIME_endTime;
    }
    public void execute(GameState gs)
    {
        //Thread thread = new Thread(() => threadExecute(gs));
        //thread.Start();
        threadExecute(gs);
        //while (thread.IsAlive)
        //{
        //}
            //    //Debug.Log("thread is alllliiiiiivvvvvvveeeeee");
            //}
            //threadExecute(gs);
            //{
            //    GameController.instance.updateGameConsoleText(gs.getPlayer(players[0]).getHeroType() + " has moved to position " + gs.playerLocations[players[0]]);
            //};
            checkMove(gs);
            gs.turnManager.passTurn();


    }
    private void threadExecute(GameState gs)
    {
        
        int pass = 1;
        int wineskinsides = gs.getPlayer(players[0]).getHero().wineskinsides;
        Debug.Log("move: " + wineskinsides);
        List <Node> path = Game.gameState.positionGraph.getPath(from, to);
        for (int i = 1; i<path.Count; i++)
        {
            int playerTimeHour = gs.getPlayer(players[0]).getHero().getHour();
            if (pass >  wineskinsides)
            {
                playerTimeHour = gs.getPlayer(players[0]).getHero().getHour() + 1;
            }
            else
            {
                usedWineskin = true;
                foreach (Wineskin w in gs.getPlayer(players[0]).getHero().getAllArticles()["Wineskin"])
                {
                    w.useArticle();
                    if(w.getNumUsed() == 2)
                    {
                        gs.getPlayer(players[0]).getHero().removeArticle2("Wineskin",w);
                        gs.addToEquimentBoard("Wineskin");
                    }
                    break;
                }
               // GameController.instance.updateGameConsoleText("USING WINESKIN");
            }
            /*if (gs.TIME_overtime <= playerTimeHour)
            {
                //going into overtime
                if(overtime == setHour)
                {
                    //each additional hour costs 2
                    if (Game.gameState.eventcard19)
                    {
                        GameController.instance.updateGameConsoleText("You will lose 2 points for the 8th hour and " + gs.TIME_overtimeCost + " willpower points per additional hour after");
                        
                    }
                    GameController.instance.overtime();
                    Debug.Log("You will lose " + gs.overtimeCost + " willpower points per additional hour");
                }

                int newPower;
                if (Game.gameState.eventcard19 && overtime == (setHour-1) )
                {
                    newPower = gs.getPlayer(players[0]).getHero().getWillpower() - 2;
                    Game.gameState.eventcard19 = false;
                }
                else
                {
                    newPower = gs.getPlayer(players[0]).getHero().getWillpower() - gs.overtimeCost;

                }

                if ( newPower <= 0)
                {
                    GameController.instance.cannotFinishMove();
                   // GameController.instance.updateGameConsoleText("You do not enough willpower points to complete your move!");
                    return false;
                   // break;
                }

                //subtract willpower points
                gs.getPlayer(players[0]).getHero().setWillpower(newPower); 

            }*/

            // Set new willpower if in overtime
            int newWillPower = gs.getPlayer(players[0]).getHero().getWillpower();
            
            if(playerTimeHour >= gs.TIME_overtime)
            {
                newWillPower -= gs.TIME_overtimeCost;
                // Check if possible to lose willpower
                if(newWillPower < 0)
                {
                    break;
                }
                gs.getPlayer(players[0]).getHero().setWillpower(newWillPower);
            }

            // Take an hour
            if( pass > wineskinsides)
            {
                gs.getPlayer(players[0]).getHero().setHour(1 + gs.getPlayer(players[0]).getHero().getHour());
                GameController.instance.setTime(players[0], gs.getPlayer(players[0]).getHero().getHour());
            }
            pass++;

            // Move player
            gs.playerLocations[players[0]] = path[i].getIndex();
            Debug.Log(path[i].getIndex());
            //Thread.Sleep(500);

            // For event card
            if(path[i].getIndex() == 57 && Game.gameState.EVENTCARD_treeOfSongBonusIsActive)
            {
                gs.getPlayer(players[0]).getHero().increaseStrength(1);
                Game.gameState.EVENTCARD_treeOfSongBonusIsActive = false;
            }


            // If past
            if(gs.getPlayer(players[0]).getHero().getHour() == gs.TIME_endTime)
            {
                break;
            }
          
        }
        gs.getPlayer(players[0]).getHero().selectedWineskin = false;
        gs.getPlayer(players[0]).getHero().wineskinsides = 0;
    }

    public void checkMove(GameState gs)
    {
        int finalDest = gs.playerLocations[players[0]];

        checkWells(gs, finalDest);
        checkFogTokens(gs, finalDest);
        //checkFarmers(gs, finalDest);

    }

    public void checkWells(GameState gs, int location)
    {
        if (gs.getWells().ContainsValue(location))
        {
            //trigger Well Scenario
            Debug.Log("YOU HAVE LANDED ON A WELL!"); 
            //GameController.instance.updateGameConsoleText("You have landed on a well!");

            foreach (Well w in gs.getWells().Keys)
            {
                if (w.getLocation() == location && !w.used)
                {
                   //GameController.instance.updateGameConsoleText("You have emptied the well and have been granted 3 willpower points!");
                    Debug.Log("landed on a well");
                    // w.emptyWell();
                    //Object.Destroy(w.getPrefab());
                    //w.getPrefab().GetComponent<Renderer>().enabled = false;
                    //GameController.instance.emptyWell(w.getPrefab());
                    //string player =  gs.turnManager.currentPlayerTurn()
                    //add 3 willpower points to the hero who emptied the well
                    //int currWillpower = gs.getPlayer(players[0]).getHero().getWillpower();
                    //gs.getPlayer(players[0]).getHero().setWillpower(currWillpower + 3);
                    // GameController.instance.emptyWellButton.IsActive();
                    GameController.instance.emptyWellButton.gameObject.SetActive(true);

                }

            }
        }
    }

    /*public void checkFarmers(GameState gs, int location)
    {
        if (gs.getFarmers().ContainsValue(location))
        {
            //trigger Well Scenario
            Debug.Log("YOU HAVE LANDED ON A FARMER!");
            //GameController.instance.updateGameConsoleText("You have landed on a well!");

            foreach (Farmer f in gs.getFarmers().Keys)
            {
                if (f.getLocation() == location)
                {
                    GameController.instance.updateGameConsoleText("You have landed on a space with a farmer!");
                    Debug.Log("emptying a well");
                    //w.emptyWell();
                    //Object.Destroy(w.getPrefab());
                    //w.getPrefab().GetComponent<Renderer>().enabled = false;
                    //GameController.instance.emptyWell(w.getPrefab());
                    //string player =  gs.turnManager.currentPlayerTurn()
                    //add 3 willpower points to the hero who emptied the well
                    //int currWillpower = gs.getPlayer(players[0]).getHero().getWillpower();
                    //gs.getPlayer(players[0]).getHero().setWillpower(currWillpower + 3);

                }
            }
        }
    }*/

    public void checkFogTokens(GameState gs, int location)
    {
        if (usedWineskin)
        {
            GameController.instance.updateGameConsoleText("You have used the  wineskin");
        }
        if (gs.getFogTokens().ContainsValue(location))
        {
            foreach (FogToken f in gs.getFogTokens().Keys)
            {
                if (f.getLocation() == location && !f.used)
                {
                    Debug.Log("using fog token");
                    f.useFogToken();
                    string token_type = f.getType();
                    if (token_type == "gold1")
                    {
                        GameController.instance.updateGameConsoleText("You have uncovered a Gold Fog Token. You will now be granted 1 gold!");
                        gs.getPlayer(players[0]).getHero().increaseGold(1);
                    }
                    else if (token_type == "event")
                    {
                        GameController.instance.updateGameConsoleText("You have uncovered an Event Fog Token.");
                        gs.uncoverEventCard();

                    }
                    else if (token_type == "wineskin")
                    {
                        GameController.instance.updateGameConsoleText("You have uncovered a wineskin Fog Token.");
                        gs.getPlayer(players[0]).getHero().addArticle(new Wineskin());
                        Game.gameState.removeFromEquimentBoard("Wineskin");

                    }
                    else if (token_type == "willpower2")
                    {
                        GameController.instance.updateGameConsoleText("You have uncovered a willpower fog token. You will now be granted 2 willpower points!");
                        gs.getPlayer(players[0]).getHero().increaseWillpower(2);

                    }
                    else if (token_type == "willpower3")
                    {
                        GameController.instance.updateGameConsoleText("You have uncovered a willpower fog token. You will now be granted 3 willpower points!");
                        gs.getPlayer(players[0]).getHero().increaseWillpower(3);

                    }
                    else if (token_type == "brew")
                    {
                        //GameController.instance.updateGameConsoleText("You have uncovered the witch Fog Token! You will be given a brew for free!");
                        GameController.instance.foundWitch(location);
                        gs.getPlayer(players[0]).getHero().addArticle(new WitchBrew());
                        Game.gameState.removeFromEquimentBoard("WitchBrew");
                        gs.witchLocation = location;
                        //GameController.instance.instantiateWitch();
                        //GameController.instance.foundWitch();

                    }
                    else if (token_type == "gor")
                    {
                        GameController.instance.updateGameConsoleText("You have uncovered a Gor Fog Token. A gor will now be placed on position " + location);
                        GameController.instance.instantiateEventGor(location);
                        //Game.gameState.addMonster(g);
                       // Game.gameState.addGor(g);
                        GameController.instance.instantiateEventGor(location);

                    }
                    else if(token_type == "strength")
                    {
                        GameController.instance.updateGameConsoleText("You have uncovered a Strength Fog Token.");
                        gs.getPlayer(players[0]).getHero().increaseStrength(1);
                    }
                    Object.Destroy(f.getPrefab());
                   //remove fog token from dictionary
                }
            }
        }
    }
}
