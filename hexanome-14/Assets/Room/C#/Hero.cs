using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;

[System.Serializable]
[JsonObject(MemberSerialization.Fields)]
public class HeroS // : MonoBehaviour, Movable, Fightable
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


    private int gold;

    public HeroS()
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

    public string getHeroType()
    {
        return heroType;
    }
    public void setHeroType(string hero)
    {
        this.heroType = hero;
    }




}
