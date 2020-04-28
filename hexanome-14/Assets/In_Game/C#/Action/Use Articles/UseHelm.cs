using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;
using System.Reflection;

[System.Serializable]
public class UseHelm : Action
{
    private Type type;
    private string[] players;
    private int sides;

    public UseHelm(string playerID, int sides)
    {
        type = Type.UseHelm;
        players = new string[] { playerID };
        this.sides = sides;
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
        if(gs.getPlayer(players[0]).getHero().inBattle && (gs.getPlayer(players[0]).getHero().usingWitchBrew || gs.getPlayer(players[0]).getHero().usingHelm))
        {
            return false;
        }
        return true;
    }

    public void execute(GameState gs)
    {
        gs.getPlayer(players[0]).getHero().usingHelm = true;
        Debug.Log("player is using helm!");

    }

}
