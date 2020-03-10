using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dwarf : Hero
{
   
    //constructor for Dwarf
    public Dwarf() 
    {
        //getStrength();  //should be initialized to 1
        //getWillpower(); //should be initialized to 7 
        setHeroType("Dwarf");
        setRank(7);     //day over when this reaches 0 
        setNumDie(1); //start off die intitialization
        setNumHours(7); 
        

    }

    public void diceRollStrategy(int NumDieCurrently)
    {



    }




    public void Canmove()
    {
        if(getNumHours() >0 || (getWillpower() > 2 && getOverTimeHours() > 0))
        {
            if (getNumHours() < 0)
            {
                

            
            } 


        }
        
        


    }





}
