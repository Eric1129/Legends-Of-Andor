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
    public Transform fightScreen;
    public Button nextButton;
    public Text selectedChoiceText;
    public Transform selectHeroFight;
    public Button startFight;
    public Button stopButton;
    public Button flipButton;
    public Button doneButton;
    public Text rollsLeft;
    public Text battleValue;
    public Text monsterBattleValueText;
    public Text header;
    public Button endBattleButton;
    public Button nextRoundButton;

    private int fightType; //solo = 0, collab = 1

    private List<string> availablePlayers; //players that are eligible to fight
    private List<string> invitedPlayers; //players that are invited to the fight
    private List<string> involvedPlayers; //players that have accepted the fight invite

    private Dictionary<string, bool> playerResponded; //keeps track of which players have responded to fight request

    private int round;
    private Fight fight;

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
        if (fightType == 0)
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
        if (fightType == 0)
        {
            involvedPlayers.Add(Game.myPlayer.getNetworkID());
            //load solo fight scene
            startFightClick();
        }
        else {
            //invite players

            selectHeroFight.gameObject.SetActive(true);
            pickYourFighter();

        }
    }

    public void startSoloFight()
    {
        fightScreen.gameObject.SetActive(true);
        int myLocation = Game.gameState.getPlayerLocations()[involvedPlayers[0]];
        Monster monster = Game.gameState.getMonsters()[0];
        foreach (Monster m in Game.gameState.getMonsters())
        {
            int monsterLoc = m.getLocation();

            if (monsterLoc == myLocation)
            {
                monster = m;
            }
        }

        fight = new Fight(involvedPlayers.ToArray(), monster);
        displayHero(Game.gameState.getPlayer(fight.fighters[0]));
        displayMonster(monster);
        Hero h = Game.gameState.getPlayer(fight.fighters[0]).getHero();

        //increase time tracker
        h.setHour(1 + h.getHour());
        GameController.instance.setTime(fight.fighters[0], h.getHour());


    }
    private int archerRound = 1;
    private int archerRoll = 0;
    private int maxArcherRound = 0;

    private int wizardRoll = 0;

    public void heroRoll()
    {
        rollButtonActive(true);
        Hero h = Game.gameState.getPlayer(fight.fighters[0]).getHero();
        if (h.getHeroType().Equals("Female Archer") || h.getHeroType().Equals("Male Archer"))
        {

            maxArcherRound = h.getNumDice();
            Debug.Log("MAX archer round : " + maxArcherRound);
            if((maxArcherRound - archerRound) > 0)
            {
                List<int> diceRolls = h.rollDice();
                archerRoll = diceRolls[0];
                displayDiceRoll(diceRolls);
                rollsLeft.gameObject.SetActive(true);
                rollsLeft.text = "Rolls Left: " + (maxArcherRound-archerRound);
                stopButton.gameObject.SetActive(true);
                archerRound++;
            }
            else
            {
                
                displayFinalOutcome(archerRoll);
                
            }

            
            

        }else if (h.getHeroType().Equals("Female Wizard") || h.getHeroType().Equals("Male Wizard"))
        {
            List<int> diceRolls = h.rollDice();
            wizardRoll = diceRolls[0];
            displayDiceRoll(diceRolls);
            flipButton.gameObject.SetActive(true);
            doneButton.gameObject.SetActive(true);
        }
        else
        {
            List<int> diceRolls = h.rollDice();
            displayDiceRoll(diceRolls);

            int maxDiceRoll = -1;

            foreach (int dice in diceRolls)
            {

                if (dice > maxDiceRoll)
                {
                    maxDiceRoll = dice;
                }
            }

            displayFinalOutcome(maxDiceRoll);
            
        }

    }

   

    public void rollButtonActive(bool active)
    {
        Transform[] trs = fightScreen.GetComponentsInChildren<Transform>();
        foreach (Transform t in trs)
        {
            if (t.name == "Dice")
            {
                t.GetComponent<Button>().interactable = active;
            }

        }
    }
  
    public void flipButtonClick()
    {
        wizardRoll = 7 - wizardRoll;
        displayFinalOutcome(wizardRoll);
    }

    public void doneButtonClick()
    {
        displayFinalOutcome(wizardRoll);
    }
    public void stopClick()
    {
        displayFinalOutcome(archerRoll);
    }

    public void displayDiceRoll(List<int> diceRoll)
    {
        
        string diceText = "";
        foreach (int dice in diceRoll)
        {
            diceText += dice + "\t";
            
        }
        Transform[] trs = fightScreen.GetComponentsInChildren<Transform>();
        foreach(Transform t in trs)
        {
            if(t.name == "DiceRolls")
            {
                

                t.GetComponent<Text>().text = diceText;
            }

        }
    }

    public void displayFinalOutcome(int final)
    {
        Hero h = Game.gameState.getPlayer(fight.fighters[0]).getHero();
        Transform[] trs = fightScreen.GetComponentsInChildren<Transform>();
        foreach (Transform t in trs)
        {


            if (t.name == "FinalOutcome")
            {
                t.GetComponent<Text>().text = "Final Outcome: " + final;
            }
        }
        displayBattleValue(final);
        rollButtonActive(false);

        flipButton.gameObject.SetActive(false);
        doneButton.gameObject.SetActive(false);
        stopButton.gameObject.SetActive(false);

        if (h.hasArticle("Shield") || h.hasArticle("Helm") || h.hasArticle("WitchBrew"))
        {
            //TODO: ask user if they would like to use article
        }
        else
        {
            creatureTurn();
        }

    }

    int heroBattleValue = 0;
    public void displayBattleValue(int final)
    {

        Hero h = Game.gameState.getPlayer(fight.fighters[0]).getHero();
        heroBattleValue = (final + h.getStrength());
        battleValue.text = "Battle Value: " + heroBattleValue;

    }

    public void creatureTurn()
    {
        monsterRoll();
    }
    public void monsterRoll()
    {
        List<int> monsterDice = fight.monster.diceRoll();
        string diceText = "";
        foreach (int dice in monsterDice)
        {
            diceText += dice + "\t";
        }
        Transform[] trs = fightScreen.GetComponentsInChildren<Transform>();
        int final = monsterRollOutcome(monsterDice);
        foreach (Transform t in trs)
        {
            if (t.name == "MonsterDiceRolls")
            {


                t.GetComponent<Text>().text = diceText;
            }
            if (t.name == "MonsterFinalOutcome")
            {
                t.GetComponent<Text>().text = "Final Outcome: " + final;
            }
        }

        creatureBattleValue(final);
        setWinner();
    }

    public int monsterRollOutcome(List<int> monsterDice)
    {
        int max = findMaxValueOfIdenticalDice(monsterDice);
        foreach (int dice in monsterDice)
        {
            if (dice > max)
            {
                max = dice;
            }
        }
        return max;

    }

    public int findMaxValueOfIdenticalDice(List<int> monsterDice)
    {
        Dictionary<int, int> identicalDice = new Dictionary<int, int>(); //<diceRoll, totalValue>
        int max = 0;
        foreach (int dice in monsterDice)
        {
            if (identicalDice.ContainsKey(dice))
            {
                identicalDice[dice] += dice;
            }
            else
            {
                identicalDice.Add(dice, dice);
            }
        }

        foreach (int value in identicalDice.Values)
        {
            if (value > max)
            {
                max = value;
            }
        }
        return max;
    }


    int monsterBattleValue = 0;

    public void creatureBattleValue(int final)
    {
        monsterBattleValue = fight.monster.getStrength() + final;
        monsterBattleValueText.text = "Battle Value: " + monsterBattleValue;
    }

    public void setWinner()
    {
        int difference = heroBattleValue - monsterBattleValue;
        if(difference > 0)
        {
            header.text = "Hero Wins Round!";
            Monster m = fight.monster;
            m.decreaseWillpower(difference);
            
        }
        else if(difference < 0)
        {
            header.text = "Creature Wins Round.";
            Hero h = Game.gameState.getPlayer(fight.fighters[0]).getHero();
            h.increaseWillpower(difference); //since the value will be negative
        }
        else
        {
            header.text = "Draw!";
        }

        displayHero(Game.gameState.getPlayer(fight.fighters[0]));
        displayMonster(fight.monster);

        if (fight.monster.getWillpower() == 0)
        {
            header.text = "Battle Over: Hero Wins!";
            //game over
            endBattle(1);
        }

        else if(Game.gameState.getPlayer(fight.fighters[0]).getHero().getHour() + 1
            == Game.gameState.TIME_endTime)
        {
            //endBattle
            endBattle(0);
        }

        else if(Game.gameState.getPlayer(fight.fighters[0]).getHero().getWillpower() == 0)
        {
            endBattle(-1);
        }
        else
        {
            endBattleButton.gameObject.SetActive(true);
            nextRoundButton.gameObject.SetActive(true);
            
        }

        
    }

    public void endBattle(int outcome)
    {
        if(outcome > 0)
        {
            //hero wins
        }else if(outcome == 0)
        {
            //draw or hero clicked end battle
            fight.monster.recover();
        }
        else
        {
            //monster wins
        }
    }

    public void nextRound()
    {
        Hero h = Game.gameState.getPlayer(fight.fighters[0]).getHero();

        //increase time tracker
        h.setHour(1 + h.getHour());
        GameController.instance.setTime(fight.fighters[0], h.getHour());

        rollButtonActive(true);

        endBattleButton.gameObject.SetActive(false);
        nextRoundButton.gameObject.SetActive(false);

        

    }

 
    public void startCollabFight()
    {
        fightScreen.gameObject.SetActive(true);
        Monster monster;
        int myLocation = Game.gameState.getPlayerLocations()[involvedPlayers[0]];
        foreach (Monster m in Game.gameState.getMonsters())
        {
            int monsterLoc = m.getLocation();

            if (monsterLoc == myLocation)
            {
                monster = m;
            }
        }
        

    }

    public void displayHero(Andor.Player player)
    {
        
        GameObject heroGameObject = GameObject.Find("Hero");
        Transform[] trs = heroGameObject.GetComponentsInChildren<Transform>();
        
        foreach (Transform attr in trs)
        {
            Debug.Log("loop");
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
            if (attr.name == "Attributes")
            {
                Debug.Log("Hero items");
                Text heroitems = attr.GetComponent<Text>();

                heroitems.text = "Strength: " + player.getHero().getStrength();
                heroitems.text += "\nWill Power: " + player.getHero().getWillpower() + "\n";


            }
        }
    }

    public void displayMonster(Monster m)
    {
        GameObject monster = GameObject.Find("Monster");
        Transform[] trs = monster.GetComponentsInChildren<Transform>();

        foreach (Transform attr in trs)
        {
            if(attr.name == "Name")
            {
                attr.GetComponent<Text>().text = m.getMonsterType();
            }

            if(attr.name == "Image"){
                Debug.Log("Monster image");
                Sprite monsterSprite = Resources.Load<Sprite>("Monsters/" + m.getMonsterType());
                attr.GetComponent<Image>().sprite = monsterSprite;
                attr.GetComponent<Image>().useSpriteMesh = true;
            }
            if(attr.name == "Attributes")
            {
                attr.GetComponent<Text>().text = "Strength: " + m.getStrength() + "\nWill Power: " + m.getWillpower();
            }
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
        
        Game.sendAction(new StartFight(involvedPlayers.ToArray(), fightType));
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

