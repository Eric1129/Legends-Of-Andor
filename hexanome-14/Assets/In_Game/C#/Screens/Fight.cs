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
    private string currentFighter;

    public Fight(string[] fighters, Monster monster)
    {
        this.fighters = fighters;
        this.monster = monster;
        foreach(Hero h in getHeroes())
        {
            heroBattleValue += h.getStrength();
        }
        this.currentFighter = this.fighters[index];
    }

    
    public string getCurrentFighter()
    {

        return this.currentFighter;
    }

    public void setCurrentFighter(int index)
    {
        currentFighter = fighters[index];
    }

    public Hero currentFighterHero()
    {
        return Game.gameState.getPlayer(getCurrentFighter()).getHero();
    }

    public void nextFighter()
    {
        index++;
        index = index % fighters.Length;
        currentFighter = fighters[index];
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

    public void setBattleValue(int battleValue)
    {
        this.heroBattleValue = battleValue;
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
