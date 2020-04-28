using System;
using UnityEngine;

[System.Serializable]
public class StartFight : Action
{
    string[] players;
    Type type;
    int fightType;
    

    public StartFight(string[] players, int fightType)
    {
        type = Type.StartFight;
        this.players = players;
        this.fightType = fightType;
        
        
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
       if(fightType == 0)
        {
            GameController.instance.fsc.startSoloFight();
        }
        else
        {
            GameController.instance.fsc.startCollabFight();
        }

    }

    public bool isLegal(GameState gs)
    {
        return true;
    }

   
}