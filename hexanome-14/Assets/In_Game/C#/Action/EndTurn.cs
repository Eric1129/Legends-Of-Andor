using UnityEngine;
using System.Linq;
using System.Threading;
using System.Collections.Generic;
using System.Collections;

[System.Serializable]
public class EndTurn :Action
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
        gs.getPlayer(players[0]).getHero().setHour(0);
        GameController.instance.setTime(players[0], gs.getPlayer(players[0]).getHero().getHour());

        if (gs.turnManager.roundDone())
        {
            gs.turnManager.reset();
            gs.day += 1;
            GameController.instance.updateDayCount(Game.gameState.day);

           
            foreach (Andor.Player player in gs.getPlayers())
            {
                player.getHero().setHour(0);

                GameController.instance.setTime(player.getNetworkID(), player.getHero().getHour());
            }

            sunriseBoxSequence(gs);
            
           // moveMonstersAtSunrise(gs);
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

    public void sunriseBoxSequence(GameState gs)
    {
        //moving all monsters at sunrise
        moveMonstersAtSunrise(gs);

        //refreshing all wells
        foreach (Well w in gs.getWells().Keys)
        {
            //check if there is a Hero on the same spot first
            foreach(Andor.Player p in gs.getPlayers())
            {
                int loc = gs.getPlayerLocations()[p.getNetworkID()];
                if(w.getLocation() != loc)
                {
                    w.refreshWell();
                }
            }
           
        }

        gs.uncoverEventCard();
        gs.TIME_overtime = 8;
        gs.TIME_endTime = 10;
        gs.TIME_overtimeCost = 2;
        //advance narrator 
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
        
        //foreach (KeyValuePair<Gor, int> g in gorList.OrderBy(key => key.Value))
        //{
        //    Debug.Log(g.Value);
        //}
     
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
                gs.updateGorLocations();

            }
        }
        //gs.updateGorLocations();
        //var gorList2 = gs.getGors().ToList();
        //foreach (KeyValuePair<Gor, int> g in gorList2.OrderBy(key => key.Value))
        //{
        //    Debug.Log("Updated Value: " + g.Value);
        //}
        //Debug.Log(gorList2);


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
                gs.updateSkralLocations();
                //Thread.Sleep(500);
            }
        }

        if (gs.outcome == "lost")
        {
            Debug.Log("YOU LOST THE GAME!");
            // OutcomeScroll.instance.UpdateFeedback("You Fuckin LOST you LOSER!");
            //call the lose outcome scroll
            GameController.instance.loseScenario();
            //scroll.GetComponent<Renderer>().enabled = true;
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
            GameController.instance.updateShieldCount(Game.gameState.maxMonstersAllowedInCastle - Game.gameState.monstersInCastle);
            moveMonsterToShield(m, gs);
            if (gs.monstersInCastle > gs.maxMonstersAllowedInCastle)
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
    

    public void moveMonsterToShield(Monster m, GameState gs)
    {
        int playerCount = gs.getPlayers().Count;
        int monstersAllowed = gs.maxMonstersAllowedInCastle;
        int monstersInside = gs.monstersInCastle;

      
            if (monstersInside == 1)
            {
                //check if farmer is on shield
                //move monster to shield 1
            }
            else if (monstersInside == 2)
            {
                //check if farmer is on shield
                //move monster to shield 2
            }
            else if (monstersInside == 3)
            {
                //check if farmer is on shield
                //move monster to shield 3
            }
            else if (monstersInside == 4)
            {
            //check if farmer is on shield
            //move monster to shield 4
            }
             else if (monstersInside == 5)
            {
            //check if farmer is on shield
            //move monster to shield 5
            }
            else if (monstersInside == 6)
            {
            //check if farmer is on shield
            //move monster to shield 6
            }

        }
}
