using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.UI;

public class SaveGameDetails : MonoBehaviour
{
    public Text gameNameLabel;
    public Text gameSaveTimeLabel;

    private GameState gameState;

    public void init(string gameName, GameState gs)
    {
        gameState = gs;
        gameNameLabel.text = gameName;
        gameSaveTimeLabel.text = gs.getSaveTime().ToString("MM/dd/yyyy");
    }

    public void gameButtonClick()
    {
        RoomLobbyController.preLoadedGameState = gameState;
        Debug.Log("GAMESTATE:");
        Debug.Log(RoomLobbyController.preLoadedGameState);
        Debug.Log("Creating room...");
        Create_Game.ROOMNAME = gameNameLabel.text;

        if (PhotonNetwork.InLobby)
        {
            RoomOptions roomOps = new RoomOptions() { IsVisible = !SavedGameController.isPrivate, IsOpen = true, MaxPlayers = (byte)4 };
            if (PhotonNetwork.CreateRoom(gameNameLabel.text, roomOps)) //attempting to create a new room 
            {
                Debug.Log("Room Created!");
            }
            else
            {
                Debug.Log("Could not create room.");
            }
        }
        else
        {
            Debug.Log("Not in Lobby?");
        }
    }
}
