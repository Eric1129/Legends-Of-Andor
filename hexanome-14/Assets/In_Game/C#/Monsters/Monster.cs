using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : Fightable, Movable
{
    private MoveStrategy moveStrat = null;
    private DiceRollStrategy diceRollStrat = null;


    public void move(ref Node path)
    {
        moveStrat.move(ref path, this);
    }

    public void diceRoll()
    {
        diceRollStrat.roll(this);
    }

}
