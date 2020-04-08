using System.Collections;
using System;
using System.Collections.Generic;
using Andor;
using UnityEngine;
using System.Linq;
using Newtonsoft.Json;

[JsonObject(MemberSerialization.Fields)]
public class GameState
{
    private DateTime dateSaved;

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
    /*public void updateGameState(GameState gs)
    {
        dateSaved = gs.getSaveTime();
        players = gs.getPlayerDict();
        monsters = gs.getMonsterDict();
        legend = gs.legend;
        playerLocations = gs.playerLocations;
        turnManager = gs.turnManager;
    }*/

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
            if(RoomLobbyController.preLoadedGameState == null)
            {
                RoomLobbyController.instance.playerListUpdate(Game.gameState.getPlayers());
            }
            else
            {
                RoomLobbyController.instance.playerListUpdateLOADED(Game.gameState.getPlayers());
            }
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
            if (RoomLobbyController.preLoadedGameState == null)
            {
                RoomLobbyController.instance.playerListUpdate(Game.gameState.getPlayers());
            }
            else
            {
                RoomLobbyController.instance.playerListUpdateLOADED(Game.gameState.getPlayers());
            }
        }
    }
    public void removeAllPlayers()
    {
        players = new Dictionary<string, Player>();
        playerLocations = new Dictionary<string, int>();
    }
    public void processAction(Action a)
    {
        switch (a.getType())
        {
            case Type.Move:
                playerLocations[a.playersInvolved()[0]] = ((Move)(a)).to;
                //GameController.instance.movePlayer(a.playersInvolved()[0], ((Move)(a)).to);
                break;

                // Need to impliment more actions
            default:
                Debug.Log("DEFAULT CASE");
                break;
        }
    }

    public bool playerCharacterExists(string tag)
    {
        foreach(Player p in players.Values.ToList())
        {
            if (p.getHeroType().Equals(tag))
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

    public void setSaveTime(DateTime dateTime)
    {
        dateSaved = dateTime;
    }
    public DateTime getSaveTime()
    {
        return dateSaved;
    }
    public Dictionary<string, Player> getPlayerDict()
    {
        return players;
    }
    public Dictionary<string, Monster> getMonsterDict()
    {
        return monsters;
    }
    public GameState DeepCopy()
    {
        return SavedGameController.deserializeGameState(SavedGameController.serializeGameState(this));
    }
}
