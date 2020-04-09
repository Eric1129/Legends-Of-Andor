
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
}
