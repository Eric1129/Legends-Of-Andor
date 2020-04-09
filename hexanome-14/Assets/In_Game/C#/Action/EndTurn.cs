
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
}
