using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FightScreenController : MonoBehaviour
{
    // Start is called before the first frame update
    public Transform fightChoice;
    public Transform collabButton;
    public Button nextButton;
    public Text selectedChoiceText;

    private int fightType; //solo = 0, collab = 1

    private List<string> availablePlayers; //players that are eligible to fight
    private List<string> invitedPlayers; //players that are invited to the fight
    private List<string> involvedPlayers; //players that have accepted the fight invite

    private int round;
    public FightScreenController()
    {
        
    }

    public void displayTypeOfFight()
    {
        fightChoice.gameObject.SetActive(true);
        collabButton.GetComponent<Button>().interactable = setAvailablePlayers();
    }

    public void setTypeOfFight(int type)
    {
        //solo is 0
        //collab is 1

        fightType = type;
        if(fightType == 0)
        {
            selectedChoiceText.text = "Fight Alone";
        }
        else
        {
            selectedChoiceText.text = "Fight Together";
        }

        nextButton.interactable = true;

        
    }

    private bool setAvailablePlayers()
    {
        //returns false if there are no players that can fight with this hero
        bool playersAvailable = false;
        foreach (Andor.Player p in Game.gameState.getPlayers())
        {
            if (!Game.myPlayer.Equals(p))
            {
                if (Game.gameState.playerLocations[p.getNetworkID()]
                == Game.gameState.playerLocations[Game.myPlayer.getNetworkID()])
                {
                    playersAvailable = true;
                    availablePlayers.Add(p.getNetworkID());
                }
            }
        }
        return playersAvailable;
    }

    public void nextClick()
    {
        if(fightType == 0)
        {
            //load solo fight scene
        }
        {
            //invite players
        }
    }

    public void invitePlayers()
    {
        //loop through all players on same space
        //check if there is a player on adjacent space with a bow
        //add them to the
    }

    public void addPlayerToInvite()
    {

    }

    public void removePlayerFromInvite()
    {

    }

    public void sendFightRequest()
    {

    }

    public void closeFightScreen()
    {
        //this is not the same as ending a fight
        fightChoice.gameObject.SetActive(false);
        availablePlayers.Clear();
        invitedPlayers.Clear();
        involvedPlayers.Clear();

    }
    
}

