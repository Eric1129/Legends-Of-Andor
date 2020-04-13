using UnityEngine;

[System.Serializable]
public class Move : Action
{
    public int from;
    public int to;
    private Type type;
    private string[] players;

    public Move(string playerID, int from, int to)
    {
        type = Type.Move;
        this.from = from;
        this.to = to;
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
        gs.playerLocations[players[0]] = to;
    }
}
