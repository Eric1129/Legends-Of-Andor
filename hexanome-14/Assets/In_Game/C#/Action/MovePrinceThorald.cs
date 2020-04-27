using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading;
using System;

[System.Serializable]
public class MovePrinceThorald : Action
{
    public int from;
    public int to;
    private Type type;
    public string[] players;

    public MovePrinceThorald(string playerID, int from, int to)
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
        return (players[0].Equals(gs.turnManager.currentPlayerTurn()) && gs.getPlayer(players[0]).getHero().getHour() < 10);
    }


    public void execute(GameState gs)
    {
        Thread thread = new Thread(() => threadExecute(gs));
        thread.Start();
        while (thread.IsAlive)
        {
            //Debug.Log("thread is alllliiiiiivvvvvvveeeeee");
        }
        GameController.instance.updateGameConsoleText("Prince Thorald has moved to position " + gs.getPrinceThorald()[0].getLocation());
        //checkMove(gs);
        gs.turnManager.passTurn();


    }

    private void threadExecute(GameState gs)
    {
        List<Node> path = Game.gameState.positionGraph.getPath(from, to);
        int spaces = path.Count;
        //int hours = Math.Ceiling((decimal)spaces / 4);
        Debug.Log("Number of spaces: " + path.Count);

        for (int i = 1; i < path.Count; i++)
        {
            // Move
            gs.getPrinceThorald()[0].setLocationNode(path[i]);
            Debug.Log(path[i].getIndex());

            // Take an hour for every 4 spaces moved by prince thorald
            if (i % 4 == 0 || i == 1)
            {
                gs.getPlayer(players[0]).getHero().setHour(1 + gs.getPlayer(players[0]).getHero().getHour());
                GameController.instance.setTime(players[0], gs.getPlayer(players[0]).getHero().getHour());
            }
           
            if (gs.getPlayer(players[0]).getHero().getHour() == 10)
            {
                break;
            }

            //Thread.Sleep(500);
        }
    }
    //// Start is called before the first frame update
    //void Start()
    //{

    //}

    //// Update is called once per frame
    //void Update()
    //{

    //}
}




   

    
   