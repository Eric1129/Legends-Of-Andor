using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wizard : Hero
{
    // init base class: Hero
    public Wizard() : base()
    {

        getStrength();  //should be initialized to 1
        getWillpower(); //should be initialized to 7 
        setHeroType("Wizard");
        setRank(34);     //day over when this reaches 0 
        setNumDie(1); //Does not change no matter willpower points for wizards 

    }


}
