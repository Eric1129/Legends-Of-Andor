using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.UI;

public class Create_Game : MonoBehaviour
{
    // Game Objects
    public InputField game_name_input;

    public Button private_button;
    public Button public_button;

    public Button Easy_button;
    public Button Hard_button;

    public GameObject inviteContainer;

    private static bool game_private = false;
    public static string DIFFICULTY;

    public static string ROOMNAME = "";

    void Start()
    {
        private_button.interactable = true;
        public_button.interactable = false;
        Easy_button.interactable = false;
        Hard_button.interactable = true;
        DIFFICULTY = "Easy";
        inviteContainer.SetActive(false);

        Game.loadedFromFile = false;

    }

    public void privateGameClick()
    {
        game_private = true;

        private_button.interactable = false;
        public_button.interactable = true;

        inviteContainer.SetActive(true);

    }

    public void publicGameClick()
    {
        game_private = false;

        private_button.interactable = true;
        public_button.interactable = false;

        inviteContainer.SetActive(false);

    }

    public void EasyClick()
    {
        DIFFICULTY = "Easy";

        Easy_button.interactable = false;
        Hard_button.interactable = true;
    }

    public void HardClick()
    {
        DIFFICULTY = "Hard";

        Easy_button.interactable = true;
        Hard_button.interactable = false;
    }



    public void CreateRoomOnClick()
    {
        // HAVE TO ADD SOME DATA VALIDATION:
        //
        //
        if (game_name_input.text.Equals(""))
        {
            return;
        }

        Debug.Log("Creating room...");
        Create_Game.ROOMNAME = game_name_input.text;

        if (PhotonNetwork.InLobby)
        {
            RoomOptions roomOps = new RoomOptions() { IsVisible = !game_private, IsOpen = true, MaxPlayers = (byte)4 };
            if (PhotonNetwork.CreateRoom(Create_Game.ROOMNAME, roomOps)) //attempting to create a new room 
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
            Debug.Log("Not in Lobby1?");
        }
    }

}
