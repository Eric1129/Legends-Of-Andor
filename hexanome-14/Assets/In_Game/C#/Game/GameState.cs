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
    private List<Monster> monsters;
    public string difficulty = "-1";
    public Dictionary<string, int> playerLocations;
    private Dictionary<Skral, int> skrals;
    private Dictionary<Gor, int> gors;
    public TurnManager turnManager;

    public Dictionary<string, List<Interactable>> playerInteractables;
    public Graph positionGraph;

    private List<Farmer> farmers;

    public string outcome;
    public int maxMonstersAllowedInCastle;
    public int monstersInCastle;
    private Dictionary<Well, int> wells;
    private Dictionary<int, Merchant> merchants;
    private Dictionary<FogToken, int> fogTokens;
    // private Dictionary<PrinceThorald, int> princeThor;
    private List<PrinceThorald> princeThor;
    private List<MedicinalHerb> medicinalHerb;
    public int[] event_cards;
    public string[] fogtoken_order;
    public int day;
    
    public int TIME_overtime = 8;
    public int TIME_endTime = 10;
    public int TIME_overtimeCost = 2;
    public bool skralTowerDefeated;
    //public bool eventcard19;
    public bool EVENTCARD_treeOfSongBonusIsActive = false;
    public int brewCost;
    public int witchLocation;
    public bool witchFound;
    public Dictionary<string, List<Article>> equipmentBoard;

    public GameState()
	{
        players = new Dictionary<string, Player>();
        playerInteractables = new Dictionary<string, List<Interactable>>();
        positionGraph = new Graph();

        monsters = new List<Monster>();
        playerLocations = new Dictionary<string, int>();
        gors = new Dictionary<Gor, int>();
        skrals = new Dictionary<Skral, int>();
        outcome = "undetermined";
        monstersInCastle = 0;
        maxMonstersAllowedInCastle = 0;
        wells = new Dictionary<Well, int>();
        fogTokens = new Dictionary<FogToken, int>();
        princeThor = new List<PrinceThorald>();
        day = 1;
        farmers = new List<Farmer>();
        merchants = new Dictionary<int, Merchant>();
        equipmentBoard = new Dictionary<string, List<Article>>();
        skralTowerDefeated = false;
        medicinalHerb = new List<MedicinalHerb>();
        //eventcard19 = false;
        EVENTCARD_treeOfSongBonusIsActive = false;
        brewCost = 2;
        witchLocation = -1;
        witchFound = false;
    }

    public void addPlayer(Player p)
	{
        if (!players.ContainsKey(p.getNetworkID()))
        {
            players.Add(p.getNetworkID(), p);
            playerInteractables.Add(p.getNetworkID(), new List<Interactable>());
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

    public Dictionary<string, int> getPlayerLocations()
    {
        return playerLocations;
    }

    public List<Monster> getMonsters()
    {
        return monsters;
    }
    public void addMonster(Monster m)
    {
        monsters.Add(m);
    }
    ////////////////////////////////////////////////////////////////////////////
    public Dictionary<Skral, int> getSkrals()
    {
        return skrals;
    }
    public void addSkral(Skral s)
    {
        skrals.Add(s, s.getLocation());
    }
    //////////////////////////////////gors//////////////////////////////////
    public Dictionary<Gor, int> getGors()
    {
        return gors;
    }
    public void addGor(Gor g)
    {
        gors.Add(g, g.getLocation());
    }

    //////////////////////////////////wells//////////////////////////////////

    public void addMerchant(int location, Merchant m)
    {
        merchants.Add(location, m);
    }

    public Merchant getMerchant(int location)
    {
        return merchants[location];
    }

    public Dictionary<Well, int> getWells()
    {
        return wells;
    }

    public void setWells(Dictionary<Well, int> updatedWells)
    {
        this.wells = updatedWells;
    }

    public void addWell(Well w)
    {
        wells.Add(w, w.getLocation());
    }

    //////////////////////////////////fog//////////////////////////////////
    public Dictionary<FogToken, int> getFogTokens()
    {
        return fogTokens;
    }
    public void addFogToken(FogToken f)
    {
        fogTokens.Add(f, f.getLocation());
    }


    public List<Farmer> getFarmers()
    {
        return farmers;
    }
    public void addFarmer(Farmer f)
    {
        farmers.Add(f);
    }



    public List<PrinceThorald> getPrinceThorald()
    {
        return princeThor;
    }
    public void addPrince(PrinceThorald prince)
    {
        princeThor.Add(prince);
    }

    public MedicinalHerb getMedicinalHerb()
    {
        return medicinalHerb.ToArray().ElementAt(0);
    }
    public void addMedicinalHerb(MedicinalHerb m)
    {
        medicinalHerb.Add(m);
    }
    public void updateGorLocations()
    {
        Dictionary<Gor,int> updatedGors = new Dictionary<Gor, int>();
        var gorList = gors.Keys;
        foreach (Gor g in gorList)
        {
            int x = g.getLocation();
            updatedGors.Add(g, x);
        }
        gors = updatedGors;
    }
    public void updateSkralLocations()
    {
        Dictionary<Skral, int> updatedSkrals = new Dictionary<Skral, int>();
        var skralList = skrals.Keys;
        foreach (Skral s in skralList)
        {
            int x = s.getLocation();
            updatedSkrals.Add(s, x);
        }
        skrals = updatedSkrals;

    }

    public void processAction(Action a)
    {
        if (a.isLegal(this)){
            a.execute(this);
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
    public Player getPlayer(string playerID)
    {
        return players[playerID];
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


    public Interactable removePlayerInteractable(string playerID, Interactable interactable)
    {
        playerInteractables[playerID].Remove(interactable);

        // Update interactable IDs
        for(int i = 0; i< playerInteractables[playerID].Count; i++)
        {
            playerInteractables[playerID][i].setInteractableID(i);
        }

        return interactable;
    }
    public void addPlayerInteractable(string playerID, Interactable interactable)
    {
        interactable.setInteractableID(playerInteractables[playerID].Count);
        playerInteractables[playerID].Add(interactable);
    }
    public List<Interactable> getInteractables(string playerID)
    {
        return playerInteractables[playerID];
    }

    public Interactable getPlayerInteractable(string playerID, int interactableID)
    {
        return playerInteractables[playerID][interactableID];
    }
    public Interactable getNodeInteractable(int NodeID, int interactableID)
    {
        return positionGraph.getNode(NodeID).getInteractables()[interactableID];
    }

    //public void setEventCardOrder(int[] event_card)
    //{
    //    event_cards = event_card;
    //}
    //public void setFogTokenOrder(string[] fog)
    //{
    //    fogtoken_order = fog;
    //}

    //public void set(int[] eventCards)
    //{
    //    event_cards = eventCards;
    //}

    public Dictionary<string, Player> getPlayerDict()
    {
        return players;
    }
    public GameState DeepCopy()
    {
        return SavedGameController.deserializeGameState(SavedGameController.serializeGameState(this));
    }

    public void uncoverEventCard()
    {
        int num = event_cards[0];
        eventCards.execute(num);
        //event_cards = RemoveAt(event_cards,0);
        int[] e = new int[event_cards.Length - 1];
        Array.Copy(event_cards, 1, e, 0, event_cards.Length - 1);
        event_cards = e;
    }

    public Dictionary<int, Merchant> getMerchants()
    {
        return this.merchants;
    }
    public Article removeFromEquimentBoard(string articleName)
    {
        Debug.Log("removing article: "+ articleName);
        //remove the last item in the list
        int numArticles = equipmentBoard[articleName].Count;
        Debug.Log("removing article2");
        Article removedArticle = equipmentBoard[articleName][numArticles - 1];
        Debug.Log("removing article3");
        equipmentBoard[articleName].Remove(equipmentBoard[articleName][numArticles - 1]);
        Debug.Log("removing article4");
        return removedArticle;
    }

    public List<Article> getArticlesOfType(string key)
    {
        return equipmentBoard[key];
    }

    public Dictionary<string, List<Article>> getEquipmentBoard()
    {
        return equipmentBoard;
    }

}
