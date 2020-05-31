using UnityEngine;

[System.Serializable]
public class PassTurn : Action
{
    private Type type;
    private string[] players;

    public PassTurn(string playerID)
    {
        type = Type.PassTurn;
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
        gs.turnManager.passTurn();

        gs.getPlayer(players[0]).getHero().setHour(1 + gs.getPlayer(players[0]).getHero().getHour());
        GameController.instance.setTime(players[0], gs.getPlayer(players[0]).getHero().getHour());

        Debug.Log("Turn Passed... " + gs.turnManager.currentPlayerTurn() + " is now up!");
    }
}
