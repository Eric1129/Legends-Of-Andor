using System.Collections.Generic;
using Andor;
using Photon.Pun;
using UnityEngine;



public static class Game
{

    public static GameState gameState;
    public static Andor.Player myPlayer;
    private static NetworkHandler nh;
    private static GameObject go;
    private static Photon.Pun.PhotonView PV;
    public static bool started = false;
    public static System.Random RANDOM = new System.Random();


        
    public static void initGame(Andor.Player player)
    {

        myPlayer = player;
        gameState = new GameState();

        ExitGames.Client.Photon.PhotonPeer.RegisterType(typeof(Player), 1, NetworkHandler.SerializeThis, NetworkHandler.Deserialize);
        ExitGames.Client.Photon.PhotonPeer.RegisterType(typeof(List<Player>), 2, NetworkHandler.SerializeThis, NetworkHandler.Deserialize);
        ExitGames.Client.Photon.PhotonPeer.RegisterType(typeof(Move), 3, NetworkHandler.SerializeThis, NetworkHandler.Deserialize);

        // MUST HAVE PV
        gameState.addPlayer(myPlayer);
        Game.addPlayer(myPlayer);

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
    public static void updateGameState(GameState gs)
    {
        gameState = gs;
    }

}
