using System;
using UnityEngine;

[System.Serializable]
public class ExitFight : Action
{
    string[] players;
    Type type;
   
    

    public ExitFight(string playerId)
    {
        type = Type.ExitFight;

       players = new string[] { playerId };        
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
        
        foreach(string playerId in players){
            gs.getPlayer(playerId).getHero().usingHelm = false;
            gs.getPlayer(playerId).getHero().usingBow = false;
            gs.getPlayer(playerId).getHero().usingWitchBrew = false;
            gs.getPlayer(playerId).getHero().usingShield= false;
            gs.getPlayer(playerId).getHero().selectedArticle= false;

        }

    }

    public bool isLegal(GameState gs)
    {
        return true;
    }

   
}