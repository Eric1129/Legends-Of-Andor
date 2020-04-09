using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rubble : Fightable
{
    private DiceRollStrategy diceRollStrat = null;

    public void diceRoll()
    {
        diceRollStrat.roll(this);
    }
    
}
