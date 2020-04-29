using System;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DistributeReward : Action
{
    string[] players;
    Type type;
    Dictionary<string, int> rewards = new Dictionary<string, int>();

    
    public DistributeReward(string[] players, Dictionary<string, int> rewards)
    {
        type = Type.DistributeReward;
        this.players = players;
        this.rewards = rewards;

        
        
    }

    public string[] playersInvolved()
    {
        return this.players;
    }
    public Type getType()
    {
        return this.type;
    }

    public void execute(GameState gs)
    {
        Debug.Log("DistrubteReponse.execute!!!!");
        GameController.instance.fsc.distributeResponse(rewards);

    }

    public bool isLegal(GameState gs)
    {
        return true;
    }

   
}