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
    public Transform fightLobby2;
    public Transform fightScreen;
    public Transform rewardScreen;
    public Transform battleEndScreen;
    public Transform distributeReward;
    public Transform waitingScreen;


    public Button nextButton;
    
    public Transform selectHeroFight;
    public Button startFight;
    public Button stopButton;
    public Button flipButton;
    public Button doneButton;
    public Button helmButton;
    public Button bowButton;
    public Button noArticleSelected;
    public Button witchBrewButton;
    public Text rollsLeft;
    public Text battleValue;
    public Text selectedChoiceText;
    public Text monsterBattleValueText;
    public Text header;
    public Button endBattleButton;
    public Button nextRoundButton;
    public string hostPlayer;

    public GameObject updateRollArticle;
    public List<string> nextRoundPlayers = new List<string>();

    public Text updateRollText;

   public bool creatureTurnCheck = false;

    public GameObject chooseArticleScroll;
    private int fightType; //solo = 0, collab = 1

    private List<string> availablePlayers; //players that are eligible to fight
    private List<string> invitedPlayers; //players that are invited to the fight
    public List<string> involvedPlayers; //players that have accepted the fight invite

    private Dictionary<string, bool> playerResponded; //keeps track of which players have responded to fight request
    private Dictionary<string, bool> playerRespondNextRound;


    private int round;
    public Fight fight;
    private bool winGame;
    private bool gettingReward = false;


    private void Update()
    {
        if (!Game.myPlayer.getHero().usingHelm && Game.myPlayer.getHero().getAllArticles().ContainsKey("Helm"))
        {
            helmButton.gameObject.SetActive(true);
        }
        if (!Game.myPlayer.getHero().usingWitchBrew && Game.myPlayer.getHero().getAllArticles().ContainsKey("WitchBrew"))
        {
            witchBrewButton.gameObject.SetActive(true);
        }
        if (!Game.myPlayer.getHero().usingBow && Game.myPlayer.getHero().getAllArticles().ContainsKey("Bow"))
        {
            bowButton.gameObject.SetActive(true);
        }
        if (Game.myPlayer.getHero().usingHelm)
        {
            Debug.Log("still using brew");
            helmButton.interactable = false;
            witchBrewButton.gameObject.SetActive(false);
        }
        if (Game.myPlayer.getHero().usingWitchBrew)
        {
            witchBrewButton.interactable = false;
            helmButton.gameObject.SetActive(false);
        }

        if (winGame)
        {
            if(Game.myPlayer.getNetworkID() == this.hostPlayer)
            {
                
                distributeOrWait(true);
                
            }
            else
            {
                //closeFightScreen();
                distributeOrWait(false);
               
                //
            }
        }

        if (gettingReward)
        {
            rewardScreen.gameObject.SetActive(true);
            gettingReward = false;
        }
        //if(Game.myPlayer.getNetworkID() ==)
        //if (Game.myPlayer.getHero().usingBow)
        // {
        //     bowButton.interactable = false;
        //     bowButton.gameObject.SetActive(false);
        // }
    }




    public FightScreenController()
    {
        availablePlayers = new List<string>();
        invitedPlayers = new List<string>();
        involvedPlayers = new List<string>();
        playerResponded = new Dictionary<string, bool>();
        playerRespondNextRound = new Dictionary<string, bool>();
    }

    public void displayTypeOfFight()
    {
        
        fightChoice.gameObject.SetActive(true);
        collabButton.GetComponent<Button>().interactable = setAvailablePlayers();
    }

    public void distributeResponse(Dictionary<string, int> rewards)
    {
        Debug.Log("does this get called????");
        //gettingReward = true;
        foreach(string fighter in fight.fighters)
        {
            if(Game.myPlayer.getNetworkID() == fighter)
            {
                fighterRewards = rewards;
                distributeReward.gameObject.SetActive(false);
                waitingScreen.gameObject.SetActive(false);
                rewardScreen.gameObject.SetActive(true);

            }
        }
        fighterRewards = rewards;
        distributeReward.gameObject.SetActive(false);
        waitingScreen.gameObject.SetActive(false);
        rewardScreen.gameObject.SetActive(true);
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
        if(Game.myPlayer.getNetworkID() == fight.getCurrentFighter())
        {
            fightScreen.gameObject.SetActive(true);
        }
        
        displayHero(Game.gameState.getPlayer(fight.getCurrentFighter()));
        displayMonster(monster);
        Hero h = Game.gameState.getPlayer(fight.getCurrentFighter()).getHero();

        //increase time tracker
        h.setHour(1 + h.getHour());
        GameController.instance.setTime(fight.getCurrentFighter(), h.getHour());


    }
    private int archerRound = 1;
    private int archerRoll = 0;
    private int maxArcherRound = 0;

    private int wizardRoll = 0;

    private int rolledDiceHelm;

     private int rolledDiceBrew;

    private int heroFinalRoll;

    private int wizardRollBrew;

    private int archerRollBrew;
    public void heroRoll()
    {
        if(fightType == 0)
        {
            soloRoll();
        }
        else
        {
            collabRoll();
        }

    }

    

    public void soloRoll()
    {
        rollButtonActive(true);
        Hero h = Game.gameState.getPlayer(fight.getCurrentFighter()).getHero();
        if (h.getHeroType().Equals("Female Archer") || h.getHeroType().Equals("Male Archer"))
        {

            maxArcherRound = h.getNumDice();
            Debug.Log("MAX archer round : " + maxArcherRound);
            if ((maxArcherRound - archerRound) > 0)
            {
                List<int> diceRolls = h.rollDice();
                archerRoll = diceRolls[0];
                archerRollBrew = (2*diceRolls[0]);
                displayDiceRoll(diceRolls);
                rollsLeft.gameObject.SetActive(true);
                rollsLeft.text = "Rolls Left: " + (maxArcherRound - archerRound);
                stopButton.gameObject.SetActive(true);
                archerRound++;
            }
            else
            {
                displayFinalOutcome(archerRoll);
            }


        }
        else if (h.getHeroType().Equals("Female Wizard") || h.getHeroType().Equals("Male Wizard"))
        {
            List<int> diceRolls = h.rollDice();
            wizardRoll = diceRolls[0];
            wizardRollBrew = (2*diceRolls[0]);
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
            rolledDiceHelm = calculateHelm(diceRolls);
            rolledDiceBrew = (2 * maxDiceRoll);

            displayFinalOutcome(maxDiceRoll);

        }
    }

public int calculateHelm(List<int> rolls)
    {
        int maxHelm = 0;
        foreach(int i in rolls)
        {
            int key = i;
            int max = 0;
            foreach (int j in rolls)
            {
                if(j == key)
                {
                    max += i;
                }
            }
            if(max >= maxHelm)
            {
                maxHelm = max;
            }
        }
        return maxHelm;
        //return 8;
    }
   
    public void collabRoll()
    {
        rollButtonActive(true);
        Hero h = Game.gameState.getPlayer(fight.getCurrentFighter()).getHero();
        List<int> diceRolls = h.rollDice();
        
        displayDiceRoll(diceRolls);
        if (h.getHeroType().Equals("Female Archer") || h.getHeroType().Equals("Male Archer"))
        {

            maxArcherRound = h.getNumDice();
            Debug.Log("MAX archer round : " + maxArcherRound);
            if ((maxArcherRound - archerRound) > 0)
            {
                archerRoll = diceRolls[0];
                rollsLeft.gameObject.SetActive(true);
                rollsLeft.text = "Rolls Left: " + (maxArcherRound - archerRound);
                stopButton.gameObject.SetActive(true);
                archerRound++;
            }
            else
            {
                displayFinalOutcome(archerRoll);
            }


        }
        else if (h.getHeroType().Equals("Female Wizard") || h.getHeroType().Equals("Male Wizard"))
        {
            
            wizardRoll = diceRolls[0];
            
            flipButton.gameObject.SetActive(true);
            doneButton.gameObject.SetActive(true);
        }
        else
        {
            

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
        archerRound = 1;
        rollsLeft.text = "Rolls Left: ";
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
        Hero h = Game.gameState.getPlayer(fight.getCurrentFighter()).getHero();
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

         if ((h.hasArticle("Helm" )&& !Game.myPlayer.getHero().usingHelm) || (h.hasArticle("WitchBrew") && !Game.myPlayer.getHero().usingWitchBrew))
        {

            // while (!h.selectedArticle)
            // {

            // }
            // //TODO: ask user if they would like to use article
            // if (h.usingWitchBrew)
            // {
            //     Debug.Log("using witch brew in fight");
            //     //double roll
            // }
            // //if (h.usingShield)
            // //{
            //     //this should only matter after battle round
            // //}
            // else if (h.usingHelm)
            // {
            //     Debug.Log("using helm in fight");
            //     //total up identical rolls and update outcome
            // }

            //creatureTurn();
            //fight.nextFighter();
            //Game.sendAction(new FightTurn(fight.fighters, fight.getIndex(), fight.getCurrentFighter()));
         creatureTurnCheck = false;
            heroFinalRoll = final;
            useArticleNotify();
        
        }
        else
        {
            creatureTurnCheck = true;
            heroFinalRoll = final;
            rollCreature();
            // if(fightType == 0)
            // {
            //     creatureTurn();

            // }
            // else
            // {
            //     //update battle value
            //     //displayBattleValue(final);

            //     //pass to next player
            //     fight.nextFighter();
            //     Debug.Log("from displayFinalOutcome()" + fight.getIndex()); 
            //     Game.sendAction(new FightTurn(fight.fighters, fight.getIndex(), fight.getCurrentFighter(), fight.getHeroBattleValue()));
            // }
            
            
        }

    }

      public void useArticleNotify()
    {
        StartCoroutine(articleroutine(5));
        chooseArticleScroll.SetActive(true);
    }

IEnumerator articleroutine(int sleep)
    {
        yield return new WaitForSeconds(sleep);
        //After we have waited 5 seconds print the time again.
        Debug.Log("Finished Coroutine at timestamp : " + Time.time);
    }


    public void rollCreature(){
        if(fightType == 0)
            {
                creatureTurn();

            }
            else
            {
                //update battle value
                //displayBattleValue(final);

                //pass to next player
                fight.nextFighter();
                Debug.Log("from displayFinalOutcome()" + fight.getIndex()); 
                Game.sendAction(new FightTurn(fight.fighters, fight.getIndex(), fight.getCurrentFighter(), fight.getHeroBattleValue()));
            }
    }

    public void nextPlayerTurnToRoll(int index, int battleValue)
    {
        //set the roll dice active and tell player it is their turn

        //update the attributes of fight which were lost on sending action
        fightType = 1;
        //Debug.Log(index);
        fight.setCurrentFighter(index);
        
        fight.setBattleValue(battleValue);
        if (Game.myPlayer.getNetworkID() == fight.getCurrentFighter())
        {
            rollButtonActive(true);
            header.text = "Your turn to fight";
        }
        else
        {
            rollButtonActive(false);
            header.text = fight.currentFighterHero().getHeroType() + " is fighting.";
        }

        displayHero(Game.gameState.getPlayer(fight.getCurrentFighter()));
        displayBattleValue(0);
    }

    //int heroBattleValue = 0;
    public void displayBattleValue(int final)
    {

        Hero h = Game.gameState.getPlayer(fight.fighters[0]).getHero();
        fight.addToBattleValue(final);
        battleValue.text = "Battle Value: " + fight.getHeroBattleValue();

    }

    public void creatureTurn()
    {

        monsterRoll();
        if(fightType == 0)
        {
            setRoundWinner();
        }
        else
        {
            
        }
        
    }

    List<int> mDiceRoll = new List<int>();

    public void creatureTurn_collab(int heroBV, List<int> monsterDiceRoll)
    {

        fight.setBattleValue(heroBV);
        displayBattleValue(0);
        header.text = "Creature Turn.";
        mDiceRoll = monsterDiceRoll;
        
        string diceText = "";
        foreach (int dice in mDiceRoll)
        {
            diceText += dice + "\t";
        }
        
        int final = monsterRollOutcome(mDiceRoll);
        Transform[] trs = fightScreen.GetComponentsInChildren<Transform>();
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
        foreach (string player in involvedPlayers)
        {
            if (!playerRespondNextRound.ContainsKey(player))
            {
                playerRespondNextRound.Add(player, false);
            }
            else
            {
                playerRespondNextRound[player] = false;
            }
            

        }
        displayCreatureBattleValue(final);
        setRoundWinner();
        
        //Game.sendAction(new CreatureTurn(involvedPlayers.ToArray(), mDiceRoll));
        //if (Game.myPlayer.getNetworkID()== fight.getCurrentFighter())
        //{

            //    //SEND THIs over the network and then display it for both players
            //}



            ////monsterRoll(); //working displays for all
            ////setRoundWinner();

            //send the creature value across network
            //Game.sendAction(new CreatureFight(
            //this should update the screens of all players with creature bv and with hero win/lose
            //handle lose/draw scenario
            //display screen for distributing reward
            //display the next round/end battle buttons
    }

    public void creatureTurnResponse(List<int> creatureRolls)
    {
        mDiceRoll = creatureRolls;
        string diceText = "";
        foreach (int dice in mDiceRoll)
        {
            diceText += dice + "\t";
        }
        Transform[] trs = fightScreen.GetComponentsInChildren<Transform>();
        foreach (Transform t in trs)
        {
            if (t.name == "MonsterDiceRolls")
            {


                t.GetComponent<Text>().text = diceText;
            }

        }

    }

    public void displayMonsterRollOutcome(int heroBattleValue, int monsterBV)
    {
        fight.setBattleValue(heroBattleValue);
        displayBattleValue(0);
        monsterBattleValue = monsterBV;
        monsterBattleValueText.text = "Battle Value: " + monsterBattleValue;

        //check round win
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

        displayCreatureBattleValue(final);
        
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

    public void displayCreatureBattleValue(int final)
    {
        monsterBattleValue = fight.monster.getStrength() + final;
        monsterBattleValueText.text = "Battle Value: " + monsterBattleValue;
    }

    

    public void setRoundWinner()
    {
       foreach(Hero h in fight.getHeroes()){
            foreach(Andor.Player p in Game.gameState.getPlayers()){
                if(p.getHero() == h){
                    Game.sendAction(new ExitFight(p.getNetworkID()));
                }
            }
        }
        int difference = fight.getHeroBattleValue() - monsterBattleValue;
        if(difference > 0)
        {
            header.text = "Hero Wins Round!";
            Monster m = fight.monster;
            m.decreaseWillpower(difference);
            
        }
        else if(difference < 0)
        {
            header.text = "Creature Wins Round.";
            foreach(Hero h in fight.getHeroes())
            {
                h.decreaseWillpower(-difference); //since the value will be negative
            }
            //Hero h = Game.gameState.getPlayer(fight.currentFighter()).getHero();
           
        }
        else
        {
            header.text = "Draw!";
        }

        displayHero(Game.gameState.getPlayer(fight.getCurrentFighter()));
        displayMonster(fight.monster);

        if(fightType == 0)
        {
            checkBattleWinner();
        }
        else
        {
            checkBattleWinner_collab();
        }
        
 
    }

    public void checkBattleWinner()
    {
        if (fight.monster.getWillpower() == 0)
        {
            header.text = "Battle Over: Hero Wins!";
            //game over
            endBattle(1);
        }

        else if (Game.gameState.getPlayer(fight.getCurrentFighter()).getHero().getHour() + 1
            == Game.gameState.TIME_endTime)
        {
            //endBattle
            endBattle(0);
        }

        else if (Game.gameState.getPlayer(fight.getCurrentFighter()).getHero().getWillpower() == 0)
        {
            header.text = "Battle Over: Creature Wins.";
            endBattle(-1);
        }
        else
        {
            //No win yet
            endBattleButton.gameObject.SetActive(true);
            nextRoundButton.gameObject.SetActive(true);
            fight.nextFighter();
            fight.resetHeroBattleValue();
        }
    }

    public void checkBattleWinner_collab()
    {

        
        

        //=================COMMENT BACK IN==========================
        //kick out players who have no more time left or no more WP
        foreach (string f in fight.fighters)
        {
            if (Game.gameState.getPlayer(f).getHero().getHour() + 1
            > Game.gameState.TIME_endTime || Game.gameState.getPlayer(f).getHero().getWillpower() == 0)
            {
                battleEndScreen.gameObject.SetActive(true);
                Transform[] trs = battleEndScreen.GetComponentsInChildren<Transform>();
                foreach(Transform t in trs)
                {
                    

                    if(t.name == "Header")
                    {
                        if (Game.gameState.getPlayer(f).getHero().getWillpower() == 0)
                        {
                            t.GetComponent<Text>().text = "No More willpower";
                        }

                        if(Game.gameState.getPlayer(f).getHero().getHour() + 1
            == Game.gameState.TIME_endTime)
                        {
                            t.GetComponent<Text>().text = "Out of time.";
                        }
                    }
                    if (t.name == "Body")
                    {
                        t.GetComponent<Text>().text = "You cannot continue to the next round.";
                    }


                }
                
                

                //leaveBattleClick();
                //fight.leaveFight(f);
            }
        }

        if (fight.monster.getWillpower() == 0)
        {
            header.text = "Battle Over: Heroes Win!";
            //game over
            //claimButton.gameObject.SetActive(true)
            fightScreen.gameObject.SetActive(false);
            //Time t = 5.0f;
            //t-= deltaTime.
            StartCoroutine(Wait(3.0f));
            endBattle_collab(1);
            
            fightLobby.gameObject.SetActive(false);
            fightLobby2.gameObject.SetActive(false);
            //closeFightScreen();

        }
        else
        {
            //No win yet
            //No win yet
            endBattleButton.gameObject.SetActive(true);
            nextRoundButton.gameObject.SetActive(true);
            //fight.nextFighter();
            fight.resetHeroBattleValue();
        }



    }

    IEnumerator Wait(float duration)
    {
        //This is a coroutine
        Debug.Log("Start Wait() function. The time is: " + Time.time);
        Debug.Log("Float duration = " + duration);
        yield return new WaitForSeconds(duration);   //Wait
        Debug.Log("End Wait() function and the time is: " + Time.time);
    }

    public void clearDistributeScreen()
    {
        Transform[] trs = distributeReward.GetComponentsInChildren<Transform>();
        int i = 1;
        foreach (Transform t in trs)
        {
            if (t.name == "f" + i)
            {
                Transform[] attrs = t.GetComponentsInChildren<Transform>();
                foreach (Transform attr in attrs)
                {
                    if (attr.name == "Add")
                    {
                        attr.GetComponent<Button>().interactable = true;
                    }
                    if (attr.name == "qty")
                    {
                        attr.GetComponent<Text>().text = "0";
                    }
                }
                i++;
            }

            if (t.name == "RewardText")
            {
                t.GetComponent<Text>().text = "Rewards Left: ";
            }
        }

    }

    public void nextRound()
    {
        if(fightType == 0)
        {
            Hero h = Game.gameState.getPlayer(fight.getCurrentFighter()).getHero();

            //increase time tracker
            h.setHour(1 + h.getHour());
            GameController.instance.setTime(fight.getCurrentFighter(), h.getHour());

            rollButtonActive(true);

            endBattleButton.gameObject.SetActive(false);
            nextRoundButton.gameObject.SetActive(false);
        }
        else
        {
            string[] players = new string[1];
            players[0] = Game.myPlayer.getNetworkID();
            
            Game.sendAction(new JoinNextRound(players, true));
            fightLobby2.gameObject.SetActive(true);
            
           // Game.sendAction(new RespondFight(players, true, false));
        }
       
    }


    public void endBattle(int outcome)
    {
        
        if (outcome ==1)
        {
            //hero wins
            //get reward
            rewardScreen.gameObject.SetActive(true);

            
            Game.gameState.removeMonster(fight.monster);

            Game.gameState.legend += 1;
            GameController.instance.advanceNarrator(Game.gameState.legend);
        }
        else if(outcome == 2)
        {
            if(fightType == 0)
            {
                fight.monster.recover();
                //end turn
                Game.gameState.turnManager.passTurn();

                battleEndScreen.gameObject.SetActive(false);
                closeFightScreen();
            }
            else
            {
                fight.monster.recover();
                leaveBattleClick();
            }
            
        }
        else if (outcome == 0)
        {
            //draw or hero clicked end battle
            fight.monster.recover();
            battleEndScreen.gameObject.SetActive(true);
            Transform[] trs = battleEndScreen.GetComponentsInChildren<Transform>();
            foreach(Transform t in trs)
            {
                if(t.name == "Body")
                {
                    t.GetComponent<Text>().text = "No time left. Draw!";
                }
            }

        }
        else
        {
            //monster wins
            Hero h = Game.gameState.getPlayer(fight.getCurrentFighter()).getHero();
            if (h.getStrength() > 0)
            {
                //lose strength point
                h.decreaseStrength(1);
            }

            h.increaseWillpower(3);
            battleEndScreen.gameObject.SetActive(true);
            Transform[] trs = battleEndScreen.GetComponentsInChildren<Transform>();
            foreach (Transform t in trs)
            {
                if (t.name == "Body")
                {
                    t.GetComponent<Text>().text = "Creature wins. You lose 1 strength point.";
                }
            }


        }

    }

    

    public void setHostPlayer(string player)
    {
        winGame = true;
        this.hostPlayer = player;
    }

    public void distributeOrWait(bool host)
    {
        winGame = false;
        if (host)
        {
            distributeReward.gameObject.SetActive(true);
            distributeRewardScreenSetup();

        }
        else
        {
            waitingScreen.gameObject.SetActive(true);
        }
        
        
       
    }

    public int rewardLeft;
    public Dictionary<string, int> fighterRewards = new Dictionary<string, int>();


    public void distributeRewardScreenSetup()
    {
        Transform[] trs = distributeReward.GetComponentsInChildren<Transform>();
        int i = 1;
        foreach(Transform t in trs)
        {
            if(i < fight.fighters.Length+1)
            {
                if (t.name == "f" + i)
                {

                    t.gameObject.SetActive(true);
                    t.GetComponent<Text>().text = Game.gameState.getPlayer(fight.fighters[i - 1]).getHeroType();
                    i++;
                }
            }
            

            if(t.name == "RewardText")
            {
                rewardLeft = fight.monster.getReward();
                t.GetComponent<Text>().text = "Rewards Left: " + fight.monster.getReward().ToString();
            }
            
        }


    }
    public void incrementReward(int buttonID)
    {
        Transform[] trs = distributeReward.GetComponentsInChildren<Transform>();
        rewardLeft--;
        if (rewardLeft == 0)
        {
            //set all add buttons to not interactable
            int i = 1;
            foreach (Transform t in trs)
            {
                if (t.name == "f" + i)
                {
                    Transform[] attrs = t.GetComponentsInChildren<Transform>();
                    foreach (Transform attr in attrs)
                    {
                        if (attr.name == "Add")
                        {
                            attr.GetComponent<Button>().interactable = false;
                        }
                    }
                        i++;
                }
            }
        }

        foreach (Transform t in trs)
        {
            
            if (t.name == "f" + buttonID)
            {
                Transform[] attrs = t.GetComponentsInChildren<Transform>();
                foreach(Transform attr in attrs)
                {
                    if(attr.name == "qty")
                    {
                        if(fighterRewards.ContainsKey(fight.fighters[buttonID - 1]))
                        {
                            fighterRewards[fight.fighters[buttonID - 1]] = fighterRewards[fight.fighters[buttonID - 1]] + 1;
                        }
                        else
                        {
                            fighterRewards.Add(fight.fighters[buttonID - 1], 1);
                        }

                        attr.GetComponent<Text>().text = fighterRewards[fight.fighters[buttonID - 1]].ToString();
                        //attr.GetComponent<Text>().text = 
                    }
                   
                    if(attr.name == "Sub")
                    {
                        if (rewardLeft > 0)
                        {
                            attr.GetComponent<Button>().interactable = true;
                        }
                    }
                }

            }

            if (t.name == "RewardText")
            {
                
                t.GetComponent<Text>().text = "Rewards Left: " + rewardLeft.ToString();
            }

        }
    }

    public void decrementReward(int buttonID)
    {
        Transform[] trs = distributeReward.GetComponentsInChildren<Transform>();
        rewardLeft++;
        if (rewardLeft > 0)
        {
            //set all add buttons to interactable
            int i = 1;
            foreach (Transform t in trs)
            {
                if (t.name == "f" + i)
                {
                    Transform[] attrs = t.GetComponentsInChildren<Transform>();
                    foreach (Transform attr in attrs)
                    {
                        if (attr.name == "Add")
                        {
                            attr.GetComponent<Button>().interactable = true;
                        }
                    }
                    i++;
                }
            }
        }
        foreach (Transform t in trs)
        {

            if (t.name == "f" + buttonID)
            {
                Transform[] attrs = t.GetComponentsInChildren<Transform>();
                foreach (Transform attr in attrs)
                {
                    if (attr.name == "qty")
                    {
                        if (fighterRewards.ContainsKey(fight.fighters[buttonID - 1]))
                        {
                            fighterRewards[fight.fighters[buttonID - 1]] = fighterRewards[fight.fighters[buttonID - 1]] - 1;
                        }
                        else
                        {
                            fighterRewards.Add(fight.fighters[buttonID - 1], 1);
                        }

                        attr.GetComponent<Text>().text = fighterRewards[fight.fighters[buttonID - 1]].ToString();
                        //attr.GetComponent<Text>().text = 
                    }


                    if (attr.name == "Sub")
                    {
                        //can't go lower than 0
                        if (fighterRewards[fight.fighters[buttonID - 1]] == 0)
                        {
                            attr.GetComponent<Button>().interactable = false;
                        }
                    }
                }

            }

            if (t.name == "RewardText")
            {

                t.GetComponent<Text>().text = "Rewards Left: " + rewardLeft.ToString();
            }

        }
    }

    public void doneDistributing()
    {
        //will need to reset it
        distributeReward.gameObject.SetActive(false);
        //rewardScreen.gameObject.SetActive(true);
        //Game.sendAction(
        Game.sendAction(new DistributeReward(fight.fighters, fighterRewards));//calls distribute response

    }


    public void endBattle_collab(int outcome)
    {

        
        if (outcome == 1)
        {
            //hero wins
            //get reward


            Game.gameState.removeMonster(fight.monster);
            Game.sendAction(new WinBattle(fight.fighters, fight.fighters[0])); //calls distributeOrWait

        }
        else if (outcome == 2)
        {
            fight.monster.recover();
            //end turn
            Game.gameState.turnManager.passTurn();

            battleEndScreen.gameObject.SetActive(false);
            closeFightScreen();
        }
        else if (outcome == 0)
        {
            //draw or hero clicked end battle
            fight.monster.recover();
            battleEndScreen.gameObject.SetActive(true);
            Transform[] trs = battleEndScreen.GetComponentsInChildren<Transform>();
            foreach (Transform t in trs)
            {
                if (t.name == "Body")
                {
                    t.GetComponent<Text>().text = "No time left. Draw!";
                }
            }

        }
        else
        {
            //monster wins
            Hero h = Game.gameState.getPlayer(fight.getCurrentFighter()).getHero();
            if (h.getStrength() > 0)
            {
                //lose strength point
                h.decreaseStrength(1);
            }

            h.increaseWillpower(3);
            battleEndScreen.gameObject.SetActive(true);
            Transform[] trs = battleEndScreen.GetComponentsInChildren<Transform>();
            foreach (Transform t in trs)
            {
                if (t.name == "Body")
                {
                    t.GetComponent<Text>().text = "Creature wins. You lose 1 strength point.";
                }
            }


        }
    }

    public Dictionary<string, bool> heroGotReward = new Dictionary<string, bool>();

    public void getReward(string type)
    {
        if(fightType == 0)
        {
            int reward = fight.monster.getReward();
            Hero h = Game.gameState.getPlayer(fight.getCurrentFighter()).getHero();
            if (type.Equals("gold"))
            {
                h.increaseGold(reward);
            }

            if (type.Equals("willpower"))
            {
                h.increaseWillpower(reward);
            }
            string[] players = fight.fighters;
            Game.sendAction(new EndFight(players));
        }
        else
        {
            distributeReward.gameObject.SetActive(false);
            string[] players = new string[1];
            players[0] = Game.myPlayer.getNetworkID();
            involvedPlayers.Remove(players[0]);
            Debug.Log(Game.gameState.getPlayer(players[0]).getHeroType() + " is getting reward " + type);
            //Hero h = Game.gameState.getPlayer(players[0]).getHero();
            //if (type == "gold")
            //{
            //    Debug.Log("GETTING MY gold");
            //    h.increaseGold(fighterRewards[players[0]]);
            //}

            //if (type == "willpower")
            //{
            //    Debug.Log("GETTING MY willpower");
            //    h.increaseWillpower(fighterRewards[players[0]]);
            //}
            //Debug.Log("Before removing from fighter rewards" + fighterRewards.Count);
            //fighterRewards.Remove(players[0]);
            //Debug.Log("After removing from fighter rewards" + fighterRewards.Count);
            Game.sendAction(new GetMyReward(players, fighterRewards, type));

            if (fighterRewards.Count == 0)
            {
                Game.sendAction(new EndFight(involvedPlayers.ToArray()));
            }
            closeFightScreen();

        }
        
    }

    public void okClick()
    {
        //fightOverAction();
        string[] players = fight.fighters;
        if(fightType == 0)
        {
            Game.sendAction(new EndFight(players));
        }
        else
        {
            leaveBattleClick();
        }
        
    }

    public void fightOverAction()
    {
        //end turn
        
        closeFightScreen();
        rewardScreen.gameObject.SetActive(false);
        battleEndScreen.gameObject.SetActive(false);
    }

    public void startCollabFight(Fight f)
    {
        
     
        fight = f;
        //set the screen active
        Debug.Log("NUm fighters " + fight.fighters.Length);
        //clear fight screen contents
        battleValue.text = "Battle Value";
        fightLobby2.gameObject.SetActive(false);
        selectedChoiceText.text = "";
        monsterBattleValueText.text = "Battle Value: ";
        monsterBattleValue = 0;
        header.text = "Fight";
        Transform[] trs = fightScreen.GetComponentsInChildren<Transform>();
        foreach (Transform t in trs)
        {
            if (t.name == "MonsterDiceRolls")
            {


                t.GetComponent<Text>().text = "";
            }
            if (t.name == "MonsterFinalOutcome")
            {
                t.GetComponent<Text>().text = "";
            }
        }

        nextRoundPlayers.Clear();
        Debug.Log("involved players " + involvedPlayers.Count);
        Debug.Log("nextround players " + nextRoundPlayers.Count);
        nextRoundButton.gameObject.SetActive(false);
        endBattleButton.gameObject.SetActive(false);

        foreach (string p in fight.fighters)
        {
            if(Game.myPlayer.getNetworkID() == p)
            {
                //display fight screen
                fightScreen.gameObject.SetActive(true);
            }
            
        }
        //set the roll dice active and tell player it is their turn
        
        if (Game.myPlayer.getNetworkID() == fight.getCurrentFighter())
        {
            rollButtonActive(true);
            header.text = "Your turn to fight";
        }
        else
        {
            rollButtonActive(false);
            header.text = fight.currentFighterHero().getHeroType() + " is fighting.";
        }
        //advance time tracker
        
        foreach (Hero h in fight.getHeroes())
        {
            h.setHour(1 + h.getHour());
            GameController.instance.setTime(fight.getCurrentFighter(), h.getHour());
            
        }

        displayHero(Game.gameState.getPlayer(fight.getCurrentFighter()));
        displayMonster(f.monster);
        displayBattleValue(0);
        
       

    }

    public void displayHero(Andor.Player player)
    {
        
        GameObject heroGameObject = GameObject.Find("Hero");
        Transform[] trs = heroGameObject.GetComponentsInChildren<Transform>();
        
        foreach (Transform attr in trs)
        {
            
            attr.gameObject.SetActive(true);
            if (attr.name == "Name")
            {

                Text heroname = attr.GetComponent<Text>();
                heroname.text = player.getHeroType();
            }
            if (attr.name == "Image")
            {
                
                Sprite herosprite = Resources.Load<Sprite>("PlayerSprites/" + player.getHeroType());
                attr.GetComponent<Image>().sprite = herosprite;
                attr.GetComponent<Image>().useSpriteMesh = true;
            }
            if (attr.name == "Attributes")
            {
               
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
                
                Sprite monsterSprite = Resources.Load<Sprite>("UI/" + m.getMonsterType());
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


    public void nextRound_collab()
    {
        
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
        Game.sendAction(new RespondFight(players, true, true)); //calls addHostPLayer
        fightLobby.gameObject.SetActive(true);
        updateFightLobby();
        startFight.gameObject.SetActive(true);
    }

    public void addHostPlayer(string player)
    {
        involvedPlayers.Clear();
        involvedPlayers.Add(player);
        //Debug.Log("Adding host players: " + involvedPlayers.Count);
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

    

    public void joinFightLobby2(string fighter)
    {
        respondToNextRound(fighter);
        Debug.Log(Game.gameState.getPlayer(fighter).getHeroType() + " joining fight lobby");
        nextRoundPlayers.Add(fighter);
        updateFightLobby2();
        //fightLobby2.gameObject.SetActive(true);
        //updateFightLobby();
        if (allResponded2())
        {
            
            //fightLobby2.gameObject.SetActive(false);
            //make sure to reset all the fight UI
            Game.sendAction(new StartFight(nextRoundPlayers.ToArray(), 1));
            //involvedPlayers = nextRoundPlayers;

        }
        
        
        //involvedPlayers.Add(fighter);
        
        

    }

    public void leaveBattleClick()
    {
        string[] players = new string[1];
        players[0] = Game.myPlayer.getNetworkID();
        Game.sendAction(new JoinNextRound(players, false));//calls leave battle execute
        
        closeFightScreen();
    }

    public void leaveBattleExecute(string player)
    {
        respondToNextRound(player);
        involvedPlayers.Remove(player);
        if(involvedPlayers.Count == 0)
        {
            Game.sendAction(new EndFight(fight.fighters));
        }
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

    private void updateFightLobby2()
    {
        int i = 1;
        foreach (string fighter in nextRoundPlayers)
        {
            displayPlayerInFightLobby2(fighter, i);
            i++;
        }
    }

    public void respondToFight(string player)
    {
        playerResponded[player] = true;
    }

    public void respondToNextRound(string player)
    {
        playerRespondNextRound[player] = true;
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

    public bool allResponded2()
    {
        Debug.Log("TEsting all responded " + playerRespondNextRound.Count);
        foreach (bool r in playerRespondNextRound.Values)
        {
            if (r == false)
            {
                return false;
            }
        }

        return playerRespondNextRound.Values.Count > 0;
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

    private void displayPlayerInFightLobby2(string player, int i)
    {
        Transform[] trs = fightLobby2.GetComponentsInChildren<Transform>(true);
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
        
        fightChoice.gameObject.SetActive(false);
        selectHeroFight.gameObject.SetActive(false);
        fightScreen.gameObject.SetActive(false);
        availablePlayers.Clear();
        invitedPlayers.Clear();
        involvedPlayers.Clear();
        playerResponded.Clear();
        battleValue.text = "Battle Value";
        playerRespondNextRound.Clear();
        startFight.gameObject.SetActive(false);
        clearDistributeScreen();

        selectedChoiceText.text = "";
        monsterBattleValueText.text = "Battle Value: ";
        monsterBattleValue = 0;
        header.text = "Fight";

       

        rollButtonActive(true);
        fightLobby.gameObject.SetActive(false);
        rewardScreen.gameObject.SetActive(false);
        waitingScreen.gameObject.SetActive(false);
        fight = null;

    }

    /////////////////////////////////////////////////////////////////////////////
    /////////////////////////////////////////////////////////////////////////////

   

    IEnumerator articleroutine(string message,int sleep)
    {
        updateRollText.GetComponent<Text>().text = message;
        updateRollArticle.SetActive(true);
        yield return new WaitForSeconds(sleep);
        //After we have waited 5 seconds print the time again.
        Debug.Log("Finished Coroutine at timestamp : " + Time.time);
         updateRollArticle.SetActive(false);
    }
    

     public void useHelmInFight()
    {
        Game.sendAction(new UseHelm(Game.myPlayer.getNetworkID()));
        chooseArticleScroll.SetActive(false);
        if(Game.myPlayer.getHeroType() == "Male Archer" || Game.myPlayer.getHeroType() == "Female Archer")
        {
            //does nothing
        }
        if (Game.myPlayer.getHeroType() == "Male Dwarf" || Game.myPlayer.getHeroType() == "Female Dward")
        {
            string update = "You have used the helm! Your roll has been updated from: " + heroFinalRoll + " to " + rolledDiceHelm;
            //displayUpdatedRoll(update);
            StartCoroutine(articleroutine(update,4));
            displayFinalOutcome(rolledDiceHelm);
        }
        if (Game.myPlayer.getHeroType() == "Male Warrior" || Game.myPlayer.getHeroType() == "Female Warrior")
        {
            string update = "You have used the helm! Your roll has been updated from: " + heroFinalRoll + " to " + rolledDiceHelm;
            //displayUpdatedRoll(update);
            StartCoroutine(articleroutine(update,4));
            displayFinalOutcome(rolledDiceHelm);
        }
        if (Game.myPlayer.getHeroType() == "Male Wizard" || Game.myPlayer.getHeroType() == "Female Wizard")
        {
            //does nothing
        }
        rollCreature();
    }

    public void useWitchBrewInFight()
    {

        Game.sendAction(new UseWitchBrew(Game.myPlayer.getNetworkID()));
        chooseArticleScroll.SetActive(false);
        if(Game.myPlayer.getHeroType() == "Male Archer" || Game.myPlayer.getHeroType() == "Female Archer")
        {
            string update = "You have used the witch's brew! Your roll has been updated from: " + archerRoll + " to " + archerRollBrew;
            StartCoroutine(articleroutine(update,4));
            displayFinalOutcome(archerRollBrew);
        }
        if (Game.myPlayer.getHeroType() == "Male Dwarf" || Game.myPlayer.getHeroType() == "Female Dward")
        {
            string update = "You have used the witch's brew! Your roll has been updated from: " + heroFinalRoll + " to " + rolledDiceBrew;
            //displayUpdatedRoll(update);
            StartCoroutine(articleroutine(update,4));
            displayFinalOutcome(rolledDiceBrew);
        }
        if (Game.myPlayer.getHeroType() == "Male Warrior" || Game.myPlayer.getHeroType() == "Female Warrior")
        {
            string update = "You have used the witch's brew! Your roll has been updated from: " + heroFinalRoll + " to " + rolledDiceBrew;
            //displayUpdatedRoll(update);
            StartCoroutine(articleroutine(update,4));
            displayFinalOutcome(rolledDiceBrew);
        }
        if (Game.myPlayer.getHeroType() == "Male Wizard" || Game.myPlayer.getHeroType() == "Female Wizard")
        {
            string update = "You have used the witch's brew! Your roll has been updated from: " + heroFinalRoll + " to " + wizardRollBrew;
            StartCoroutine(articleroutine(update,4));
            displayFinalOutcome(wizardRollBrew);
            //does nothing
        }
        rollCreature();
    }
    public void useBowInFight()
    {
        Game.sendAction(new UseBow(Game.myPlayer.getNetworkID()));
        chooseArticleScroll.SetActive(false);
        rollCreature();
    }
    public void onClickNoArticle()
    {
        Game.myPlayer.getHero().selectedArticle = true;
        chooseArticleScroll.SetActive(false);
        rollCreature();
    }


}

