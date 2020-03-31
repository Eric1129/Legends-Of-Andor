using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rubble : Fightable
{
    private DiceRollStrategy diceRollStrat;

    public void diceRoll()
    {
        diceRollStrat.roll(this);
    }

}
