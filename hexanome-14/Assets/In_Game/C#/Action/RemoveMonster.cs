using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class RemoveMonster : Action
{
    string[] players;
    Type type;
    Monster monster;
    

    public RemoveMonster(string[] players, Monster m)
    {
        type = Type.RemoveMonster;
        this.players = players;
        this.monster = m;
        
        
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
        //GameController.instance.fsc.setHostPlayer(currentFighter);
        gs.removeMonster(monster);

    }

    public bool isLegal(GameState gs)
    {
        return true;
    }
}
