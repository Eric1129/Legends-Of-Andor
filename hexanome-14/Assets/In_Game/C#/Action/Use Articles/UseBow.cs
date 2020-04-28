using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;
using System.Reflection;

[System.Serializable]
public class UseBow : Action
{
    private Type type;
    private string[] players;
    private Monster monster;

    public UseBow(string playerID, Monster m)
    {
        type = Type.UseBow;
        players = new string[] { playerID };
        monster = m;
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
        //if player with bow is on same space as monster then they cannot use the bow
        if (gs.getPlayerLocations()[players[0]] == monster.getLocation() )
        {
            return false;
        }
        return true;
    }

    public void execute(GameState gs)
    {
        gs.getPlayer(players[0]).getHero().usingBow = true;
        Debug.Log("player is using helm!");

    }

}

