using System.Collections;
using System.Collections.Generic;
using Andor;
using Photon.Pun;
using UnityEngine;



public static class Game
{

    public static GameState gameState;
    public static Andor.Player myPlayer;//applies to my player only
    private static NetworkHandler nh;
    private static GameObject go;
    private static Photon.Pun.PhotonView PV;
    public static bool started = false;
    public static System.Random RANDOM = new System.Random();
    public static bool loadedFromFile = false;
    public static int[] event_cards = { 2, 11, 13, 14, 17, 24, 28, 31, 32, 1 };
    private static string[] fogTokens = {"event", "strength", "willpower3", "willpower2", "brew",
            "wineskin", "gor", "event", "gor", "gold1", "gold1", "gold1", "event", "event", "event",};



    public static void initGame(Andor.Player player, bool addPlayer = true)
    {

        myPlayer = player;
        gameState = new GameState();
        

        ExitGames.Client.Photon.PhotonPeer.RegisterType(typeof(Player), 1, NetworkHandler.SerializeThis, NetworkHandler.Deserialize);
        ExitGames.Client.Photon.PhotonPeer.RegisterType(typeof(List<Player>), 2, NetworkHandler.SerializeThis, NetworkHandler.Deserialize);
        ExitGames.Client.Photon.PhotonPeer.RegisterType(typeof(List<string>), 3, NetworkHandler.SerializeThis, NetworkHandler.Deserialize);
        ExitGames.Client.Photon.PhotonPeer.RegisterType(typeof(Move), 4, NetworkHandler.SerializeThis, NetworkHandler.Deserialize);
        ExitGames.Client.Photon.PhotonPeer.RegisterType(typeof(PassTurn), 5, NetworkHandler.SerializeThis, NetworkHandler.Deserialize);
        ExitGames.Client.Photon.PhotonPeer.RegisterType(typeof(EndTurn), 6, NetworkHandler.SerializeThis, NetworkHandler.Deserialize);

        ExitGames.Client.Photon.PhotonPeer.RegisterType(typeof(InitiateTrade), 7, NetworkHandler.SerializeThis, NetworkHandler.Deserialize);
        ExitGames.Client.Photon.PhotonPeer.RegisterType(typeof(RespondTrade), 8, NetworkHandler.SerializeThis, NetworkHandler.Deserialize);
        //If you want to send something through the network, you need to execute this command and create the corresponding class
        ExitGames.Client.Photon.PhotonPeer.RegisterType(typeof(MovePrinceThorald), 9, NetworkHandler.SerializeThis, NetworkHandler.Deserialize);
        ExitGames.Client.Photon.PhotonPeer.RegisterType(typeof(BuyFromMerchant), 10, NetworkHandler.SerializeThis, NetworkHandler.Deserialize);
        ExitGames.Client.Photon.PhotonPeer.RegisterType(typeof(EmptyWell), 11, NetworkHandler.SerializeThis, NetworkHandler.Deserialize);
        ExitGames.Client.Photon.PhotonPeer.RegisterType(typeof(BuyBrew), 12, NetworkHandler.SerializeThis, NetworkHandler.Deserialize);
        ExitGames.Client.Photon.PhotonPeer.RegisterType(typeof(SendChat), 13, NetworkHandler.SerializeThis, NetworkHandler.Deserialize);
        ExitGames.Client.Photon.PhotonPeer.RegisterType(typeof(UseTelescope), 14, NetworkHandler.SerializeThis, NetworkHandler.Deserialize);

        ExitGames.Client.Photon.PhotonPeer.RegisterType(typeof(Interact), 15, NetworkHandler.SerializeThis, NetworkHandler.Deserialize);
        ExitGames.Client.Photon.PhotonPeer.RegisterType(typeof(UseWineskin), 16, NetworkHandler.SerializeThis, NetworkHandler.Deserialize);

        ExitGames.Client.Photon.PhotonPeer.RegisterType(typeof(InviteFighter), 17, NetworkHandler.SerializeThis, NetworkHandler.Deserialize);
        ExitGames.Client.Photon.PhotonPeer.RegisterType(typeof(RespondFight), 18, NetworkHandler.SerializeThis, NetworkHandler.Deserialize);

        // ExitGames.Client.Photon.PhotonPeer.RegisterType(typeof(MovePrinceThorald), 9, NetworkHandler.SerializeThis, NetworkHandler.Deserialize);

        // MUST HAVE PV
        gameState.addPlayer(myPlayer);
        Game.addPlayer(myPlayer);

        int[] randomEventOrder = event_cards;
        randomEventOrder.Shuffle();
        //event_cards2 = randomEventOrder;
        Debug.Log("STARTING TO SET EVENT CARD ORDER");
        Game.setEventCardOrder(randomEventOrder);
        Debug.Log("FINISHING TO SET EVENT CARD ORDER");
        string[] randomFogTokenOrder = fogTokens;
        randomFogTokenOrder.Shuffle();
        //fogTokens2 = randomFogTokenOrder;
        Debug.Log("STARTING TO SET FOG ORDER");
        Game.setFogTokenOrder(randomFogTokenOrder);
        Debug.Log("FINISHING TO SET FOG ORDER");


        Debug.Log("Initialized Game!");
    }

    public static void createPV()
    {
        go = PhotonNetwork.Instantiate("NetworkHandler", new Vector3(), new Quaternion());
        PV = go.GetComponent<PhotonView>();
    }
    public static void destroyPV()
    {
        PhotonNetwork.Destroy(go);
        PV = null;
    }

    #region RPC calls
    // Should be called once before
    public static void PREGAMEupdateGameState(GameState gs)
    {
        if (PV != null && PV.IsMine)
        {
            Debug.Log(Game.myPlayer.getNetworkID() + " ~ Updating GameState...");

            PV.RPC("PREGAMEupdateGameState", RpcTarget.AllBuffered, SavedGameController.serializeGameState(gs));
        }
        else
        {
            Debug.Log(Game.myPlayer.getNetworkID() + " ~ Could not access PhotoView");
        }
    }

    public static void PREGAMEstartGame()
    {
        if (PV != null && PV.IsMine)
        {
            Debug.Log(Game.myPlayer.getNetworkID() + " ~ Updating GameState...");

            PV.RPC("PREGAMEstartGame", RpcTarget.AllBuffered);
        }
        else
        {
            Debug.Log(Game.myPlayer.getNetworkID() + " ~ Could not access PhotoView");
        }
    }
    public static void addPlayer(Andor.Player p)
    {

        if (PV != null && PV.IsMine)
        {
            Debug.Log(Game.myPlayer.getNetworkID() + " ~ Adding Player...");

            PV.RPC("HOSTaddPlayer", RpcTarget.MasterClient, p);
        }
        else
        {
            Debug.Log(Game.myPlayer.getNetworkID() + " ~ Could not access PhotoView");
        }

    }
    public static void updateDifficulty(string difficulty)
    {
        if (PV != null && PV.IsMine)
        {
            if (PhotonNetwork.IsMasterClient)
            {
                Debug.Log(Game.myPlayer.getNetworkID() + " ~ Updating Legend...");

                PV.RPC("updateDifficulty", RpcTarget.AllBuffered, difficulty);
            }
            else
            {
                Debug.Log(Game.myPlayer.getNetworkID() + " ~ This is a function only the Host can call! You are not master client!");
            }
            
        }
        else
        {
            Debug.Log(Game.myPlayer.getNetworkID() + " ~ Could not access PhotoView");
        }
    }

    public static void updatePlayer(Andor.Player p)
    {
        if (PV != null && PV.IsMine)
        {
            Debug.Log(Game.myPlayer.getNetworkID() + " ~ Updating Player...");

            PV.RPC("updatePlayer", RpcTarget.All, p);
        }
        else
        {
            Debug.Log(Game.myPlayer.getNetworkID() + " ~ Could not access PhotoView");
        }

    }

    public static void sendAction(Action a)
    {
        //action is pass, move, end day
        if (PV != null && PV.IsMine)
        {
            Debug.Log(Game.myPlayer.getNetworkID() + " ~ Sending Action...");

            PV.RPC("sendAction", RpcTarget.All, a);
        }
        else
        {
            Debug.Log(Game.myPlayer.getNetworkID() + " ~ Could not access PhotoView");

        }

    }
    // ONLY CALLED IF HOST
    // Tell players that there is a new player/player change
    public static void updateClients()
    {
        if (PV != null && PV.IsMine)
        {
            Debug.Log(Game.myPlayer.getNetworkID() + " (HOST) ~ Updating clients on Players...");
            gameState.displayPlayers();

            List<Player> players = gameState.getPlayers();
            PV.RPC("updatePlayerList", RpcTarget.Others, players);
        }
        else
        {
            Debug.Log(Game.myPlayer.getNetworkID() + " ~ Could not access PhotoView");

        }
    }
    // ONLY CALLED IF HOST
    // Sets the turn manager for all clients
    public static void setTurnManager(List<Andor.Player> players)
    {
        
        if (PV != null && PV.IsMine)
        {
            Debug.Log(Game.myPlayer.getNetworkID() + " (HOST) ~ Updating TurnManager for clients...");

            PV.RPC("setTurnManager", RpcTarget.All, getPlayerNames(players));
        }
        else
        {
            Debug.Log(Game.myPlayer.getNetworkID() + " ~ Could not access PhotoView");

        }
    }

    //eventCardStuff
    public static void setEventCardOrder(int[] event_cards)
    {

        if (PV != null && PV.IsMine)
        {
            Debug.Log(Game.myPlayer.getNetworkID() + " (HOST) ~ Updating EventCards for clients...");

            PV.RPC("setEventCardOrder", RpcTarget.All, event_cards);
        }

        else
        {
            Debug.Log(Game.myPlayer.getNetworkID() + " ~ Could not access PhotoView");

        }

    }

    //eventCardStuff
    public static void setFogTokenOrder(string[] fog_cards)
    {

        if (PV != null && PV.IsMine)
        {
            Debug.Log(Game.myPlayer.getNetworkID() + " (HOST) ~ Updating EventCards for clients...");

            PV.RPC("setFogTokenOrder", RpcTarget.All, fog_cards);
        }
        else
        {
            Debug.Log(Game.myPlayer.getNetworkID() + " ~ Could not access PhotoView");

        }

    }

    #endregion

    public static GameState getGame()
    {
        if (gameState == null)
        {
            throw new System.Exception("Did not init gameState");
        }
        return gameState;
    }

    public static void Shuffle<T>(this IList<T> list)
    {
        int n = list.Count;

        for (int i = list.Count - 1; i > 1; i--)
        {
            int rnd = Game.RANDOM.Next(i + 1);

            T value = list[rnd];
            list[rnd] = list[i];
            list[i] = value;
        }
    }

    public static List<string> getPlayerNames(List<Andor.Player> players)
    {
        List<string> playerNames = new List<string>();
        foreach (Andor.Player player in players)
        {
            playerNames.Add(player.getNetworkID());
        }
        return playerNames;
    }


    public static IEnumerator sleep(float sec)
    {
        yield return new WaitForSeconds(sec);
    }
}
