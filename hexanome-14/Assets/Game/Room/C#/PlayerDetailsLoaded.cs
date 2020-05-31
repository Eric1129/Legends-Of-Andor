using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerDetailsLoaded : MonoBehaviour
{

    public Text nameLabel;
    public Dropdown playerDropdown;
    public Button readyButton;
    public Transform mainContainer;

    private bool ready = false;

    private void Start()
    {
        

    }
    public void init(Andor.Player p)
    {
        List<string> options = new List<string>();

        options.Add("Not Selected");
        foreach (Andor.Player player in RoomLobbyController.preLoadedGameState.getPlayers())
        {
            options.Add(player.getNetworkID());
        }

        playerDropdown.ClearOptions();
        playerDropdown.AddOptions(options);

        playerDropdown.value = RoomLobbyController.instance.playerMatches[p.getNetworkID()];


        if (p.getNetworkID().Equals(Game.myPlayer.getNetworkID()))
        {

            playerDropdown.onValueChanged.AddListener(delegate
            {
                selectvalue();
            });
        }
        else
        {
            playerDropdown.interactable = false;
        }



    }
    private void selectvalue()
    {
        Debug.Log("val: " + playerDropdown.value);
        Game.myPlayer.getHero().setGold(playerDropdown.value);

        if (playerDropdown.value == 0)
        {
            this.ready = false;
            Game.myPlayer.ready = this.ready;


            setReady(this.ready);
        }
        //using gold as a dropdown value holder
        Game.myPlayer.getHero().setGold(playerDropdown.value);

        Game.updatePlayer(Game.myPlayer);
    }

    public void setReady(bool r)
    {
        ColorBlock buttonColors = readyButton.colors;

        this.ready = r;
        if (r)
        {
            buttonColors.normalColor = new Color32(95, 255, 95, 255);
            buttonColors.highlightedColor = new Color32(142, 253, 142, 255);
            buttonColors.pressedColor = new Color32(182, 255, 182, 255);
            buttonColors.selectedColor = new Color32(206, 255, 206, 255);
            buttonColors.disabledColor = new Color32(95, 255, 95, 255);
        }
        else
        {
            buttonColors.normalColor = new Color32(255, 95, 95, 255);
            buttonColors.highlightedColor = new Color32(253, 142, 142, 255);
            buttonColors.pressedColor = new Color32(255, 182, 182, 255);
            buttonColors.selectedColor = new Color32(255, 206, 206, 255);
            buttonColors.disabledColor = new Color32(255, 95, 95, 255);
        }
        readyButton.colors = buttonColors;
    }


    public void readyClick()
    {
        if(Game.myPlayer.getHero().getGold() != 0)
        {
            this.ready = !this.ready;
            Game.myPlayer.ready = this.ready;
            Game.updatePlayer(Game.myPlayer);

            Debug.Log("Ready Pressed! Current state: " + this.ready);

            setReady(this.ready);
        }
        
    }
}

