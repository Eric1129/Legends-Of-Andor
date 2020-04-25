﻿using System.Collections.Generic;
using UnityEngine;
using System.Threading;

[System.Serializable]
public class Move : Action
{
    public int from;
    public int to;
    private Type type;
    private string[] players;

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
        return players[0].Equals(gs.turnManager.currentPlayerTurn()) && gs.getPlayer(players[0]).getHero().getHour() < 10;
    }
    public void execute(GameState gs)
    {
        //Thread thread = new Thread(() => threadExecute(gs));
        //thread.Start();
        //while(thread.IsAlive)
        //{
        //    //Debug.Log("thread is alllliiiiiivvvvvvveeeeee");
        //}
        if (threadExecute(gs))
        {
            GameController.instance.updateGameConsoleText(gs.getPlayer(players[0]).getHeroType() + " has moved to position " + gs.playerLocations[players[0]]);
        };
        checkMove(gs);
        gs.turnManager.passTurn();


    }
    private bool threadExecute(GameState gs)
    {
        List<Node> path = Game.positionGraph.getPath(from, to);
        int pass = 1;
        for (int i = 1; i<path.Count; i++)
        {
            int overtime = gs.overtime;
            int setHour = gs.getPlayer(players[0]).getHero().getHour() + 1;
            if (overtime <= setHour)
            {
                //going into overtime
                if(overtime == setHour)
                {
                    //each additional hour costs 2
                    if (Game.gameState.eventcard19)
                    {
                        GameController.instance.updateGameConsoleText("You will lose 2 points for the 8th hour and " + gs.overtimeCost + " willpower points per additional hour after");
                        
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

            }
            // Move
            gs.playerLocations[players[0]] = path[i].getIndex();
            Debug.Log(path[i].getIndex());
            if(path[i].getIndex() == 57 && Game.gameState.eventcard3)
            {
                gs.getPlayer(players[0]).getHero().increaseStrength(1);
                Game.gameState.eventcard3 = false;
            }

            // Take an hour
            gs.getPlayer(players[0]).getHero().setHour(1 + gs.getPlayer(players[0]).getHero().getHour());
            GameController.instance.setTime(players[0], gs.getPlayer(players[0]).getHero().getHour());

            if(gs.getPlayer(players[0]).getHero().getHour() == gs.endtime)
            {
                break;
            }
          
            //Thread.Sleep(500);
        }
        return true;
    }

    public void checkMove(GameState gs)
    {
        int finalDest = gs.playerLocations[players[0]];

        //checkWells(gs, finalDest);
        checkFogTokens(gs, finalDest);
        checkFarmers(gs, finalDest);

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

    public void checkFarmers(GameState gs, int location)
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
    }

    public void checkFogTokens(GameState gs, int location)
    {
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
                        gs.getPlayer(players[0]).getHero().addArticle("wineskin");
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
                        GameController.instance.updateGameConsoleText("You have uncovered the witch Fog Token! You will be given a brew for free!");
                        gs.getPlayer(players[0]).getHero().addArticle("brew");
                        GameController.instance.foundWitch();

                    }
                    else if (token_type == "gor")
                    {
                        GameController.instance.updateGameConsoleText("You have uncovered a Gor Fog Token. A gor will now be placed on position " + location);
                        GameController.instance.instantiateEventGor(location);
                        //Game.gameState.addMonster(g);
                       // Game.gameState.addGor(g);
                        GameController.instance.instantiateEventGor(location);

                    }
                    Object.Destroy(f.getPrefab());
                   //remove fog token from dictionary
                }
            }
        }
    }
}
