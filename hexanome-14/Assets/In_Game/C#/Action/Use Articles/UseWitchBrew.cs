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
    private int numUsed;

    public UseWitchBrew(string playerID)
    {
        type = Type.UseWitchBrew;
        players = new string[] { playerID };
        numUsed = 0;
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
        //if
        foreach(WitchBrew w in gs.getPlayer(players[0]).getHero().heroArticles["WitchBrew"])
        {
            if (w.getNumUsed() < 2)
            {
                w.useArticle();
                gs.getPlayer(players[0]).getHero().usingWitchBrew = true;
                gs.getPlayer(players[0]).getHero().selectedArticle = true;
                Debug.Log("player is using the witch's brew!");

                if (w.getNumUsed() == 2)
                {
                    //removed once fully used 
                    gs.getPlayer(players[0]).getHero().removeArticle2("WitchBrew", w);
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
