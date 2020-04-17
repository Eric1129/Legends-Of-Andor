using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using Photon.Pun;
using UnityEngine;
using Newtonsoft.Json;


public class NetworkHandler : MonoBehaviour
{
    // Should be called only when initializing the game
    [PunRPC]
    public void PREGAMEupdateGameState(string serializedGameStateJSON)
    {
        RoomLobbyController.preLoadedGameState = JsonConvert.DeserializeObject<GameState>(serializedGameStateJSON);
        RoomLobbyController.instance.legendLabel.text = "Difficulty " + RoomLobbyController.preLoadedGameState.difficulty;
        Game.loadedFromFile = true;
        Debug.Log(Game.myPlayer.getNetworkID() + " ~ Updated GameState");
    }
    [PunRPC]
    public void PREGAMEstartGame()
    {
        Game.started = true;

        GameState originalGame = RoomLobbyController.preLoadedGameState.DeepCopy();
        List<Andor.Player> originalPlayers = originalGame.getPlayers();

        RoomLobbyController.preLoadedGameState.removeAllPlayers();

        Dictionary<string, Andor.Player> originalPlayerDict = originalGame.getPlayerDict();
        Dictionary<string, int> originalLocations = originalGame.playerLocations;


        foreach (Andor.Player p in Game.gameState.getPlayers())
        {
            string playerChoosen = originalPlayers[p.getHero().getGold()-1].getNetworkID();

            originalPlayerDict[playerChoosen].setNetworkID(p.getNetworkID());
            RoomLobbyController.preLoadedGameState.addPlayer(originalPlayerDict[playerChoosen]);
            RoomLobbyController.preLoadedGameState.playerLocations.Add(p.getNetworkID(), originalLocations[playerChoosen]);

            if (p.getNetworkID().Equals(Game.myPlayer.getNetworkID())){
                Game.myPlayer = originalPlayerDict[playerChoosen];
            }
        }

        Game.gameState = RoomLobbyController.preLoadedGameState.DeepCopy();


    }

    [PunRPC]
    public void HOSTaddPlayer(Andor.Player p) // Only host should get this called
    {
        Game.getGame().addPlayer(p);
        Game.updateClients();
        Debug.Log(Game.myPlayer.getNetworkID() + " ~ Added Player");
    }

    [PunRPC]
    public void updateDifficulty(string difficulty)
    {
        Game.gameState.difficulty = difficulty;
        if (!Game.started)
        {
            RoomLobbyController.instance.legendLabel.text = "Difficulty " + difficulty;
        }
    }

    [PunRPC]
    public void setTurnManager(List<string> order)
    {
        Game.gameState.turnManager = new TurnManager(order.ToArray());
    }

    [PunRPC]
    public void updatePlayerList(List<Andor.Player> players)
    {
        foreach (Andor.Player p in players)
        {
            if (!Game.getGame().hasPlayer(p))
            {
                Game.getGame().addPlayer(p);
                Debug.Log(Game.myPlayer.getNetworkID() + " ~ Added Player: " + p.getNetworkID());
            }
        }
    }

    //Clients get this method called to update a specific player
    [PunRPC]
    public void updatePlayer(Andor.Player p)
    {
        Game.getGame().updatePlayer(p);
        Debug.Log(Game.myPlayer.getNetworkID() + " ~ Updated Player");
    }

    [PunRPC]
    public void sendAction(Action a)
    {
        Game.getGame().processAction(a);
    }

    [PunRPC]
    public void setEventCardOrder(int[] event_cards)
    {
        Game.getGame().event_cards = event_cards;
        Debug.Log("got event cards");
    }

    [PunRPC]
    public void setFogTokenOrder(string[] fog_tokens)
    {
        Game.getGame().fogtoken_order = fog_tokens;
        Debug.Log("got fog tokens");
    }


    public static byte[] SerializeThis(object obj)
    {
        BinaryFormatter bf = new BinaryFormatter();
        using (var ms = new MemoryStream())
        {
            bf.Serialize(ms, obj);
            return ms.ToArray();
        }
    }

    // Convert a byte array to an Object
    public static object Deserialize(byte[] arrBytes)
    {
        using (var memStream = new MemoryStream())
        {
            var binForm = new BinaryFormatter();
            memStream.Write(arrBytes, 0, arrBytes.Length);
            memStream.Seek(0, SeekOrigin.Begin);
            var obj = binForm.Deserialize(memStream);
            return obj;
        }
    }

}