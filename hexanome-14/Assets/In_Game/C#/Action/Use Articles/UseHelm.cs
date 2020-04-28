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

    public UseHelm(string playerID)
    {
        type = Type.UseHelm;
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
        if(gs.getPlayer(players[0]).getHero().inBattle && (gs.getPlayer(players[0]).getHero().usingWitchBrew || gs.getPlayer(players[0]).getHero().usingHelm))
        {
            return false;
        }
        return true;
    }

    public void execute(GameState gs)
    {
        foreach (Helm h in gs.getPlayer(players[0]).getHero().heroArticles["Helm"])
        {
            if (h.getNumUsed() < 2)
            {
                h.useArticle();
                gs.getPlayer(players[0]).getHero().usingHelm = true;
                gs.getPlayer(players[0]).getHero().selectedArticle = true;
                Debug.Log("player is using helm!");

                if (h.getNumUsed() == 2)
                {
                    //removed once fully used 
                    gs.getPlayer(players[0]).getHero().removeArticle2("Helm", h);
                }

                break;


                //    gs.getPlayer(players[0]).getHero().removeArticle2("WitchBrew", w);
                //    gs.addToEquimentBoard("WitchBrew");
                //}
                //break;
            }
        }
        

    }

}
