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
        Thread thread = new Thread(() => threadExecute(gs));
        thread.Start();
        while(thread.IsAlive)
        {
            //Debug.Log("thread is alllliiiiiivvvvvvveeeeee");
        }
        GameController.instance.updateGameConsoleText(gs.getPlayer(players[0]).getHeroType() + " has moved to position " + gs.playerLocations[players[0]]);
        checkMove(gs);
        gs.turnManager.passTurn();


    }
    private void threadExecute(GameState gs)
    {
        List<Node> path = Game.positionGraph.getPath(from, to);
        for (int i = 1; i<path.Count; i++)
        {
            // Move
            gs.playerLocations[players[0]] = path[i].getIndex();
            Debug.Log(path[i].getIndex());

            // Take an hour
            gs.getPlayer(players[0]).getHero().setHour(1 + gs.getPlayer(players[0]).getHero().getHour());
            GameController.instance.setTime(players[0], gs.getPlayer(players[0]).getHero().getHour());

            if(gs.getPlayer(players[0]).getHero().getHour() == 10)
            {
                break;
            }

            //Thread.Sleep(500);
        }
    }

    public void checkMove(GameState gs)
    {
        int finalDest = gs.playerLocations[players[0]];

        checkWells(gs, finalDest);
        checkFogTokens(gs, finalDest);

    }

    public void checkWells(GameState gs, int location)
    {
        if (gs.getWells().ContainsValue(location))
        {
            //trigger Well Scenario
            Debug.Log("YOU HAVE LANDED ON A WELL!");
            GameController.instance.updateGameConsoleText("You have landed on a well!");
            foreach (Well w in gs.getWells().Keys)
            {
                if (w.getLocation() == location && !w.used)
                {
                    Debug.Log("emptying a well");
                    w.emptyWell();
                    //Object.Destroy(w.getPrefab());
                    //w.getPrefab().GetComponent<Renderer>().enabled = false;
                    w.getPrefab().GetComponent<Renderer>().material.color = Color.gray;
                    //GameController.instance.emptyWell(w.getPrefab());
                    //string player =  gs.turnManager.currentPlayerTurn();

                    //add 3 willpower points to the hero who emptied the well
                    int currWillpower = gs.getPlayer(players[0]).getHero().getWillpower();
                    gs.getPlayer(players[0]).getHero().setWillpower(currWillpower + 3);

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

                    }
                    else if (token_type == "wineskin")
                    {
                        GameController.instance.updateGameConsoleText("You have uncovered a wineskin Fog Token.");

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
                        GameController.instance.updateGameConsoleText("You have uncovered a brew Fog Token");

                    }
                    else if (token_type == "gor")
                    {
                        GameController.instance.updateGameConsoleText("You have uncovered a Gor Fog Token. A gor will now be placed on position " + location);
                        Gor g = new Gor(Game.positionGraph.getNode(location));
                        //Game.gameState.addMonster(g);
                       // Game.gameState.addGor(g);
                        GameController.instance.instantiateEventGor(g, location);

                    }
                    Object.Destroy(f.getPrefab());
                }
            }
        }
    }
}
