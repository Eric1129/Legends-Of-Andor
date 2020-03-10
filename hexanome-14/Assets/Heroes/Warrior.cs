using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Warrior : Hero
{
    // init base class: Hero
    public Warrior() : base()
    {
        //NOTE Warriors get 5 willpower points instead of 3 when they empty a well !!
        //MUST BE REFLECTED IN CODE !!! 

        //getStrength();  //should be initialized to 1
        //getWillpower(); //should be initialized to 7 
        setHeroType("Warrior");
        setRank(14);     //day over when this reaches 0 
        setNumDie(2); //start off die intitialization

    }
}
