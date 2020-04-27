using System;
using UnityEngine;

[System.Serializable]
public class InviteFighter: Action
{
    private Type type;
    private string[] players;

    public InviteFighter()
    {
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
       
        return false;
    }

    public void execute(GameState gs)
    {
        
    }
}

