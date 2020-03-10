using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Archer : Hero
{


    // init base class: Hero
    public Archer() : base()
    { 
            //getStrength();  //should be initialized to 1
            //getWillpower(); //should be initialized to 7 
            setHeroType("Archer");
            setRank(25);     //day over when this reaches 0 
            setNumDie(3); //start off die intitialization


        }







    }

