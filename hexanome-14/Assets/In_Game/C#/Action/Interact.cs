
using UnityEngine;

[System.Serializable]
public class Interact : Action
{
    private Type type;
    private string[] players;
    private int NodeID;     // -1 means located on the player
    private int interactableID;

    public Interact(string playerID, int interactableID, int location)
    {
        type = Type.PassTurn;
        players = new string[] { playerID };
        this.interactableID = interactableID;
        this.NodeID = location;
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
        Interactable i;

        if(NodeID == -1)    // If on player
        {
            i = gs.getPlayerInteractable(players[0], interactableID);
        }
        else
        {
            i = gs.getNodeInteractable(NodeID, interactableID);

        }

        i.interact(gs.getPlayer(players[0]));
    }
}
