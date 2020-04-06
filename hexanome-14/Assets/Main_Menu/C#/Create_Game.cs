using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Create_Game : MonoBehaviour
{
    // Game Objects
    private InputField game_name_input;

    private Button private_button;
    private Button public_button;

    private Button L1_button;
    private Button L2_button;

    private GameObject inviteContainer;

    private static bool game_private = false;
    public static int LEGEND;

    private bool clicked = false;
    public static string ROOMNAME = "";




    void Start()
    {
        // Initialize Game Objects
        game_name_input = GameObject.FindGameObjectWithTag("Game_Name_input").GetComponent<InputField>();

        private_button = GameObject.FindGameObjectWithTag("Private_Game_button").GetComponent<Button>();
        public_button = GameObject.FindGameObjectWithTag("Public_Game_button").GetComponent<Button>();

        L1_button = GameObject.FindGameObjectWithTag("Legend1_button").GetComponent<Button>();
        L2_button = GameObject.FindGameObjectWithTag("Legend2_button").GetComponent<Button>();

        inviteContainer = GameObject.FindGameObjectWithTag("inviteContainer");


        private_button.interactable = true;
        public_button.interactable = false;
        L1_button.interactable = false;
        L2_button.interactable = true;
        LEGEND = 1;
        inviteContainer.SetActive(false);

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

    public void legend1Click()
    {
        LEGEND = 1;

        L1_button.interactable = false;
        L2_button.interactable = true;
    }

    public void legend2Click()
    {
        LEGEND = 2;

        L1_button.interactable = true;
        L2_button.interactable = false;
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
            Debug.Log("Not in Lobby?");
        }
    }


}
