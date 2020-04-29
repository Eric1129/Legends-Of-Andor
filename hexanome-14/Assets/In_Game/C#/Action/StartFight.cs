using System;
using UnityEngine;

[System.Serializable]
public class StartFight : Action
{
    string[] players;
    Type type;
    int fightType;
    

    public StartFight(string[] players, int fightType)
    {
        type = Type.StartFight;
        this.players = players;
        this.fightType = fightType;
        
        
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
       if(fightType == 0)
        {
            GameController.instance.fsc.startSoloFight();
            //GameController.instance.
        }
        else
        {
            Monster monster = Game.gameState.getMonsters()[0];
            int myLocation = Game.gameState.getPlayerLocations()[players[0]];
            foreach (Monster m in Game.gameState.getMonsters())
            {
                int monsterLoc = m.getLocation();

                if (monsterLoc == myLocation)
                {
                    monster = m;
                }
            }
            Fight fight = new Fight(players, monster);
            GameController.instance.fsc.startCollabFight(fight);
        }

    }

    public bool isLegal(GameState gs)
    {
        return true;
    }

   
}