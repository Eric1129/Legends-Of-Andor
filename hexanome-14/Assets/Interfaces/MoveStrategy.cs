using System.Collections;
using System.Collections.Generic;
using UnityEngine;

class MoveStrategy
{

    Hero hero;
    public MoveStrategy(Hero h)
    {
        this.hero = h;
    }

    public bool canMove()
    {
        bool canMoveBool = true;

        int numHoursLeft = hero.getNumHours();
        if (numHoursLeft > 0)
        {



        }




    }




    void move(ref Node path, Movable obj);
}
