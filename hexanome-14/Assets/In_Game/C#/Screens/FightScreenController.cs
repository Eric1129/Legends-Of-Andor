using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FightScreenController : MonoBehaviour
{
    // Start is called before the first frame update
    public Transform fightChoice;
    public Transform collabButton;

    public Transform fightLobby;
    public Button nextButton;
    public Text selectedChoiceText;
    public Transform selectHeroFight;
    public Button startFight;

    private int fightType; //solo = 0, collab = 1

    private List<string> availablePlayers; //players that are eligible to fight
    private List<string> invitedPlayers; //players that are invited to the fight
    private List<string> involvedPlayers; //players that have accepted the fight invite

    private Dictionary<string, bool> playerResponded; //keeps track of which players have responded to fight request

    private int round;

    public FightScreenController()
    {
        availablePlayers = new List<string>();
        invitedPlayers = new List<string>();
        involvedPlayers = new List<string>();
        playerResponded = new Dictionary<string, bool>();

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
            selectedChoiceText.gameObject.SetActive(true);
            selectedChoiceText.text = "Fight Alone";
        }
        else
        {
            selectedChoiceText.gameObject.SetActive(true);
            selectedChoiceText.text = "Fight Together";
        }

        nextButton.interactable = true;

        
    }

    private bool setAvailablePlayers()
    {
        //returns false if there are no players that can fight with this hero
        bool playersAvailable = false;
        int myPlayerLoc = Game.gameState.playerLocations[Game.myPlayer.getNetworkID()];
        foreach (Andor.Player p in Game.gameState.getPlayers())
        {
            if (!Game.myPlayer.Equals(p))
            {
                int otherPlayerLoc = Game.gameState.playerLocations[p.getNetworkID()];
                if (otherPlayerLoc == myPlayerLoc)
                {
                    playersAvailable = true;
                    availablePlayers.Add(p.getNetworkID());
                }
                else
                {
                    if (p.getHero().hasArticle("Bow"))
                    {
                        List<Node> neighbours = Game.gameState.positionGraph.getNode(myPlayerLoc).getAdjacentNodes();
                        foreach (Node n in neighbours)
                        {
                            if (otherPlayerLoc == n.getIndex())
                            {
                                playersAvailable = true;
                                availablePlayers.Add(p.getNetworkID());
                            }
                        }
                    }
                    
                    
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
        else{
            //invite players

            selectHeroFight.gameObject.SetActive(true);
            pickYourFighter();

        }
    }

    public void pickYourFighter()
    {
        int i = 1;
        foreach(string player in availablePlayers)
        {
            //displayPlayer
            Andor.Player p = Game.gameState.getPlayer(player);
            displayPlayerInfo(p, i);
            i++;
        }
    }
    public void displayPlayerInfo(Andor.Player player, int i)
    {

        GameObject selectHero = GameObject.Find("SelectHero");
        GameObject herogameobj;
        Transform[] trs = selectHero.GetComponentsInChildren<Transform>(true);
        //Transform[] heroattr = new Transform[3];
        foreach (Transform t in trs)
        {

            if (t.name == ("Hero" + i))
            {
                herogameobj = t.gameObject;
                t.gameObject.SetActive(true);
                Transform[] heroattr = herogameobj.GetComponentsInChildren<Transform>(true);
                foreach (Transform attr in heroattr)
                {
                    attr.gameObject.SetActive(true);
                    if (attr.name == "Name")
                    {

                        Text heroname = attr.GetComponent<Text>();
                        heroname.text = player.getHeroType();
                    }
                    if (attr.name == "Image")
                    {
                        Debug.Log("Image");
                        Sprite herosprite = Resources.Load<Sprite>("PlayerSprites/" + player.getHeroType());
                        attr.GetComponent<Image>().sprite = herosprite;
                        attr.GetComponent<Image>().useSpriteMesh = true;
                    }
                    if (attr.name == "HeroItems")
                    {
                        Debug.Log("Hero items");
                        Text heroitems = attr.GetComponent<Text>();

                        heroitems.text = "Strength: " + player.getHero().getStrength();
                        heroitems.text += "\nWill Power: " + player.getHero().getWillpower() + "\n";

                        heroitems.text += "Articles: ";
                        heroitems.text += player.getHero().allArticlesAsString();

                
                    }
                }
            }
        }


    }

    public void addPlayerToInvite(int index)
    {
        string selectedPlayer = availablePlayers[index-1];
        invitedPlayers.Add(selectedPlayer);
        GameObject selectHero = GameObject.Find("SelectHero");
       
        Transform[] trs = selectHero.GetComponentsInChildren<Transform>(true);
        //Transform[] heroattr = new Transform[3];
        foreach (Transform t in trs)
        {
            if(t.name == "Remove" + index)
            {
                t.gameObject.SetActive(true);
            }

            if(t.name == "InviteList")
            {
                t.gameObject.SetActive(true);
                string invitePlayersString = "Invite List: ";
                foreach (string p in invitedPlayers)
                {
                    invitePlayersString += Game.gameState.getPlayer(p).getHeroType();
                }
                t.gameObject.GetComponent<Text>().text = invitePlayersString;
            }
            if(t.name == "SendRequest")
            {
                t.gameObject.GetComponent<Button>().interactable = true;
            }
        }
    }

    public void removePlayerFromInvite(int index)
    {
        string removedPlayer = invitedPlayers[index-1];
        invitedPlayers.Remove(removedPlayer);
        GameObject selectHero = GameObject.Find("SelectHero");

        Transform[] trs = selectHero.GetComponentsInChildren<Transform>(true);
        //Transform[] heroattr = new Transform[3];
        foreach (Transform t in trs)
        {
            if (t.name == "Remove" + index)
            {
                t.gameObject.SetActive(false);
            }

            if (t.name == "InviteList")
            {
                t.gameObject.SetActive(true);
                string invitePlayersString = "";
                foreach (string p in invitedPlayers)
                {
                    invitePlayersString += Game.gameState.getPlayer(p).getHeroType();
                }
                t.gameObject.GetComponent<Text>().text = invitePlayersString;
            }
            if (t.name == "SendRequest")
            {
                if(invitedPlayers.Count == 0)
                {
                    t.gameObject.GetComponent<Button>().interactable = false;
                }
                
            }

        }
    }

    public void sendFightRequest()
    {

        string[] players = new string[1 + invitedPlayers.Count];
        players[0] = Game.myPlayer.getNetworkID();
        int i = 1;
        foreach(string p in invitedPlayers)
        {
            playerResponded.Add(p, false);
            players[i] = p;
            i++;
        }
        closeFightScreen();
        Game.sendAction(new InviteFighter(players));
    }

    public void acceptFightRequest(bool accept, string player)
    {
        Debug.Log("Accepting fight");
        string[] players = new string[1];
        players[0] = player;
        if (accept)
        {
            Game.sendAction(new RespondFight(players, accept, false));
        }
    }

    public void openFightLobby(string fighter)
    {
        string[] players = new string[1];
        players[0] = fighter;
        Game.sendAction(new RespondFight(players, true, true));
        fightLobby.gameObject.SetActive(true);
        updateFightLobby();
        startFight.gameObject.SetActive(true);
    }

    public void addHostPlayer(string player)
    {
        involvedPlayers.Add(player);
    }

    public void joinFightLobby(string fighter)
    {
        Debug.Log(Game.gameState.getPlayer(fighter).getHeroType() + " joining fight lobby");
        respondToFight(fighter);
        involvedPlayers.Add(fighter);
        Debug.Log("Num players " + involvedPlayers.Count);
        fightLobby.gameObject.SetActive(true);
        updateFightLobby();
        
    }

    private void updateFightLobby()
    {
        int i = 1;
        foreach(string fighter in involvedPlayers)
        {
            displayPlayerInFightLobby(fighter, i);
            i++;
        }
    }

    public void respondToFight(string player)
    {
        playerResponded[player] = true;
    }

    public bool allResponded()
    {
        
        foreach(bool r in playerResponded.Values)
        {
            if(r == false)
            {
                return false;
            }
        }

        return playerResponded.Values.Count > 0;
    }

    public void fightReady()
    {
        startFight.interactable = true;
    }

    private void displayPlayerInFightLobby(string player, int i)
    {
        Transform[] trs = fightLobby.GetComponentsInChildren<Transform>(true);
        foreach (Transform t in trs)
        {
            if (t.name == "Hero" + i)
            {
                t.gameObject.GetComponent<Text>().text = Game.gameState.getPlayer(player).getHeroType();
            }
        }
    }

    public void startFightClick()
    {
        Debug.Log("FIGHT");
    }

    public void closeFightScreen()
    {
        //this is not the same as ending a fight
        fightChoice.gameObject.SetActive(false);
        selectHeroFight.gameObject.SetActive(false);
        availablePlayers.Clear();
        invitedPlayers.Clear();
        involvedPlayers.Clear();

    }
    
}

