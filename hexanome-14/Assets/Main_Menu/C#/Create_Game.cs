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

    private Text invite_friends_label;
    private InputField invite_friends_input;

    private static bool game_private = false;
    private static int legend;

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

        invite_friends_label = GameObject.FindGameObjectWithTag("CG_Invite_Friends_label").GetComponent<Text>();
        invite_friends_input = GameObject.FindGameObjectWithTag("CG_Invite_Friends_input").GetComponent<InputField>();


        invite_friends_input.enabled = false;
        invite_friends_label.enabled = false;

    }

    public void privateGameClick()
    {
        game_private = true;

        private_button.interactable = false;
        public_button.interactable = true;

        invite_friends_input.gameObject.SetActive(true);
        invite_friends_label.gameObject.SetActive(true);
    }

    public void publicGameClick()
    {
        game_private = false;

        private_button.interactable = true;
        public_button.interactable = false;

        invite_friends_input.gameObject.SetActive(false);
        invite_friends_label.gameObject.SetActive(false);
    }

    public void legend1Click()
    {
        legend = 1;

        L1_button.interactable = false;
        L2_button.interactable = true;
    }

    public void legend2Click()
    {
        legend = 2;

        L1_button.interactable = true;
        L2_button.interactable = false;
    }



    public void CreateRoomOnClick()
    {
     
        Debug.Log("Creating room...");
        Create_Game.ROOMNAME = game_name_input.text;

        // HAVE TO ADD SOME DATA VALIDATION:
        //
        //
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
