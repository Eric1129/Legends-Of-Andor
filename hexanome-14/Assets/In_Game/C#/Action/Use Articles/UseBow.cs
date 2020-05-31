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

    public UseBow(string playerID)
    {
        type = Type.UseBow;
        players = new string[] { playerID };
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
      
        return true;
    }

    public void execute(GameState gs)
    {
        gs.getPlayer(players[0]).getHero().usingBow = true;
        Debug.Log("player is using helm!");

    }

}

