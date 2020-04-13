using System;
[System.Serializable]
public class InitiateTrade: Action
{
    private Type type;
    private string[] players;

    private string[] tradeType;

    public InitiateTrade(string[] players, string[] tradeType)
    {
        type = Type.InitiateTrade;
        this.players = players;
        this.tradeType = tradeType;


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
        int location0 = 0;
        Game.gameState.playerLocations.TryGetValue(players[0], out location0);
        int location1 = -1;
        Game.gameState.playerLocations.TryGetValue(players[1], out location1);
        return location0 == location1;
    }

    public void execute(GameState gs)
    {
        GameController.instance.sendTradeRequest(tradeType, players[0], players[1]);
    }
}
