using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;
using System.Reflection;

[System.Serializable]
public class UseShield : Action
{
	private Type type;
	private string[] players;
	private int sides;

	public UseShield(string playerID)
	{
		type = Type.UseShield;
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
		if (gs.getPlayer(players[0]).getHero().inBattle && (gs.getPlayer(players[0]).getHero().usingWitchBrew || gs.getPlayer(players[0]).getHero().usingHelm))
		{
			return false;
		}
		return true;
	}

	public void execute(GameState gs)
	{
		foreach (Shield s in gs.getPlayer(players[0]).getHero().heroArticles["Shield"])
		{
			if (s.getNumUsed() < 2)
			{
				s.useArticle();
				gs.getPlayer(players[0]).getHero().usingShield = true;
                gs.getPlayer(players[0]).getHero().selectedArticle = true;

                Debug.Log("player is using shield!");

				if (s.getNumUsed() == 2)
				{
					//removed once fully used 
					gs.getPlayer(players[0]).getHero().removeArticle2("Shield", s);
					gs.addToEquimentBoard("Shield");
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