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
    private int creatureBattleValue;

    public Fight(string[] fighters, Monster monster)
    {
        this.fighters = fighters;
        this.monster = monster;
        foreach(Hero h in getHeroes())
        {
            heroBattleValue += h.getStrength();
        }
        this.currentFighter = this.fighters[index];
        creatureBattleValue = monster.getStrength();
    }

    
    public string getCurrentFighter()
    {

        return this.currentFighter;
    }

    public void setCurrentFighter(int index)
    {
        this.index = index;
        currentFighter = fighters[index];
    }

    public Hero currentFighterHero()
    {
        return Game.gameState.getPlayer(getCurrentFighter()).getHero();
    }

    public void nextFighter()
    {
        Debug.Log("NEXT FIGHTER (before) " + index);
        index++;
        Debug.Log("NEXT FIGHTER (++) " + index);
        index = index % fighters.Length;
        Debug.Log("NEXT FIGHTER length " + fighters.Length);
        Debug.Log("NEXT FIGHTER (after)" + index);
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

    public void leaveFight(string leavingFighter)
    {
        List<string> listFighters = new List<string>(fighters);
        listFighters.Remove(leavingFighter);
        fighters = listFighters.ToArray();
        
    }

    
}
