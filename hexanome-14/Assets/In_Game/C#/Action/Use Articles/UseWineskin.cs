using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;
using System.Reflection;

[System.Serializable]
public class UseWineskin : Action
{
    private Type type;
    private string[] players;
    private int sides;

    public UseWineskin(string playerID, int sides)
    {
        type = Type.UseWineskin;
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
        return true;
    }

    public void execute(GameState gs)
    {
        gs.getPlayer(players[0]).getHero().wineskinsides = this.sides;
        Debug.Log("usewineskin: " + sides);

    }

}
