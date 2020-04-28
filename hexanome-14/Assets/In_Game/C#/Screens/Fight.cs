using System;
using System.Collections.Generic;
using UnityEngine;

public class Fight
{
    public bool fightOver = false;
    public string[] fighters;
    public Monster monster;
    private int index = 0;

    public Fight(string[] fighters, Monster monster)
    {
        this.fighters = fighters;
        this.monster = monster;
    }

    //have a loop here that keeps track of turns of fighters
    public string currentFighter()
    {
        Debug.Log("current fighter " + index);
        return fighters[index];
    }

    public void nextFighter()
    {
        index++;
        index = index % fighters.Length;
    }

    public List<Hero> getHeroes()
    {
        List<Hero> allHeroes = new List<Hero>();
        foreach(string p in fighters)
        {
            allHeroes.Add(Game.gameState.getPlayer(p).getHero());
        }
        return allHeroes;

    }

    
}
