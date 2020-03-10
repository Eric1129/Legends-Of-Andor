using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

class DiceRollStrategy  
{
    Hero hero;
    int maxDieBattleValue;

    public DiceRollStrategy(Hero h)
    {
        this.hero = h;    

    }


    public void setDieNumberForCharacters()

    {
        if (hero.getHeroType().Equals("Dwarf"))
            //set the number of dice for each character based on their board attributes 
        {
            int willpowerPts = hero.getWillpower();
            if(willpowerPts <= 6)
            {
                hero.setNumDie(1);
            }else if(willpowerPts <= 13)
            {
                hero.setNumDie(2);
            }
            else
            {
                hero.setNumDie(3);
            }

        }

        if (hero.getHeroType().Equals("Archer"))
        {

            int willpowerPts = hero.getWillpower();
            if (willpowerPts <= 6)
            {
                hero.setNumDie(3);
            }
            else if (willpowerPts <= 13)
            {
                hero.setNumDie(4);
            }
            else
            {
                hero.setNumDie(5);
            }

        }

        if (hero.getHeroType().Equals("Wizard"))
        {
            hero.setNumDie(1);

        }

        if (hero.getHeroType().Equals("Warrior"))
        {

            int willpowerPts = hero.getWillpower();
            if (willpowerPts <= 6)
            {
                hero.setNumDie(2);
            }
            else if (willpowerPts <= 13)
            {
                hero.setNumDie(3);
            }
            else
            {
                hero.setNumDie(4);
            }

        }

    }


    public void rollDie()
    {
        //To generate random number for die rolls
        //OVERWRITES Unity.Random
        System.Random rnd = new System.Random();
        //get number of dice for hero
        int numDieToRoll = hero.getNumDie();


        if (hero.getHeroType().Equals("Archer"))
        {

            for(int i = 0; i < numDieToRoll; i++)
            {
                int rollValue = rnd.Next(1, 7); 

                //DISPLAY TO USER MESSAGE ASKING IF THEY WANT TO STOP OR KEEP GOING SERVER ??
                //system.out.println("Would you like to keep rolling your dice or stop at your current value?"
                //if they stop then assign their dice battle value to be equal to this roll, else continue in the loop 

            }
        }


        if (hero.getHeroType().Equals("Wizard"))
        {
            for(int i = 0; i < numDieToRoll; i++)
            {
                //Generate random die value 
                int rollValue = rnd.Next(1, 7);
                if (rollValue == 1)
                {
                    //System.out.println("Special power enacted, turning 1 into a 6")
                    rollValue = 6; 
                }
                if (rollValue == 2)
                {
                    //System.out.println("Special power enacted, turning 2 into a 5")
                    rollValue = 5;

                }
                if (rollValue == 3)
                {
                    //System.out.println("Special power enacted, turning 3 into a 4")
                    rollValue = 4;
                }

            }
            }

        int[] dieValues = new int[numDieToRoll]; 

        for(int i = 0; i < numDieToRoll; i++)
        {
            int rollValue = rnd.Next(1, 7);
            dieValues[i] = rollValue; 

        }

        //get the largest value from the array of rolled values
        //maxDieBattleValue now needs to be added to will power points for combined battle score 
        maxDieBattleValue = dieValues.Max();
        
    }


    public void getBattleScore()
    {
        //generate the battleScore of the hero with their dice roll value and their willpower value 
        int battleScore = hero.getWillpower() + maxDieBattleValue;  

    }




    void fight(Fightable obj);
}
