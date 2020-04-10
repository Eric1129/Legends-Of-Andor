using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;

[System.Serializable]
[JsonObject(MemberSerialization.Fields)]
public class Hero // : MonoBehaviour, Movable, Fightable
{
    // private GameObject sphere;
    // private SpriteRenderer spriteRenderer;
    private string heroType = "";
    private string myTag;

    // this is the tag of the sphere gameObject which we show
    // as a hero's position! (currently the gray squished sphere)
    private string sphereTag;

    //private MoveStrategy moveStrat;
    //private DiceRollStrategy diceRollStrat;

    // don't care which farmers, just that this number exists on our space.
    private int numFarmers;
    private bool hasThorald;


    private int gold = 0;
    private int strength = 0;
    private int willpower = 0;

    private int hour = 0;

    public Hero()
    {

    }

    public int getGold()
    {
        return gold;
    }
    public void setGold(int gold)
    {
        this.gold = gold;
    }

    public int getStrength()
    {
        return strength;
    }
    public void setStrength(int strength)
    {
        this.strength = strength;
    }

    public int getWillpower()
    {
        return willpower;
    }
    public void setWillpower(int willpower)
    {
        this.willpower = willpower;
    }

    public int getHour()
    {
        return hour;
    }
    public void setHour(int hour)
    {
        this.hour = hour;
    }

    public string getHeroType()
    {
        return heroType;
    }
    public void setHeroType(string hero)
    {
        this.heroType = hero;
    }




}
