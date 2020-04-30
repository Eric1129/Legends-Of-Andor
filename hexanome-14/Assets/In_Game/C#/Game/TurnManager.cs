using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;

[JsonObject(MemberSerialization.Fields)]
public class TurnManager
{

    private Queue<string> turnQueue;
    private string[] playerOrder;

    public TurnManager(string[] order)
    {
        turnQueue = new Queue<string>();
        playerOrder = order;

        foreach(string player in order)
        {
            turnQueue.Enqueue(player);
            Debug.Log("order: " + player);

        }
    }

    public string currentPlayerTurn()
    {
        Debug.Log("Get Player: " + turnQueue.Peek());
        return turnQueue.Peek();
    }
    // cycle queue (go to next player)
    public void passTurn()
    {
        turnQueue.Enqueue(turnQueue.Dequeue());

        // Maybe do some call here to let the others know
        Debug.Log("pass turn: " + turnQueue.Peek());

    }

    // remove from queue and go to the next player
    public void endTurn()
    {
        turnQueue.Dequeue();
        Debug.Log("end turn: " + turnQueue.Peek());

    }

    public bool roundDone()
    {
        return turnQueue.Count == 0 ? true : false;
    }

    public void reset()
    {
        while(turnQueue.Count > 0)
        {
            turnQueue.Dequeue();
        }
        foreach (string player in playerOrder)
        {
            turnQueue.Enqueue(player);
        }
    }
}
