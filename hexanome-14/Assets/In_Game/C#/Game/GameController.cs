using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;
using System.Reflection;

public class GameController : MonoBehaviour
{

    public static GameController instance;

    public Transform gameContainer;
    public Transform pauseMenuContainer;
    public Transform saveGameContainer;

    public Transform tradeRequest;
    public Transform tradeScreenController;
    public Transform notification;
    public Transform merchantScreenController;
    public Transform fightScreenController;
    

    public Transform fightRequest;
   
    //public Transform merchantScreenController;

    public Transform heroInfoScreen;

    public Transform boardSpriteContainer;
    public Transform playerContainer;
    public Transform playerTimeContainer;
    public Transform legendTrackContainer;
    public Transform monsterContainer;
    public Transform heroInfoContainer;
    public Transform pickDropContainer;

    public Button moveButton;
    public Button fightButton;
    public Button movePrinceButton;
    public Button emptyWellButton;
    public Button buyBrewButton;
    public Button chatButton;
    public Button closeChatButton;
    public Button dropPickButton;
    //public Button wineskinButton;
    public GameObject wineskinDropdown;
    public GameObject wineskin;
    public GameObject useFalcon;

    public Transform merchantButton;


    public Text chatText;
    public GameObject chat;
    public Text input;

    //Article
    public Button useTelescope;

    //FogToken
    public GameObject willpower2Token;
    public GameObject willpower3Token;
    public GameObject brewToken;
    public GameObject gold1Token;
    public GameObject gorToken;
    public GameObject wineskinToken;
    public GameObject eventToken;
    public GameObject strengthToken;
    public GameObject tower;



    public Text turnLabel;
    //public Text scrollText;
    public Text scrollTxt;
    public Text gameConsoleText;
    public Text shieldCountText;
    public Text dayCountText;
    public Text witchText;
    public Text heroStatsText;


    public GameObject emptyPrefab;
    public GameObject playerPrefab;
    public Sprite fullBoardSprite;
    public GameObject circlePrefab;
    public GameObject heroInfoPrefab;
    public GameObject well_front;
    public GameObject fog;
    //public GameObject scroll;
    public GameObject scroll;
    public GameObject prince;
    public GameObject farmer;
    public GameObject medicinalHerb3;
    public GameObject witch;
    public GameObject gold1;

    public GameObject narrator;
    public Dictionary<int, GameObject> Narrator;

    public Dictionary<int, BoardPosition> tiles;
    public Dictionary<string, GameObject> playerObjects;
    public Dictionary<string, GameObject> timeObjects;
    public Dictionary<int, Bounds> timeTileBounds;
    public Dictionary<int, Bounds> legendTiles;

    public Bounds timeObjectBounds;
    public Dictionary<string, Vector3> rndPosInTimeBox;
    public Dictionary<Monster, GameObject> monsterObjects;
    //public PrinceThorald princeThor;
    public Dictionary<PrinceThorald, GameObject> princeThoraldObject;
    public Dictionary<MedicinalHerb, GameObject> medicinalHerbObject;
    public GameObject towerSkral;

    //private int[] event_cards = { 2, 11, 13, 14, 17, 24, 28, 31, 32, 1 };
    //private string[] fogTokens = {"event", "strength", "willpower3", "willpower2", "brew",
    //        "wineskin", "gor", "event", "gor", "gold1", "gold1", "gold1", "event", "event", "event"};

    private bool pauseMenuActive = false;
    private bool moveSelected = false;
    private bool movePrinceSelected = false;


    private bool tradeRequestSent = false;
    private string playerTradeTo; //these could belong to
    private string playerTradeFrom;
    private string tradeMsg;

    private string notifMsg;
    private float notifTime;
    private string notifUser;

    //private int tradeTypeIndex = -1;
    public static TradeScreen ts;
    private bool notificationOn = false;
    public static MerchantScreen ms;

    public FightScreenController fsc;

    private Transform initTransform;
    //private string[] tradeType;
    //private string[] players;

    private int[] event_cards2;
    private string[] fogTokens2;
    private string[] playersToNotify;
    public string chatMessages;


    public bool easy = true;

    private string[] invitedFighters;
    private bool fightRequestSent;
    private bool fightActive;

    // Start is called before the first frame update
    void Start()
    {
        ts = tradeScreenController.gameObject.GetComponent<TradeScreen>();
        fsc = fightScreenController.gameObject.GetComponent<FightScreenController>();
        //ms = merchantScreenController.gameObject.GetComponent<MerchantScreen>();
        playersToNotify = new string[4];
        //ts = new TradeScreen();
        //this.tradeType = new string[3];
        //this.players = new string[2];
        Game.started = true;
        Game.createPV();

        tiles = new Dictionary<int, BoardPosition>();
        playerObjects = new Dictionary<string, GameObject>();
        timeObjects = new Dictionary<string, GameObject>();
        Narrator = new Dictionary<int, GameObject>();
        timeTileBounds = new Dictionary<int, Bounds>();
        legendTiles = new Dictionary<int, Bounds>();
        rndPosInTimeBox = new Dictionary<string, Vector3>();
        monsterObjects = new Dictionary<Monster, GameObject>();
        princeThoraldObject = new Dictionary<PrinceThorald, GameObject>();
        medicinalHerbObject = new Dictionary<MedicinalHerb,GameObject>();
        initTransform = transform;

        instance = this;

        movePrinceButton.interactable = false;

        // For drawing everything
        loadBoard();

        // For setting up resource distribution 

        // Set up Turn Manager
        if (PhotonNetwork.IsMasterClient)
        {
            List<Andor.Player> randomOrder = Game.getGame().getPlayers();
            Game.Shuffle(randomOrder);
            Game.setTurnManager(randomOrder);
            Debug.Log("SET TURN");
           

        }


        GameSetup();
        int timeout1 = 300;
        int timeout2 = 1000;
        int timeout3 = 1000;
        if (Game.gameState != null)
        {
            while (Game.gameState.turnManager == null)
            {
                StartCoroutine(Game.sleep(0.01f));
                if (timeout1 <= 0)
                {
                    throw new Exception("Could not initialize TurnManager!");
                }
                timeout1--;
            }
            Debug.Log(Game.gameState.turnManager.currentPlayerTurn());

            turnLabel.text = Game.gameState.turnManager.currentPlayerTurn();
            if (Game.gameState.turnManager.currentPlayerTurn().Equals(Game.myPlayer.getNetworkID()))
            {
                turnLabel.color = Game.myPlayer.getColor();
            }
            else
            {
                turnLabel.color = UnityEngine.Color.black;
            }
            while (Game.gameState.fogtoken_order == null)
            {
                StartCoroutine(Game.sleep(0.01f));
                if (timeout3 <= 0)
                {
                    throw new Exception("Could not initialize fog token!");
                }
                timeout3--;
            }
        }
    }

    public void updateHeroStats()
    {
        //string update = Game.myPlayer.getHeroType()
        //    + "\nGold: " + Game.myPlayer.getHero().getGold().ToString()
        //    + "\nStrength: " + Game.myPlayer.getHero().getStrength().ToString()
        //    + "\nWillpower: " + Game.myPlayer.getHero().getWillpower().ToString()
        //    + "\nHour: " + Game.myPlayer.getHero().getHour().ToString()
        //    + "\nArticles: " + Game.myPlayer.getHero().allArticlesAsString();

        //heroStatsText.text = update;
        string update = "";
        foreach (Andor.Player p in Game.gameState.getPlayers())
        {
            string text = p.getHeroType() + " hour: " + p.getHero().getHour() + " , gold: " + p.getHero().getGold() + " , will: " + p.getHero().getWillpower() + " ,strength: " + p.getHero().getStrength() + " " + p.getHero().allArticlesAsString() + "\n" + "\n";
            update += text;

        }
        //string update = Game.myPlayer.getHeroType()
        //   + "\nG: " + Game.myPlayer.getHero().getGold().ToString()
        //   + "\nStrength: " + Game.myPlayer.getHero().getStrength().ToString()
        //   + "\nWillpower: " + Game.myPlayer.getHero().getWillpower().ToString()
        //   + "\nHour: " + Game.myPlayer.getHero().getHour().ToString()
        //   + "\nArticles: " + Game.myPlayer.getHero().allArticlesAsString();

        heroStatsText.text = update;
    }

    public bool checkFalconUse()
    {
        return ts.usingFalcon;
    }
   

    void Update()
    {
      
        chatText.text = chatMessages;

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (pauseMenuActive)
            { 
                this.removePauseMenu();
            }
            else
            {
                this.displayPauseMenu();
            }
        }
        if (Game.gameState != null)
        {

            // Update Player position
            foreach (Andor.Player player in Game.gameState.getPlayers())
            {
                playerObjects[player.getNetworkID()].transform.position =
                    moveTowards(playerObjects[player.getNetworkID()].transform.position, tiles[Game.gameState.playerLocations[player.getNetworkID()]].getMiddle(), 0.5f);

                timeObjects[player.getNetworkID()].transform.position =
                    moveTowards(timeObjects[player.getNetworkID()].transform.position, rndPosInTimeBox[player.getNetworkID()], 1);

            }

            int loc = Game.gameState.getPlayerLocations()[Game.myPlayer.getNetworkID()];
            bool wellValid = false;
            foreach (Well w in Game.gameState.getWells().Keys)
            {
                if (w.getLocation() == loc && !w.used)
                {
                    GameController.instance.emptyWellButton.gameObject.SetActive(true);
                    wellValid = true;
                }

            }

            if (!wellValid)
            {
                GameController.instance.emptyWellButton.gameObject.SetActive(false);
            }
            //check if player has telescope
            if (Game.myPlayer.getHero().allArticlesAsStringList().Contains("Telescope"))
            {
                GameController.instance.useTelescope.gameObject.SetActive(true);
            }
            else
            {
                GameController.instance.useTelescope.gameObject.SetActive(false);
            }

            bool brewValid = false;
            if (loc == Game.gameState.witchLocation && Game.gameState.witchLocation != -1)
            {
                GameController.instance.buyBrewButton.gameObject.SetActive(true);
                brewValid = true;
            }
            if (!brewValid)
            {
                GameController.instance.buyBrewButton.gameObject.SetActive(false);
            }
            if (Game.myPlayer.getHero().hasArticle("Wineskin"))
            {
                GameController.instance.wineskin.gameObject.SetActive(true);

            }
            else
            {
                GameController.instance.wineskin.gameObject.SetActive(false);
            }

            bool validFalcon = false;
            //check for falcon
            if (Game.myPlayer.getHero().hasArticle("Falcon"))
            {
                foreach (Falcon f in Game.myPlayer.getHero().heroArticles["Falcon"])
                {
                    if (!f.checkUsedToday())
                    {
                        GameController.instance.useFalcon.gameObject.SetActive(true);
                        validFalcon = true;
                        break;

                    }
                }

            }
            if (!validFalcon)
            {
                GameController.instance.useFalcon.gameObject.SetActive(false);
                //Debug.Log("removing falcon");
            }


            // Update Player position
            foreach (Monster monster in Game.gameState.getMonsters())
            {
                if (monster.canMonsterMove())
                {
                    monsterObjects[monster].transform.position =
                    moveTowards(monsterObjects[monster].transform.position, tiles[monster.getLocation()].getMiddle(), 0.5f);
                }
                if (monster.isMedicinalGor())
                {
                     medicinalHerbObject[Game.gameState.getMedicinalHerb()].transform.position = moveTowards(monsterObjects[monster].transform.position, tiles[monster.getLocation()].getMiddle(), 0.5f);
                }
                
            }
            //foreach (PrinceThorald princeT in Game.gameState.getPrinceThorald())
            //{
            //    princeThoraldObject[princeT].transform.position = moveTowards(princeThoraldObject[princeT].transform.position, tiles[princeT.getLocation()].getMiddle(), 0.5f);

            //}
            // Update player turn
            //turnLabel.text = Game.gameState.turnManager.currentPlayerTurn();
            if (Game.gameState.turnManager.currentPlayerTurn().Equals(Game.myPlayer.getNetworkID()))
            {
                turnLabel.color = Game.myPlayer.getColor(130);
            }
            else
            {
                turnLabel.color = UnityEngine.Color.black;
            }

            updateHeroStats();

            if (winScenario() && Game.gameState.outcome == "won")
            {
                Game.gameState.outcome = "wonNotified";
                winNotify();
            }

            if (tradeRequestSent)
            {

                if (Game.myPlayer.getNetworkID().Equals(playerTradeTo))
                {
                    processTradeRequest();
                }
            }

            //if (ts.tradeType[0] != "")
            //{
            //    Debug.Log("trade type " + ts.tradeType[0]);
            //}

            if (notificationOn)
            {
                notify();
                notifTime -= Time.deltaTime;
            }


            bool onMerchant = false;

            foreach (int merchantLoc in Game.gameState.getMerchants().Keys)
            {
                if (Game.gameState.getPlayerLocations()[Game.myPlayer.getNetworkID()] == merchantLoc)
                {
                    onMerchant = true;
                    updateGameConsoleText("You've landed on the same space as a merchant. Click the merchatn button to buy articles");

                }
            }
            merchantButton.gameObject.SetActive(onMerchant);

            //if(Game.gameState.playerLocations[Game.myPlayer.getNetworkID()])
            foreach (string player in playersToNotify)
            {
                if (Game.myPlayer.getNetworkID() == player)
                {
                    updateGameConsoleText(this.gameConsoleText.text.ToString());
                }
            }

            if (fightRequestSent)
            {

                foreach (string p in invitedFighters)
                {

                    if (!Game.myPlayer.getNetworkID().Equals(invitedFighters[0]))
                    {
                        processFightRequest(true);
                    }
                    else
                    {
                        processFightRequest(false);
                       
                    }
                    
                }

            }

            //bool currentlyFighting = true;

            if (fightActive)
            {
                if (fsc.fight != null)
                {
                    foreach(Andor.Player p in Game.gameState.getPlayers())
                    {
                        if(!Array.Exists(fsc.fight.fighters, element => element == p.getNetworkID()))
                        {
                            fsc.fightScreen.gameObject.SetActive(false);
                        }
                    }
                }
            }

            
        }

        if (invitedFighters != null)
        {
            if (Game.myPlayer.getNetworkID().Equals(invitedFighters[0])
            && fsc.allResponded())
            {
                fsc.fightReady();
            }
        }
        

        bool canFight = false;
        if (Game.myPlayer.getNetworkID().Equals(Game.gameState.turnManager.currentPlayerTurn())){
            //check if player is on the same space as a monster
            
            int myLocation = Game.gameState.getPlayerLocations()[Game.myPlayer.getNetworkID()];
            
            foreach (Monster m in Game.gameState.getMonsters())
            {
                
                int monsterLoc = m.getLocation();
                
                if (monsterLoc == myLocation)
                {
                    canFight = true;
                }
                else
                {
                    if (Game.myPlayer.getHero().hasArticle("Bow"))
                    {
                        List<Node> neighbours = Game.gameState.positionGraph.getNode(myLocation).getAdjacentNodes();
                        foreach (Node n in neighbours)
                        {
                            if (monsterLoc == n.getIndex())
                            {
                                canFight = true;
                            }
                        }
                    }
                }
            }
        }
        fightButton.interactable = canFight;
    }

    public void moveToNewPos(Andor.Player player)
    {
        Vector3 playerPos = playerObjects[player.getNetworkID()].transform.position;
        Vector3 cellPos = tiles[Game.gameState.playerLocations[player.getNetworkID()]].getMiddle();
        playerObjects[player.getNetworkID()].transform.position = moveTowards(playerPos, cellPos, 0.5f);
    }


    private void loadBoard()
    {
        boardSpriteContainer.gameObject.GetComponent<Image>().color = new UnityEngine.Color(0, 0, 0, 0);
        // load background board
        Vector3 boardContainerPos = new Vector3(boardSpriteContainer.position.x - boardSpriteContainer.parent.position.x,
            boardSpriteContainer.position.y - boardSpriteContainer.parent.position.y, 35- boardSpriteContainer.parent.lossyScale.z);
        Vector3 boardContainerScaling = new Vector3(1 / boardSpriteContainer.parent.lossyScale.x, 1 / boardSpriteContainer.parent.lossyScale.y, 1 / boardSpriteContainer.parent.lossyScale.z);

        GameObject fullBoard = Instantiate(emptyPrefab, boardContainerPos, boardSpriteContainer.transform.rotation, boardSpriteContainer);
        fullBoard.name = "full-Board";

        fullBoard.AddComponent<SpriteRenderer>();
        SpriteRenderer spriteRenderer = fullBoard.GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = fullBoardSprite;

        fullBoard.transform.localScale = boardContainerScaling;
        //fullBoard.transform.position = Vector3.Scale(fullBoard.transform.position, new Vector3(1,1,0));

        Debug.Log(boardSpriteContainer.position);
        Debug.Log(boardSpriteContainer.parent.position);
        Debug.Log(boardContainerPos);


        // load sprites
        Sprite[] sprites = Resources.LoadAll<Sprite>("BoardSprites");
        // Requirement: have Resources/Sprites folder under Assets
        if (sprites == null)
            print("Could not load board tile sprites");

        foreach (Sprite sprite in sprites)
        {
            createBoardPosition(sprite, boardContainerPos, boardContainerScaling);
        }

        sprites = Resources.LoadAll<Sprite>("TimeSprites");
        // Requirement: have Resources/Sprites folder under Assets
        if (sprites == null)
            print("Could not load time tile sprites");

        foreach (Sprite sprite in sprites)
        {
            GameObject temp = Instantiate(emptyPrefab, boardContainerPos, boardSpriteContainer.transform.rotation, playerTimeContainer);
            temp.AddComponent<SpriteRenderer>().sprite = sprite;

            TileBounds tb = new TileBounds(temp.AddComponent<PolygonCollider2D>(), boardSpriteContainer);
            Bounds b = tb.createBounds();

            timeTileBounds.Add(Int32.Parse(sprite.name.Split('-')[1]), b);
            Destroy(temp);
        }

        sprites = Resources.LoadAll<Sprite>("LegendTrack");
        // Requirement: have Resources/Sprites folder under Assets
        if (sprites == null)
            print("Could not load Legend Track sprites");

        foreach (Sprite sprite in sprites)
        {
            GameObject temp = Instantiate(emptyPrefab, boardContainerPos, boardSpriteContainer.transform.rotation, legendTrackContainer);
            temp.AddComponent<SpriteRenderer>().sprite = sprite;

            TileBounds tb = new TileBounds(temp.AddComponent<PolygonCollider2D>(), boardSpriteContainer);
            Bounds b = tb.createBounds();

            legendTiles.Add(Int32.Parse(sprite.name.Split('-')[1]), b);
            Destroy(temp);
        }
    }

    private void createBoardPosition(Sprite sprite, Vector3 pos, Vector3 scaling)
    {
        int cellNumber = Int32.Parse(sprite.name.Split('_')[1]);

        GameObject cellObject = Instantiate(emptyPrefab, pos, transform.rotation, boardSpriteContainer);
        //cellObject.transform.localScale = boardSpriteContainer.transform.localScale;
        cellObject.tag = cellNumber.ToString();
        cellObject.name = "position-" + cellNumber.ToString();

        cellObject.transform.localScale = scaling;


        cellObject.AddComponent<SpriteRenderer>();
        SpriteRenderer spriteRenderer = cellObject.GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = sprite;


        cellObject.AddComponent<LineRenderer>();
        cellObject.AddComponent<PolygonCollider2D>();
        cellObject.AddComponent<BoardPosition>();

        BoardPosition boardPosition = cellObject.GetComponent<BoardPosition>();
        boardPosition.init(cellNumber);
        tiles.Add(cellNumber, boardPosition);
        
    }

    public void sendChat()
    {
        Debug.Log("chat button clicked");
        string message = input.text;
        Game.sendAction(new SendChat(message, Game.myPlayer.getNetworkID(), PhotonNetwork.LocalPlayer.NickName));
    }


    public void wellClick()
    {
        Game.sendAction(new EmptyWell(Game.myPlayer.getNetworkID()));
    }

    public void buyBrewClick()
    {
        Game.sendAction(new BuyBrew(Game.myPlayer.getNetworkID()));
    }

    public void chatClick()
    {
        //Game.sendAction(new ChatOpen(Game.myPlayer.getNetworkID()));
        chat.SetActive(true);
        chatButton.gameObject.SetActive(false);
        closeChatButton.gameObject.SetActive(true);
    }

    public void  clickTelescope()
    {
        Game.sendAction(new UseTelescope(Game.myPlayer.getNetworkID()));
    }


    public void closeChatClick()
    {
        //Game.sendAction(new ChatOpen(Game.myPlayer.getNetworkID()));
        chat.SetActive(false);
        chatButton.gameObject.SetActive(true);
        closeChatButton.gameObject.SetActive(false);
    }

    public void loseScenario()
    {
        scrollTxt.text = "YOU LOST!";
        //scroll.SetActive(true);
        Game.gameState.outcome = "lost";
        StartCoroutine(overtimeCoroutine(10));
    }

    public void archerBuysBrew()
    {
        if(Game.myPlayer.getHeroType() == "Male Archer" || Game.myPlayer.getHeroType() == "Female Archer")
        {
            scrollTxt.text = "Archer pays 1 less gold for brew!";
            StartCoroutine(overtimeCoroutine(3));
        }
        
    }

    public void overtime()
    {
        if(Game.myPlayer.ToString() == Game.gameState.turnManager.currentPlayerTurn())
        {
            scrollTxt.text = "You will now lose +" + Game.gameState.TIME_overtimeCost+ " willpower points for each additional hour!";
            StartCoroutine(overtimeCoroutine(3));
        }       
    }

    public void cannotFinishMove()
    {
        scrollTxt.text = "You do not have enough willpower points to finish your move! Please end your day!";
        StartCoroutine(overtimeCoroutine(3));
    }

    public void updateChatText(string all_messages)
    {
        //chatText.text = all_messages;
        chatMessages = all_messages;
        if (chatButton.IsActive())
        {
            scrollTxt.text = "New message in Chat!";
            StartCoroutine(overtimeCoroutine(2));
        }
    }
    public void wineskinClicked()
    {
        Game.myPlayer.getHero().selectedWineskin = true;
        Debug.Log("selected wineskin");
    }
    public void buttonIsClicked()
    {
        Debug.Log("chat button clicked");
        string message = input.text;
        Game.sendAction(new SendChat(message, Game.myPlayer.getNetworkID(), PhotonNetwork.LocalPlayer.NickName));
        //Debug.Log("got the input");
        //object[] data = { message, photonView.ViewID, PhotonNetwork.LocalPlayer.NickName };
        //Debug.Log("sent the data");
        //PhotonNetwork.RaiseEvent((byte)53, data, sendToAllOptions, SendOptions.SendReliable);
    }
    public void foundWitch(int loc)
    {
        //instantiateTheWitch here
        scrollTxt.text = "You have found Reka the witch! " + Game.gameState.turnManager.currentPlayerTurn() + " will recieve a free brew!";
        StartCoroutine(overtimeCoroutine(5));
        //instantiate witch
        Debug.Log("Added witch at position: " + loc);
        //ned to make this true everywhere
        Game.gameState.witchFound = true;
        //
        GameObject wellObject = Instantiate(witch, tiles[loc].getMiddle(), transform.rotation);
        //Well w = new Well(Game.positionGraph.getNode(pos), wellObject);
        //Debug.Log(w);
        //Debug.Log(w.getLocation());
        //Game.gameState.addWell(w);


        //if it is my player, then get roll
        //instantiateMedicinalHerb(roll)
        //scrollTxt.text = "The medicinal herb is on location " + medicinalHerb.getLocation() + "!");
        //Game.gameState.foundWitch
        //Game.gameState.brewCost
    }
    IEnumerator overtimeCoroutine(int sleep)
    {
        //Print the time of when the function is first called.
        //Debug.Log("Started Coroutine at timestamp : " + Time.time);
        instance.scroll.SetActive(true);

        //yield on a new YieldInstruction that waits for 5 seconds.
        yield return new WaitForSeconds(sleep);
        instance.scroll.SetActive(false);

        //After we have waited 5 seconds print the time again.
        Debug.Log("Finished Coroutine at timestamp : " + Time.time);
    }

public void updateGameConsoleText(string message)
    {
           gameConsoleText.text = message;
    }

    public void updateGameConsoleText(string message, string[] players)
    {
        //foreach(string p in players)
        //{
        //    if(Game.myPlayer.getNetworkID() == p)
        //    {
        //        gameConsoleText.text = message;
        //    }
        //}
        gameConsoleText.text = message;
        playersToNotify = players;
    }


    public void winNotify()
    {
        scrollTxt.text = "Congratulations, you have successfully completed the legend!";
        StartCoroutine(overtimeCoroutine(10));
        
    }

    public void invalidTradeNotify(Andor.Player player)
    {
        if (Game.myPlayer == player)
        {
            Debug.Log("invalid trade");
            scrollTxt.text = "Invalid Trade Request!";
            StartCoroutine(overtimeCoroutine(6));
        }

    }

    public void invalidTradeNotify(String message)
    {
        scrollTxt.text = message;
        StartCoroutine(overtimeCoroutine(10));

    }

    //IEnumerator consoleCoroutine(string message)
    //{
    //    //Print the time of when the function is first called.
    //    //Debug.Log("Started Coroutine at timestamp : " + Time.time);
    //    gameConsoleText.text = message;
    //    //yield on a new YieldInstruction that waits for 5 seconds.
    //    yield return new WaitForSeconds(5);
    //    //After we have waited 5 seconds print the time again.
    //   // Debug.Log("Finished Coroutine at timestamp : " + Time.time);
    //}
    //public void updateGameConsoleText(string message, string[] players)
    //{
    //    gameConsoleText.text = message;
    //    playersToNotify = players;
    //}

    public void updateShieldCount(int shieldLeft)
    {
        shieldCountText.text = "Shields Left: " + shieldLeft.ToString();
    }

    public void updateDayCount(int day)
    {
        dayCountText.text = "Day: " + day;
    }

    public void advanceNarrator(int legend)
    {
        Narrator[0].transform.position = moveTowards(Narrator[0].transform.position, legendTiles[legend].center, 10);
    }

    public void GameSetup()
    {
        int playerCount = Game.gameState.getPlayers().Count;

        if (playerCount == 1 || playerCount == 2)
        {
            Game.gameState.maxMonstersAllowedInCastle = 3;
            Game.gameState.brewCost = 3;
        }
        else if (playerCount == 3)
        {
            Game.gameState.maxMonstersAllowedInCastle = 2;
            Game.gameState.brewCost = 4;

        }
        else if (playerCount == 4)
        {
            Game.gameState.maxMonstersAllowedInCastle = 1;
            Game.gameState.brewCost = 5;

        }
        GameController.instance.updateShieldCount(Game.gameState.maxMonstersAllowedInCastle - Game.gameState.monstersInCastle);
        GameController.instance.updateDayCount(Game.gameState.day);
        ms = merchantScreenController.gameObject.GetComponent<MerchantScreen>();
        /////////////////////////////////////////////////////////////////////

        // load players
        if (Game.gameState != null)
        {
            loadPlayers();

            loadMonsters();

            loadWells();

            loadMerchants();

            loadFogTokens();
            //Debug.Log("Finished loading fog tokens");

            loadPrinceThorald();

            loadFarmers();

            loadNarrator();

            loadSkralOnTower();

            setupEquipmentBoard();

        Debug.Log("INITIALIZING THE STRENGTH POINTS");
            initializeStrengthPoints();

            Debug.Log("INITIALIZING THE STRENGTH POINTS");
            initializeWineskin();

            Debug.Log("INITIALIZING THE MED HERB");
            instantiateMedicinalHerb(3);

        }

    }


    public void setupEquipmentBoard()
    {

        //4 shields
        List<Article> shields = new List<Article>();
        Game.gameState.equipmentBoard.Add("Shield", shields);
        for (int i = 0; i < 4; i++)
        {
            Game.gameState.equipmentBoard["Shield"].Add(new Shield());
        }

        //3 bows
        List<Article> bows = new List<Article>();
        Game.gameState.equipmentBoard.Add("Bow", bows);
        for (int i = 0; i < 3; i++)
        {
            Game.gameState.equipmentBoard["Bow"].Add(new Bow());
        }

        //2 falcon
        List<Article> falcons = new List<Article>();
        Game.gameState.equipmentBoard.Add("Falcon", falcons);
        for (int i = 0; i < 2; i++)
        {
            Game.gameState.equipmentBoard["Falcon"].Add(new Falcon());
        }

        //2 wineskin
        List<Article> wineskins = new List<Article>();
        Game.gameState.equipmentBoard.Add("Wineskin", wineskins);
        for (int i = 0; i < 2; i++)
        {
            Wineskin w = new Wineskin();
            w.numUsed = 0;
            Game.gameState.equipmentBoard["Wineskin"].Add(w);
        }

        //2 telescope
        List<Article> telescopes = new List<Article>();
        Game.gameState.equipmentBoard.Add("Telescope", telescopes);
        for (int i = 0; i < 2; i++)
        {
            Game.gameState.equipmentBoard["Telescope"].Add(new Telescope());
        }


        //3 helm
        List<Article> helms = new List<Article>();
        Game.gameState.equipmentBoard.Add("Helm", helms);
        for (int i = 0; i < 3; i++)
        {
            Game.gameState.equipmentBoard["Helm"].Add(new Helm());
        }

        List<Article> brews = new List<Article>();
        Game.gameState.equipmentBoard.Add("WitchBrew", brews);
        for (int i = 0; i < 5; i++)
        {
            Game.gameState.equipmentBoard["WitchBrew"].Add(new WitchBrew());
        }
    }

   
   

    public void monsterAtCastle(Monster monster)
    {
        // Do something now that this monster has made it to the castle
        Debug.Log("Monster in Castle!");
    }


    public void emptyWell(GameObject well)
    {
        //well.SetActive(false);
        well.GetComponent<MeshRenderer>().material.SetColor("_Color", UnityEngine.Color.grey);

    }

    private void loadPlayers()
    {
        Vector3 boardContainerScaling1 = new Vector3(1 / boardSpriteContainer.parent.lossyScale.x, 1 / boardSpriteContainer.parent.lossyScale.y, 1/ boardSpriteContainer.parent.lossyScale.z);
        Vector3 boardContainerScaling2 = new Vector3(2 / boardSpriteContainer.parent.lossyScale.x, 2 / boardSpriteContainer.parent.lossyScale.y, 2 / boardSpriteContainer.parent.lossyScale.z);
        Vector3 boardContainerScaling3 = new Vector3(3 / boardSpriteContainer.parent.lossyScale.x, 3 / boardSpriteContainer.parent.lossyScale.y, 3 / boardSpriteContainer.parent.lossyScale.z);
        Vector3 boardContainerScaling4 = new Vector3(1.5f / boardSpriteContainer.parent.lossyScale.x, 1.5f / boardSpriteContainer.parent.lossyScale.y, 1.5f / boardSpriteContainer.parent.lossyScale.z);

        foreach (Andor.Player player in Game.gameState.getPlayers())
        {
            // Player Icons
            GameObject playerObject = Instantiate(playerPrefab, playerContainer);
            playerObjects.Add(player.getNetworkID(), playerObject);
            SpriteRenderer spriteRenderer = playerObject.GetComponent<SpriteRenderer>();
            
            spriteRenderer.sprite = Resources.Load<Sprite>("PlayerSprites/" + player.getHeroType());
            if(player.getHeroType() ==  "Male Archer")
            {
                playerObject.transform.localScale = boardContainerScaling2;
            }
            if (player.getHeroType() == "Female Archer")
            {
                playerObject.transform.localScale = boardContainerScaling4;
            }
            if (player.getHeroType() == "Male Warrior")
            {
                playerObject.transform.localScale = boardContainerScaling3;
            }
            if (player.getHeroType() == "Female Warrior")
            {
                playerObject.transform.localScale = boardContainerScaling4;
            }
            if (player.getHeroType() == "Male Wizard")
            {
                playerObject.transform.localScale = boardContainerScaling1;
            }
            if (player.getHeroType() == "Female Wizard")
            {
                playerObject.transform.localScale = boardContainerScaling2;
            }
            if (player.getHeroType() == "Male Dwarf")
            {
                playerObject.transform.localScale = boardContainerScaling1;
            }
            if (player.getHeroType() == "Female Dwarf")
            {
                playerObject.transform.localScale = boardContainerScaling2;
            }

            if (!Game.getGame().playerLocations.ContainsKey(player.getNetworkID())){
                // Give a random position
                Debug.Log(player.getHeroType());
                //int startingTile = Game.RANDOM.Next(20, 40);
                int startingTile = player.getHeroRank();
                Debug.Log(startingTile);
                playerObject.transform.position = tiles[startingTile].getMiddle();

                Game.getGame().playerLocations.Add(player.getNetworkID(), startingTile);
            }
            else
            {
                playerObject.transform.position = tiles[Game.getGame().playerLocations[player.getNetworkID()]].getMiddle();
            }

            // Time Icons
            GameObject timeObject = Instantiate(circlePrefab, playerTimeContainer);
            timeObjects.Add(player.getNetworkID(), timeObject);
            SpriteRenderer sr = timeObject.GetComponent<SpriteRenderer>();
            sr.color = player.getColor();

            if(timeObjectBounds == null)
            {
                timeObjectBounds = sr.bounds;
            }
            timeObject.transform.localScale = boardContainerScaling1;

            Vector3 timePos = getRandomPositionInBounds(timeTileBounds[0], timeObjectBounds, transform.position);
            timeObject.transform.position = timePos;
            rndPosInTimeBox[player.getNetworkID()] = timePos;


            //GameObject heroInfo = Instantiate(heroInfoPrefab, heroInfoContainer);
            //heroInfo.GetComponent<HeroInfoButton>().init(player);
        }
    }

    private void loadMonsters()
    {
        Vector3 boardScaling = new Vector3(1 / boardSpriteContainer.parent.lossyScale.x, 1 / boardSpriteContainer.parent.lossyScale.y, 1 / boardSpriteContainer.parent.lossyScale.z);

        //created all the monsters for Legend 2
        foreach (int gorTile in new int[]{8, 20, 21, 26, 48})
        {
            Gor g = new Gor(Game.gameState.positionGraph.getNode(gorTile));
            g.setMonsterType("Gor");
            g.setStrength(2);
            g.setWillpower(4);
            g.setReward(2);
            //Debug.Log("Gor" + g);
            Game.gameState.addMonster(g);
            Game.gameState.addGor(g);
        }
        foreach (int skralTile in new int[]{19})
        {
            Skral s = new Skral(Game.gameState.positionGraph.getNode(skralTile));
            s.setMonsterType("Skral");
            s.setStrength(6);
            s.setWillpower(5);
            s.setReward(4);
            Game.gameState.addMonster(s);
            Game.gameState.addSkral(s);
        }

        foreach (Monster monster in Game.gameState.getMonsters())
        {
            Debug.Log(monster.getPrefab());
            GameObject tempObj = Instantiate(monster.getPrefab(), -transform.position, transform.rotation, monsterContainer); ;
            monsterObjects.Add(monster, tempObj);
            tempObj.transform.position = tiles[monster.getLocation()].getMiddle();
            tempObj.transform.localScale = boardScaling;

        }

    }

    private void loadSkralOnTower()
    {
        int roll = 54;
        Game.gameState.skralTowerLocation = roll;
        Vector3 boardScaling = new Vector3(1 / boardSpriteContainer.parent.lossyScale.x, 1 / boardSpriteContainer.parent.lossyScale.y, 1 / boardSpriteContainer.parent.lossyScale.z);
        Skral s = new Skral(Game.gameState.positionGraph.getNode(roll));
        s.setMonsterType("Skral");
        s.setStrength(6);
        s.setWillpower(6);
        s.setReward(4);
        s.setCantMove();
        s.setSkralTower();
        Game.gameState.addMonster(s);
        Game.gameState.addSkral(s);
        GameObject tempObj = Instantiate(s.getPrefab(), -transform.position, transform.rotation, monsterContainer);
        monsterObjects.Add(s, tempObj);
        tempObj.transform.position = tiles[s.getLocation()].getMiddle();
        tempObj.transform.localScale = boardScaling;
        towerSkral = Instantiate(tower, tiles[roll].getMiddle(), transform.rotation);


    }

    private void loadMerchants()
    {
        int[] locations = { 18, 57, 71 };
        foreach(int loc in locations)
        {
            Merchant m = new Merchant(loc);
            Game.gameState.addMerchant(loc, m);
        }
    }


    private void loadWells()
    {
        foreach (int pos in new int[] { 5, 35, 45, 55 })
        {
            Debug.Log("Added well at position: " + pos);
            GameObject wellObject = Instantiate(well_front, tiles[pos].getMiddle(), transform.rotation);
            Well w = new Well(Game.gameState.positionGraph.getNode(pos),wellObject);
            Debug.Log(w);
            Debug.Log(w.getLocation());
            Game.gameState.addWell(w);
            //Debug.Log("Added well at position: " + pos);
        }
    }


    private void loadPrinceThorald()
    {
        GameObject princeThorald = Instantiate(prince, tiles[72].getMiddle(), transform.rotation);
        PrinceThorald princeT = new PrinceThorald(Game.gameState.positionGraph.getNode(72), princeThorald);
        Game.gameState.addPrince(princeT);
        princeThoraldObject.Add(princeT, princeThorald);
        Debug.Log("Added prince at position: " + princeT.getLocation());
    }

    public void instantiateEventGor(int location)
    {
        Gor g = new Gor(Game.gameState.positionGraph.getNode(location));
        Vector3 boardScaling = new Vector3(1 / boardSpriteContainer.parent.lossyScale.x, 1 / boardSpriteContainer.parent.lossyScale.y, 1 / boardSpriteContainer.parent.lossyScale.z);
        GameObject tempObj = Instantiate(g.getPrefab(), -transform.position, transform.rotation, monsterContainer);
        tempObj.transform.position = tiles[g.getLocation()].getMiddle();
        tempObj.transform.localScale = boardScaling;
        monsterObjects.Add(g, tempObj);
        Game.gameState.addGor(g);
        Game.gameState.addMonster(g);
        Debug.Log("Added event gor");
        
    }

    public void instantiateEventSkral(int location)
    {
        Skral s = new Skral(Game.gameState.positionGraph.getNode(location));
        Vector3 boardScaling = new Vector3(1 / boardSpriteContainer.parent.lossyScale.x, 1 / boardSpriteContainer.parent.lossyScale.y, 1 / boardSpriteContainer.parent.lossyScale.z);
        GameObject tempObj = Instantiate(s.getPrefab(), -transform.position, transform.rotation, monsterContainer);
        tempObj.transform.position = tiles[s.getLocation()].getMiddle();
        tempObj.transform.localScale = boardScaling;
        monsterObjects.Add(s, tempObj);
        Game.gameState.addSkral(s);
        Game.gameState.addMonster(s);
        Debug.Log("Added skral");

    }


    private void instantiateMedicinalHerb(int roll)
    {
        int loc = 0;
        if (roll == 1 || roll == 2)
        {
            loc = 37;
        }
        if (roll == 3 || roll == 4)
        {
            loc = 67;
        }
        if (roll == 5 || roll == 6)
        {
            loc = 61;
        }
        Debug.Log("got roll");
        GameObject herb = Instantiate(medicinalHerb3, tiles[loc].getMiddle(), transform.rotation);
        Debug.Log("instantiated");
        MedicinalHerb mh = new MedicinalHerb(Game.gameState.positionGraph.getNode(loc), herb);
        Debug.Log("instantiated2");
        Game.gameState.addMedicinalHerb(mh);
        Debug.Log("instantiated3");
        medicinalHerbObject.Add(mh, herb);
        Debug.Log("Added medicinal herb at position: " + mh.getLocation());


        Gor g = new Gor(Game.gameState.positionGraph.getNode(loc));
        g.setMonsterType("Gor");
        g.setStrength(2);
        g.setWillpower(4);
        g.setReward(2);
       // g.setCantMove();
        g.setHerbGor();

        Vector3 boardScaling = new Vector3(1 / boardSpriteContainer.parent.lossyScale.x, 1 / boardSpriteContainer.parent.lossyScale.y, 1 / boardSpriteContainer.parent.lossyScale.z);
        GameObject tempObj = Instantiate(g.getPrefab(), -transform.position, transform.rotation, monsterContainer);
        tempObj.transform.position = tiles[g.getLocation()].getMiddle();
        tempObj.transform.localScale = boardScaling;
        monsterObjects.Add(g, tempObj);
        Game.gameState.addGor(g);
        Game.gameState.addMonster(g);
        Debug.Log("Added medicinal herb gor");

        //foreach (Monster monster in Game.gameState.getMonsters())
        //{
        //    Debug.Log(monster.getPrefab());
        //    GameObject tempObj = Instantiate(monster.getPrefab(), -transform.position, transform.rotation, monsterContainer); ;
        //    monsterObjects.Add(monster, tempObj);
        //    tempObj.transform.position = tiles[monster.getLocation()].getMiddle();
        //    tempObj.transform.localScale = boardScaling;

        //}
        //need to instantiate Gor on the same spot

    }

    public void loadFogTokens()
    {

        int i = 0;
        foreach (int pos in new int[] { 8,11,12,13,16,32,42,44,46,47,48,49,56,64,63})
        {
            Debug.Log("Added fog at position: " + pos);
            GameObject fogToken = Instantiate(fog, tiles[pos].getMiddle(), transform.rotation);
            //Game.gameState.fogtoken_order[i]
            FogToken f = new FogToken(Game.gameState.positionGraph.getNode(pos), fogToken, Game.gameState.fogtoken_order[i]);
            Game.gameState.addFogToken(f);
            i++;
            //Debug.Log("Added well at position: " + pos);
        }
    }

    public void loadNarrator()
    {
        Debug.Log("Added Narrator at position: ");
        GameObject temp = Instantiate(narrator, legendTiles[1].center, transform.rotation);
        Narrator.Add(0, temp);

    }

    public void tele(int loc)
    {
        Vector3 boardContainerScaling3 = new Vector3(0.15f / boardSpriteContainer.parent.lossyScale.x, 0.15f / boardSpriteContainer.parent.lossyScale.y, 0.15f / boardSpriteContainer.parent.lossyScale.z);

        List<Node> neighbors = Game.gameState.positionGraph.getNode(loc).getAdjacentNodes();
        foreach (Node n in neighbors)
        {
            int nodeIndex = n.getIndex();
            foreach (KeyValuePair<FogToken, int> f in Game.gameState.getFogTokens())
            {
                if (f.Value == nodeIndex && !f.Key.used)
                {
                    GameObject fogToken = f.Key.getPrefab();
                    UnityEngine.Object.Destroy(f.Key.getPrefab());
                    if (f.Key.type == "brew")
                    {
                        fogToken = Instantiate(brewToken,tiles[nodeIndex].getMiddle(), transform.rotation);
                    }
                    if (f.Key.type == "gor")
                    {
                        fogToken = Instantiate(gorToken, tiles[nodeIndex].getMiddle(), transform.rotation);
                    }
                    if (f.Key.type == "willpower2")
                    {
                        fogToken = Instantiate(willpower2Token, tiles[nodeIndex].getMiddle(), transform.rotation);
                    }
                    if (f.Key.type == "willpower3")
                    {
                        fogToken = Instantiate(willpower3Token, tiles[nodeIndex].getMiddle(), transform.rotation);
                    }
                    if (f.Key.type == "wineskin")
                    {
                        fogToken = Instantiate(wineskinToken, tiles[nodeIndex].getMiddle(), transform.rotation);
                    }
                    if (f.Key.type == "event")
                    {
                        fogToken = Instantiate(eventToken, tiles[nodeIndex].getMiddle(), transform.rotation);
                    }
                    if (f.Key.type == "gold1")
                    {
                        fogToken = Instantiate(gold1Token, tiles[nodeIndex].getMiddle(), transform.rotation);
                    }
                    if (f.Key.type == "strength")
                    {
                        fogToken = Instantiate(strengthToken, tiles[nodeIndex].getMiddle(), transform.rotation);
                    }
                    fogToken.transform.localScale = boardContainerScaling3;
                    f.Key.setPrefab(fogToken);
                    
                }
            }

        }
    }

    public void loadFarmers()
    {
        foreach (int pos in new int[] { 24,36 })
        {
            Debug.Log("Added farmer at position: " + pos);
            Farmer f = new Farmer(Game.gameState.positionGraph.getNode(pos), Instantiate(farmer, tiles[pos].getMiddle(), transform.rotation));
            Game.gameState.addFarmer(f);
        }
    }

    public void loadGold()
    {
        foreach (Andor.Player p in Game.gameState.getPlayers())
        {
            Debug.Log("Added gold at position: ");
            Gold g = new Gold(Game.gameState.positionGraph.getNode(Game.gameState.getPlayerLocations()[(p.getNetworkID())]));
            g.setGold(2);
           //Game.gameState.addGold(g);
        }
    }


    public void setTime(string PlayerID, int hour)
    {
        rndPosInTimeBox[PlayerID] = getRandomPositionInBounds(timeTileBounds[hour], timeObjectBounds, new Vector3());
    }

    
    public void setTradeRequest(bool tradeReq)
    {
        tradeRequestSent = tradeReq;
    }

    public void sendNotif(string msg, float time, string notifyUser)
    {
        notifTime = time;
        notificationOn = true;
        notifMsg = msg;
        notifUser = notifyUser;
        
    }

    public void notify()
    {
        if(notifUser == Game.myPlayer.getNetworkID())
        {
            if (notifTime > 0)
            {
                notification.gameObject.SetActive(true);
                Transform[] trs = notification.gameObject.GetComponentsInChildren<Transform>();
                foreach (Transform t in trs)
                {
                    if (t.name == "Message")
                    {
                        Text msg = t.gameObject.GetComponent<Text>();
                        msg.text = notifMsg;
                    }
                }
            }
            else
            {
                closeNotif();
            }
        }
    }

    public void closeNotif()
    {
        notificationOn = false;
        notification.gameObject.SetActive(false);
    }

    public void sendTradeRequest(string[] tradeType, string playerFrom, string playerTo, bool usingFalcon)
    {
        //foreach(string t in ts.tradeType)
        //{
        //    Debug.Log("gc sendTradeRequest(): " + t);
        //}

        ts.setTradeType(tradeType);
        string[] pl = new string[2];

        pl[0] = playerFrom;
        pl[1] = playerTo;
        ts.setPlayers(pl);

        playerTradeTo = playerTo;
        playerTradeFrom = playerFrom;
        string msg = "";
        if (tradeType[0].Equals("Gold"))
        {
            msg = Game.gameState.getPlayer(playerFrom).getHeroType() + " would like to give gold";
            tradeRequestSent = true;

        }
        else if (tradeType[0].Equals("Gemstones"))
        {
            msg = Game.gameState.getPlayer(playerFrom).getHeroType() + " would like to give a gemstone";
            tradeRequestSent = true;

        }
        else
        {

            if (usingFalcon)
            {
                Debug.Log("using falcon!");
                if (tradeType[1] == "Shield" || tradeType[2] == "Shield" || tradeType[1] == "Bow" || tradeType[2] == "Bow")
                {
                    Debug.Log(tradeType[1]);
                    Debug.Log(tradeType[2]);
                    Debug.Log("invalid!");
                    ts.clear();
                    invalidTradeNotify(Game.gameState.getPlayer(playerFrom));

                }
                else
                {
                    ts.usingFalcon = true;
                    msg = Game.gameState.getPlayer(playerFrom).getHeroType() + " would like to trade your " + tradeType[2]
                + " for " + Game.gameState.getPlayer(playerFrom).getHero().getPronouns()[2] + " " + tradeType[1];
                    tradeRequestSent = true;

                }

            }
            else {
                Debug.Log(tradeType[1]);
                Debug.Log(tradeType[2]);

                msg = Game.gameState.getPlayer(playerFrom).getHeroType() + " would like to trade your " + tradeType[2]
                + " for " + Game.gameState.getPlayer(playerFrom).getHero().getPronouns()[2] + " " + tradeType[1];
                tradeRequestSent = true;

            }

        }

        tradeMsg = msg;
        
    }

    public void processTradeRequest()
    {
        tradeRequestSent = false;

        tradeRequest.gameObject.SetActive(true);
        Transform[] trs = tradeRequest.gameObject.GetComponentsInChildren<Transform>(true);
        foreach(Transform t in trs)
        {
            if (t.name == "Title") {
                Text title = t.gameObject.GetComponent<Text>();
                title.text = Game.gameState.getPlayer(playerTradeFrom).getHeroType() + " would like to trade!";

            }
            if(t.name == "Body")
            {
                Text body = t.gameObject.GetComponent<Text>();
                body.text = tradeMsg;
            }
        }
    }

    public void accept(bool acc)
    {
        //foreach (string t in tradeType)
        //{
        //    Debug.Log("from accept(): " + t);
        //}
        tradeRequestSent = false;
        tradeRequest.gameObject.SetActive(false);
        ts.accept(acc);
        //Game.sendAction(new RespondTrade(players, tradeType, acc));

    }

    public void merchantClick()
    {
        ms.displayAvailableItems();
    }

    public void useHelmInFight()
    {
        Game.sendAction(new UseHelm(Game.myPlayer.getNetworkID()));
    }

    public void useWitchBrewInFight()
    {
        Game.sendAction(new UseWitchBrew(Game.myPlayer.getNetworkID()));
    
    }
    public void useBowInFight()
    {
        Game.sendAction(new UseBow(Game.myPlayer.getNetworkID()));

    }

    public void sendFightRequest(string[] players)
    {
       
        invitedFighters = players;
       
        fightRequestSent = true;
        
    }

    public void processFightRequest(bool otherPlayers)
    {
        fightRequestSent = false;
        
        
        if (otherPlayers)
        {

            fightRequest.gameObject.SetActive(true);
            string msg = Game.gameState.getPlayer(invitedFighters[0]).getHeroType() + " has invited you to fight!";
            Transform[] trs = fightRequest.gameObject.GetComponentsInChildren<Transform>(true);
            foreach (Transform t in trs)
            {
                if (t.name == "HeaderText")
                {
                    t.gameObject.GetComponent<Text>().text = msg;
                }
            }
        }
        else
        {
            //string[] fightPlayers = new string[1];
            //fightPlayers[0] = invitedFighters[0];
            //Game.sendAction(new RespondFight(fightPlayers, true));
            fsc.openFightLobby(invitedFighters[0]);
        }
            
        
        
        

    }

    public void acceptFightRequest(bool accept)
    {
        fightRequestSent = false;
        fightRequest.gameObject.SetActive(false);
        fsc.acceptFightRequest(accept, Game.myPlayer.getNetworkID());

    }

    public void setActiveFight()
    {
        fightActive = true;
    }

    #region buttonClicks
    //Logic for game tile clicks
    public void tileClick(BoardPosition tile)
    {
        if (moveSelected)
        {
            
            // if(Game.myPlayer.getHero().selectedWineskin == true)
            //{
            Game.sendAction(new Move(Game.myPlayer.getNetworkID(), Game.getGame().playerLocations[Game.myPlayer.getNetworkID()], tile.tileID));

           // }

            ColorBlock cb = moveButton.colors;
            cb.normalColor = new Color32(229, 175, 81, 255);
            cb.selectedColor = new Color32(229, 175, 81, 255);

            moveSelected = false;

            moveButton.colors = cb;

        }

        if (movePrinceSelected)
        {
            Game.sendAction(new MovePrinceThorald(Game.myPlayer.getNetworkID(), Game.getGame().getPrinceThorald()[0].getLocation(), tile.tileID));

            ColorBlock cb = movePrinceButton.colors;
            cb.normalColor = new Color32(229, 175, 81, 255);
            cb.selectedColor = new Color32(229, 175, 81, 255);

            movePrinceSelected = false;

            movePrinceButton.colors = cb;
        }


    }

    public void wineskinUse(int sides)
    {
        Debug.Log("controller sides" + sides);
        Game.sendAction(new UseWineskin(Game.myPlayer.getNetworkID(), sides));

    }

    public void moveClick()
    {
        ColorBlock cb = moveButton.colors;

        if (!moveSelected)
        {
            moveSelected = true;
            cb.normalColor = new Color32(255, 240, 150, 255);
            cb.selectedColor = new Color32(255, 240, 150, 255);
            Debug.Log("updating wineskin");
            updateWineskin2();
        }
        else
        {
            cb.normalColor = new Color32(229, 175, 81, 255);
            cb.selectedColor = new Color32(229, 175, 81, 255);

            moveSelected = false;
        }
        moveButton.colors = cb;
    }



    public void movePrinceClick()
    {
        ColorBlock cb = moveButton.colors;

        if (!movePrinceSelected)
        {
            movePrinceSelected = true;
            cb.normalColor = new Color32(255, 240, 150, 255);
            cb.selectedColor = new Color32(255, 240, 150, 255);

        }
        else
        {
            cb.normalColor = new Color32(229, 175, 81, 255);
            cb.selectedColor = new Color32(229, 175, 81, 255);

            movePrinceSelected = false;
        }
        movePrinceButton.colors = cb;
    }
    public void exitGameClick()
    {
        displayPauseMenu();
    }

    public void fightClick()
    {
        Debug.Log("fight clicked");
        fsc.displayTypeOfFight();
    }
    public void passClick()
    {
        Debug.Log("pass clicked");
        //Game.sendAction(new PassTurn(Game.myPlayer.getNetworkID()));
        //updateWineskin();
        


    }
    public void updateWineskin2()
    {
        int numLeft = 0;
        Debug.Log("wineskin1");
        if (Game.myPlayer.getHero().allArticlesAsStringList().Contains("Wineskin"))
        {
            Debug.Log("wineskin2");
            foreach (Wineskin w in Game.myPlayer.getHero().getAllArticles()["Wineskin"])
            {
                Debug.Log("wineskincheckloop");
                int left = 2 - w.getNumUsed();
                numLeft += left;
            }
            Debug.Log(numLeft);
            Debug.Log("searching for dropdown");
            List<String> numbers = new List<String>();
            for (int i = 0; i < numLeft + 1; i++)
            {
                numbers.Add(i.ToString());
            }

            Transform[] trs = wineskinDropdown.GetComponentsInChildren<Transform>(true);
            foreach (Transform t in trs)
            {
                if (t.name == "wineselect")
                {

                    Dropdown myArticlesMenu = t.gameObject.GetComponent<Dropdown>();
                    Debug.Log("got dropdown");

                    myArticlesMenu.ClearOptions();
                    myArticlesMenu.AddOptions(numbers);
                    //myArticlesMenu.GetComponent<Dropdown>().captionText.text = myArticles[0];
                    Debug.Log("added it to dropdowns!");
                }

            }

        }
    }

    public void updateWineskin()
    {
        int numLeft = 0;
        Debug.Log("wineskin1");
        if (Game.myPlayer.getHero().allArticlesAsStringList().Contains("Wineskin"))
        {
            Debug.Log("wineskin2");

            //foreach (Article a in Game.myPlayer.getHero().getAllArticles()["Wineskin"])
            //{
            Debug.Log("wineskincheckloop");

            //if (a.getArticle() == ArticleType.Wineskin)
            //{
            //    Debug.Log("WOOOOOOOGOOOOOOOO");
            //}
            //int left = 2 - a.getNumUsed();
            // numLeft += left;
            //w.useArticle();
            //if (w.getNumUsed() == 2)
            //{
            // Game.myPlayer.getHero().removeArticle2("Wineskin", w);
            //Debug.Log("removed Article");
            // Game.gameState.
            //add to equipment board
            //}
            // }
            Debug.Log("wineskin3");

            Debug.Log(numLeft);
            Debug.Log("searching for dropdown");
            List<String> numbers = new List<String>();
            for (int i = 0; i < numLeft + 5; i--)
            {
                numbers.Add(i.ToString());
            }

            //GameObject parentObj = GameObject.Find("SelectHero");
            //Transform[] trs = wineskinDropdown.GetComponentsInChildren<Transform>(true);
            //foreach (Transform t in trs)
            //{
            //    if (t.name == "wineselect")
            //    {

            //        Dropdown myArticlesMenu = t.gameObject.GetComponent<Dropdown>();
            //        Debug.Log("got dropdown");

            //        myArticlesMenu.ClearOptions();
            //        myArticlesMenu.AddOptions(numbers);
            //        //myArticlesMenu.GetComponent<Dropdown>().captionText.text = myArticles[0];
            //        Debug.Log("added it to dropdowns!");
            //    }

            //}


            //Debug.Log("setting dropdown");
            //wineMenu.ClearOptions();
            //wineMenu.AddOptions(numbers);
            //}
        }
    }
    public void endDayClick()
    {
        Debug.Log("end day clicked");
        Game.sendAction(new EndTurn(Game.myPlayer.getNetworkID()));
    }

    public void tradeClick()
    {
        ts.displayTradeType();
        
        //List<Dropdown.OptionData> menuOptions = dropdown.GetComponent<Dropdown>().options;
        //string value = menuOptions[menuIndex].text;

    }

    public void dropPickClick()
    {

        pickDropContainer.gameObject.SetActive(true);
        pickDropContainer.GetChild(0).GetChild(0).GetComponent<PickDropController>().updateInteractables();
        // Dropping Item
        /*foreach (Interactable interact in Game.gameState.getInteractables(Game.myPlayer.getNetworkID()))
        {
            if(interact is PickDrop)
            {
                Debug.Log("Dropping ITEM!");
                Game.sendAction(new Interact(Game.myPlayer.getNetworkID(), interact.getInteractableID(), -1));
                return;
            }
        }

        // Picking up an item
        foreach (Interactable interact in Game.gameState.positionGraph.getNode(Game.gameState.playerLocations[Game.myPlayer.getNetworkID()]).getInteractables())
        {
            if (interact is PickDrop)
            {
                Debug.Log("Picking up ITEM!");
                Game.sendAction(new Interact(Game.myPlayer.getNetworkID(), interact.getInteractableID(), Game.gameState.playerLocations[Game.myPlayer.getNetworkID()]));
                return;
            }
        }*/
    }


    #endregion

    #region pause_menu

    public void displayPauseMenu()
    {
        pauseMenuActive = true;
        pauseMenuContainer.gameObject.SetActive(true);
    }
    public void removePauseMenu()
    {
        pauseMenuActive = false;
        pauseMenuContainer.gameObject.SetActive(false);
        saveGameContainer.gameObject.SetActive(false);

    }

    #endregion

    public static Vector3 getRandomPositionInBounds(Bounds mainObject, Bounds objInside, Vector3 translateOffset)
    {
        float widthToUse = mainObject.size.x - objInside.size.x;
        float heightToUse = mainObject.size.y - objInside.size.y;

        float randPosX = (float)Game.RANDOM.NextDouble() * widthToUse + objInside.size.x / 2;
        float randPosY = (float)Game.RANDOM.NextDouble() * heightToUse + objInside.size.y / 2;

        float translateToCenterX = randPosX - mainObject.size.x / 2;
        float translateToCenterY = randPosY - mainObject.size.y / 2;

        return new Vector3(mainObject.center.x + translateToCenterX - translateOffset.x, mainObject.center.y + translateToCenterY - translateOffset.y, -1);
    }

    public static Vector3 moveTowards(Vector3 from, Vector3 to, float delta)
    {
        return new Vector3(Mathf.MoveTowards(from.x, to.x, delta), Mathf.MoveTowards(from.y, to.y, delta), -1);
    }
    public void SLEEP(float sec)
    {
        StartCoroutine(Game.sleep(sec));
    }


    public bool winScenario()
    {
        //checks that herb is in castle, castle defended
        if (checkMedicinalHerbAtCastle() && (Game.gameState.outcome == "undetermined") && Game.gameState.skralTowerDefeated)
        {
            Game.gameState.outcome = "won";
            return true;
        }
        else
        {
            return false;
        }

    }

    public bool checkMedicinalHerbAtCastle()
    {
        //foreach (MedicinalHerb mh in Game.gameState.getMedicinalHerb())
        //{
        //    if (mh.getLocation() == 0)
        //    {
        //        return true;
        //    }
        //}
        //return false;
        if (Game.gameState.getMedicinalHerb() != null)
        {
            return false;
        }

        if(Game.gameState.getMedicinalHerb().getLocation() == 0)
        {
            return true;
        }
        return false;
    }

    public void initializeStrengthPoints()
    {
        foreach(Andor.Player p in Game.gameState.getPlayers())
        {
            p.getHero().increaseStrength(2);
            //will comment out
            p.getHero().increaseWillpower(5);
            Debug.Log(p.getHero() + " " + p.getHero().getStrength());
        }
    }

    public void initializeWineskin()
    {
        foreach (Andor.Player p in Game.gameState.getPlayers())
        {
            //p.getHero().increaseStrength(2);
            ////will comment out
            //p.getHero().increaseWillpower(5);
            //Debug.Log(p.getHero() + " " + p.getHero().getStrength());
            p.getHero().addArticle(new Wineskin());
        }
    }

    public void clearTrade()
    {
        ts.clear();
    }


}
