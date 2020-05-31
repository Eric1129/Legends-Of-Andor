using System;
using UnityEngine;

[System.Serializable]
public class WinBattle : Action
{
    string[] players;
    Type type;
    string currentFighter;
    

    public WinBattle(string[] players, string currentFighter)
    {
        type = Type.WinBattle;
        this.players = players;
        this.currentFighter = currentFighter;
        
        
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
        GameController.instance.fsc.setHostPlayer(currentFighter);

    }

    public bool isLegal(GameState gs)
    {
        return true;
    }

   
}