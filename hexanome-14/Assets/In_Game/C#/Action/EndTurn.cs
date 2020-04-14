using UnityEngine;
using System.Linq;
using System.Threading;
using System.Collections.Generic;
using System.Collections;

[System.Serializable]
public class EndTurn : Action
{
    private Type type;
    private string[] players;

    public EndTurn(string playerID)
    {
        type = Type.EndTurn;
        players = new string[] { playerID };
    }

    public Type getType()
    {
        return type;
    }

    public string[] playersInvolved()
    {
        return players;
    }


    public bool isLegal(GameState gs)
    {
        return players[0].Equals(gs.turnManager.currentPlayerTurn());
    }
    public void execute(GameState gs)
    {
        gs.turnManager.endTurn();

        if (gs.turnManager.roundDone())
        {
            gs.turnManager.reset();
            foreach (Andor.Player player in gs.getPlayers())
            {
                player.getHero().setHour(0);

                GameController.instance.setTime(player.getNetworkID(), player.getHero().getHour());
            }

            moveMonstersAtSunrise(gs);
            //moveGors(gs);
            //moveSkrals(gs);
            //// Move monsters
            //foreach(Monster monster in gs.getMonsters())
            //{
            //    monster.move();
            //}
        }
        else
        {
            Debug.Log("Turn Ended... " + gs.turnManager.currentPlayerTurn() + " is now up!");
        }
    }
    //public void moveGors(GameState gs)
    //{
    //    var gorList = gs.getGors().ToList();
    //    foreach (KeyValuePair<Gor, int> g in gorList.OrderBy(key => key.Value))
    //    {
    //        //if the monster is not in the castle, we move it towards the castle
    //        if (g.Key.getLocation() != 0)
    //        {
    //            moveMonster(g.Key, gs);
    //            //Thread.Sleep(500);
    //        }
    //    }
    //}

    //public void moveSkrals(GameState gs)
    //{
    //    var skralList = gs.getSkrals().ToList();
    //    foreach (KeyValuePair<Skral, int> s in skralList.OrderBy(key => key.Value))
    //    {
    //        //if the monster is not in the castle, we move it towards the castle
    //        if (s.Key.getLocation() != 0)
    //        {
    //            moveMonster(s.Key, gs);
    //            //Thread.Sleep(500);
    //        }
    //    }
    //}
    public void moveMonstersAtSunrise(GameState gs)
    {
        var gorList = gs.getGors().ToList();
        foreach (KeyValuePair<Gor, int> g in gorList.OrderBy(key => key.Value))
        {
            if (gs.outcome == "lost")
            {
                break;
            }
            //if the monster is not in the castle, we move it towards the castle
            if (g.Key.getLocation() != 0 && gs.outcome != "lost")
            {
                moveMonster(g.Key, gs);
            }
        }
        var skralList = gs.getSkrals().ToList();
        foreach (KeyValuePair<Skral, int> s in skralList.OrderBy(key => key.Value))
        {
            if (gs.outcome == "lost")
            {
                break;
            }
            //if the monster is not in the castle, we move it towards the castle
            if (s.Key.getLocation() != 0)
            {
                moveMonster(s.Key, gs);
                //Thread.Sleep(500);
            }
        }

        if (gs.outcome == "lost")
        {
            Debug.Log("YOU LOST THE GAME!");
            //Trigger Loss Scenario but need to add in shield check with farmers
        }

    }


    public bool moveMonster(Monster m, GameState gs)
    {
        int currloc = m.getLocation();
        int nextloc = m.getLocationNode().toCastleNode().getIndex();

        if (nextloc == 0)
        {
            m.move();
            Debug.Log("Monster has entered the castle!" + " " + gs.maxMonstersAllowedInCastle);
            Debug.Log(gs.monstersInCastle);
            gs.monstersInCastle += 1;
            if (gs.monstersInCastle == gs.maxMonstersAllowedInCastle)
            {
                Debug.Log("YOU LOST THE GAME");
                gs.outcome = "lost";
                return true;
            }
        }
        else
        {
            //check if there is already a monster on that spot
            foreach (Monster checkMonster in gs.getMonsters())
            {
                if (checkMonster.getLocation() == nextloc)
                {
                    Node nextNode = m.getLocationNode().toCastleNode();
                    m.setLocationNode(nextNode);
                    moveMonster(m, gs);
                    break;
                }
                else
                //if there is no monster on the next spot, we move the monster there
                {
                    m.move();
                    break;
                }
            }
        }

        return false;
    }

}
