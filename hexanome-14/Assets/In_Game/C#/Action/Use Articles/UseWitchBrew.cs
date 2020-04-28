using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;
using System.Reflection;

[System.Serializable]
public class UseWitchBrew : Action
{
    private Type type;
    private string[] players;
    private int sides;

    public UseWitchBrew(string playerID, int sides)
    {
        type = Type.UseWitchBrew;
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
        if (gs.getPlayer(players[0]).getHero().inBattle && (gs.getPlayer(players[0]).getHero().usingHelm || gs.getPlayer(players[0]).getHero().usingWitchBrew))
        {
            return false;
        }
        return true;
    }

    public void execute(GameState gs)
    {
        gs.getPlayer(players[0]).getHero().usingWitchBrew = true;
        Debug.Log("player is using the witch's brew!");

    }

}
