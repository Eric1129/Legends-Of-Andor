using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hero : Movable, Fightable
{

    // private GameObject sphere;
    // private SpriteRenderer spriteRenderer;
    private string heroType;
    private string myTag;

    // this is the tag of the sphere gameObject which we show
    // as a hero's position! (currently the gray squished sphere)
    private string sphereTag;

    private MoveStrategy moveStrat;
    private DiceRollStrategy diceRollStrat;

    // don't care which farmers, just that this number exists on our space.
    private int numFarmers;
    private bool hasThorald;

    private int numHoursLeft;

    private int goldCoins;

    private int overTimeHours = 3; 


    // all heroes start with 1 strength point
    private int strength = 1;
    // all heroes start with 7 willPower points
    private int willPower = 7;
    // rank differs per hero so initialize this in child classes.
    private int rank;
    private int NumDice; 


    // use List not ArrayList -- see microsoft docs on arraylist
    private List<Article> myArticles;

    public Hero(){ }


    // called AFTER gameObject for this script has been created
    // AND the necessary components have been added.
    public Hero(string mySphereTag, string thisMyTag)
    {
        myTag = thisMyTag;
        heroType = myTag;

        // need to keep track of this things location.
        sphereTag = mySphereTag;
    }

    public string getSphereTag()
    {
        // this allows a moveStrategy to do:
        // GameObject sphere = GameObject.FindWithTag(hero.getSphereTag());
        // sphere.transform = newPosition;
        return sphereTag;

        // NOTE: maybe we don't need to think about sphere's..
        // maybe since the sphere is a child this hopefully means that moving
        // the hero object should move the child sphere which shows their location
    }


    // thinking the moveStrategy should just be a param here.
    public void move(ref Node path)
    {
        moveStrat.move(ref path, this);
    }

    public void diceRoll()
    {
        diceRollStrat.roll(this);
    }

    //getters and setters for all private attributes. 

    public string getHeroType ()
    {

        return heroType; 

    }
    public void setHeroType(string heroTypeInput)
    {

        heroType = heroTypeInput; 

    }
    public int getStrength()
    {
        return strength; 
    }
    public void setStrength(int strengthToSet)
    {

        strength = strengthToSet; 

    }
    public int getWillpower()
    {
        return willPower; 
    }
    public void setWillpower(int willpowerTortrn)
    {
        willPower = willpowerTortrn; 
    }

    public int getNumHours()
    {
        return numHoursLeft;
    }
    public void setNumHours(int numHrs)
    {
        numHoursLeft = numHrs; 
    }
    public int getRank()
    {
        return rank;
    }
    public void setRank(int rankSet)
    {
        rank = rankSet; 
    }
    public int getNumDie() { 
        return NumDice; 
    }
    public void setNumDie(int numDiceSet)
    {
        NumDice = numDiceSet; 
    }
    public int getOverTimeHours()
    {
        return overTimeHours; 
    }





}
