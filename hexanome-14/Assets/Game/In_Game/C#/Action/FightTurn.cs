using System;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class FightTurn : Action
{
    string[] players;
    Type type;
    int fightIndex;
    string currentPlayer;
    int battleValue;
    List<int> monsterDiceRoll;
    
    

    public FightTurn(string[] players, int fightIndex, string currentPlayer, int battleValue)
    {
        
        type = Type.FightTurn;
        this.players = players;
        this.fightIndex = fightIndex;
        this.currentPlayer = currentPlayer;
        this.battleValue = battleValue;

        monsterDiceRoll = GameController.instance.fsc.fight.monster.diceRoll();
        
        
    }

    public string[] playersInvolved()
    {
        return this.players;
    }
    public Type getType()
    {
        return this.type;
    }

    public void execute(GameState gs)
    {


        Debug.Log("FIGHT TURN: " + fightIndex);
        if (fightIndex == 0)
        {
            //Debug.Log("creature turn");
            //GameController.instance.fsc.fight.setBattleValue(battleValue);
            //GameController.instance.fsc.displayBattleValue(0);

            //GameController.instance.fsc.fight.monster.diceRoll(); 
            //GameController.instance.fsc.setRoundWinner();
            GameController.instance.fsc.creatureTurn_collab(battleValue, monsterDiceRoll);
        }
        else
        {
            Debug.Log("sending fight turn");
            GameController.instance.fsc.nextPlayerTurnToRoll(fightIndex, battleValue);
        }
    }

    public bool isLegal(GameState gs)
    {
        return true;
    }

   
}