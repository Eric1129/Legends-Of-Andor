using System;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GetMyReward : Action
{
    string[] players;
    Type type;
    Dictionary<string, int> rewards = new Dictionary<string, int>();

    string rewardType;

    public GetMyReward(string[] players, Dictionary<string, int> rewards, string rewardType)
    {
        type = Type.GetMyReward;
        this.players = players;
        this.rewards = rewards;

        this.rewardType = rewardType;
        
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
        Debug.Log("GETTING MY REWARD");
        Hero h = Game.gameState.getPlayer(players[0]).getHero();
        if (rewardType == "gold")
        {
            Debug.Log("GETTING MY gold");
            h.increaseGold(rewards[players[0]]);
        }

        if (rewardType == "willpower")
        {
            Debug.Log("GETTING MY willpower");
            h.increaseWillpower(rewards[players[0]]);
        }
    }

    public bool isLegal(GameState gs)
    {
        return true;
    }

   
}