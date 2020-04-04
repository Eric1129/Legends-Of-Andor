using System.Collections;
using System.Collections.Generic;
using Andor;
using UnityEngine;
using System.Linq;

public class GameState
{
	private Dictionary<string, Player> players;
    private Dictionary<string, Monster> monsters;
    public short legend = 0;

    public Dictionary<string, int> playerLocations;


    public TurnManager turnManager;

    public GameState()
	{
		players = new Dictionary<string, Player>();
        monsters = new Dictionary<string, Monster>();

        playerLocations = new Dictionary<string, int>();

    }
    public void addPlayer(Player p)
	{
        if (!players.ContainsKey(p.getNetworkID()))
        {
            players.Add(p.getNetworkID(), p);
            Debug.Log("Added player " + p);

        }
        else
        {
            Debug.Log("I already have myself/this player");
        }
        displayPlayers();

        if (!Game.started)
        {
            RoomLobbyController.instance.playerListUpdate(getPlayers());
        }

    }
    public void updatePlayer(Player p)
    {
        if (players.ContainsKey(p.getNetworkID()))
        {   
            players[p.getNetworkID()] = p;
            Debug.Log("Player is updated!");
        }
        else
        {
            Debug.Log("Player is not regestered!");
        }

        if (!Game.started)
        {
            RoomLobbyController.instance.playerListUpdate(getPlayers());
        }
    }
    public void processAction(Action a)
    {
        switch (a.getType())
        {
            case Type.Move:
                playerLocations[a.playersInvolved()[0]] = ((Move)(a)).to;
                GameController.instance.movePlayer(a.playersInvolved()[0], ((Move)(a)).to);
                break;

            default:
                Debug.Log("DEFAULT CASE");
                break;
        }
    }

    public bool playerCharacterExists(string tag)
    {
        foreach(Player p in players.Values.ToList())
        {
            if (p.getTag().Equals(tag))
            {
                return true;
            }
        }
        return false;
    }

    public void displayPlayers()
    {
        Debug.Log("Printing Players! Size: " + players.Count);
        Debug.Log("=================================");
        foreach (Player p in players.Values.ToList())
        {
            Debug.Log(p);
        }
        Debug.Log("=================================\n");

    }
    public List<Player> getPlayers()
    {
        return players.Values.ToList();
    }
    public bool hasPlayer(Player player)
    {
        return players.ContainsKey(player.getNetworkID());
    }
}
