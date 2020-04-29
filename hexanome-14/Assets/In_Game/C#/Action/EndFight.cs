using System;
using UnityEngine;

[System.Serializable]
public class EndFight : Action
{
    string[] players;
    Type type;
   
    

    public EndFight(string[] players)
    {
        type = Type.EndFight;

        this.players = players;
        
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

        }
        Array.Clear(players, 0, players.Length);
        GameController.instance.fsc.fightOverAction();

    }

    public bool isLegal(GameState gs)
    {
        return true;
    }

   
}