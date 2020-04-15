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
        
    }
    private void threadExecute(GameState gs)
    {
        List<Node> path = Game.positionGraph.getPath(from, to);
        for (int i = 1; i<2; i++)
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

            Thread.Sleep(500);
        }

        gs.turnManager.passTurn();


        int finalDest = gs.playerLocations[players[0]];
        if (gs.getWells().ContainsValue(finalDest))
        {
            //trigger Well Scenario
            Debug.Log("YOU HAVE LANDED ON A WELL!");
            foreach(Well w in gs.getWells().Keys)
            {
                if(w.getLocation() == finalDest && !w.used)
                {
                    Debug.Log("emptying a well");
                    w.emptyWell();
                }
            }
        }
        
    }
}
