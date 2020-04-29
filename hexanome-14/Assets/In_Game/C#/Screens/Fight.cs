using System;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class Fight
{
    public bool fightOver = false;
    public string[] fighters;
    public Monster monster;
    private int index = 0;
    private int heroBattleValue = 0;

    public Fight(string[] fighters, Monster monster)
    {
        this.fighters = fighters;
        this.monster = monster;
        foreach(Hero h in getHeroes())
        {
            heroBattleValue += h.getStrength();
        }
    }

    
    public string currentFighter()
    {
        
        return fighters[index];
    }

    public Hero currentFighterHero()
    {
        return Game.gameState.getPlayer(currentFighter()).getHero();
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

    public void addToBattleValue(int amount)
    {
        heroBattleValue += amount;
    }

    public void resetHeroBattleValue()
    {
        heroBattleValue = 0;
    }

    public int getHeroBattleValue()
    {
        return this.heroBattleValue;
    }

    public int getIndex()
    {
        return this.index;
    }

    
}
