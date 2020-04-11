using UnityEngine;

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


            // Move monsters
            foreach(Monster monster in gs.getMonsters())
            {
                monster.move();
            }
        }
        else
        {
            Debug.Log("Turn Ended... " + gs.turnManager.currentPlayerTurn() + " is now up!");
        }
    }
}
