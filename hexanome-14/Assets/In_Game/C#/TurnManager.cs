using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnManager
{

    private Queue<string> turnQueue;

    public TurnManager(Queue<string> orderByPlayerName)
    {
        turnQueue = orderByPlayerName;
    }

    public string currentPlayerTurn()
    {
        return turnQueue.Peek();
    }
    // cycle queue (go to next player)
    public void passTurn()
    {
        turnQueue.Enqueue(turnQueue.Dequeue());

        // Maybe do some call here to let the others know
    }

    // remove from queue and go to the next player
    public void endTurn()
    {
        turnQueue.Dequeue();
    }

}
