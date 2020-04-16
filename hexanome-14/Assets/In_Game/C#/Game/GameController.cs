using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;

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

    public Transform heroInfoScreen;

    public Transform boardSpriteContainer;
    public Transform playerContainer;
    public Transform playerTimeContainer;
    public Transform monsterContainer;
    public Transform heroInfoContainer;


    public Button moveButton;
    public Text turnLabel;
    //public Text scrollText;
    public Text scrollTxt;
    

    public GameObject emptyPrefab;
    public GameObject playerPrefab;
    public Sprite fullBoardSprite;
    public GameObject circlePrefab;
    public GameObject heroInfoPrefab;
    public GameObject well_front;
    //public GameObject scroll;
    public GameObject scroll;

    public Dictionary<int, BoardPosition> tiles;
    public Dictionary<string, GameObject> playerObjects;
    public Dictionary<string, GameObject> timeObjects;
    public Dictionary<int, Bounds> timeTileBounds;
    public Bounds timeObjectBounds;
    public Dictionary<string, Vector3> rndPosInTimeBox;
    public Dictionary<Monster, GameObject> monsterObjects;


    private bool pauseMenuActive = false;
    private bool moveSelected = false;


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

    private Transform initTransform;
    //private string[] tradeType;
    //private string[] players;


    // Start is called before the first frame update
    void Start()
    {
        ts = tradeScreenController.gameObject.GetComponent<TradeScreen>();
        ms = merchantScreenController.gameObject.GetComponent<MerchantScreen>();
        //ts = new TradeScreen();
        //this.tradeType = new string[3];
        //this.players = new string[2];
        Game.started = true;
        Game.createPV();

        tiles = new Dictionary<int, BoardPosition>();
        playerObjects = new Dictionary<string, GameObject>();
        timeObjects = new Dictionary<string, GameObject>();
        timeTileBounds = new Dictionary<int, Bounds>();
        rndPosInTimeBox = new Dictionary<string, Vector3>();
        monsterObjects = new Dictionary<Monster, GameObject>();

        initTransform = transform;

        instance = this;

        // For drawing everything
        loadBoard();

        // For setting up resource distribution 
        GameSetup();

        // Set up Turn Manager
        if (PhotonNetwork.IsMasterClient)
        {
            List<Andor.Player> randomOrder = Game.getGame().getPlayers();
            Game.Shuffle(randomOrder);

            Game.setTurnManager(randomOrder);
        }
        int timeout = 300;
        if (Game.gameState != null)
        {
            while (Game.gameState.turnManager == null)
            {
                StartCoroutine(Game.sleep(0.01f));
                if (timeout <= 0)
                {
                    throw new Exception("Could not initialize TurnManager!");
                }
                timeout--;
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
        }
        
    }

    void Update()
    {
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
        if(Game.gameState != null)
        {
            // Update Player position
            foreach (Andor.Player player in Game.gameState.getPlayers())
            {
                playerObjects[player.getNetworkID()].transform.position =
                    moveTowards(playerObjects[player.getNetworkID()].transform.position, tiles[Game.gameState.playerLocations[player.getNetworkID()]].getMiddle(), 0.5f);

                timeObjects[player.getNetworkID()].transform.position =
                    moveTowards(timeObjects[player.getNetworkID()].transform.position, rndPosInTimeBox[player.getNetworkID()], 1);
            }

            // Update Player position
            foreach (Monster monster in Game.gameState.getMonsters())
            {
                monsterObjects[monster].transform.position =
                    moveTowards(monsterObjects[monster].transform.position, tiles[monster.getLocation()].getMiddle(), 0.5f);
            }

            // Update player turn
            turnLabel.text = Game.gameState.turnManager.currentPlayerTurn();
            if (Game.gameState.turnManager.currentPlayerTurn().Equals(Game.myPlayer.getNetworkID()))
            {
                turnLabel.color = Game.myPlayer.getColor(130);
            }
            else
            {
                turnLabel.color = UnityEngine.Color.black;
            }
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


    public void loseScenario()
    {
        scrollTxt.text = "YOU LOST!";
        scroll.SetActive(true);
    }
    public void GameSetup()
    {
        int playerCount = Game.gameState.getPlayers().Count;

        if (playerCount == 1 || playerCount == 2)
        {
            Game.gameState.maxMonstersAllowedInCastle = 3;
        }
        else if (playerCount == 3)
        {
            Game.gameState.maxMonstersAllowedInCastle = 2;

        }
        else if (playerCount == 4)
        {
            Game.gameState.maxMonstersAllowedInCastle = 1;

        }
        /////////////////////////////////////////////////////////////////////

        // load players
        if (Game.gameState != null)
        {
            loadPlayers();

            loadMonsters();

            loadWells();
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
        Vector3 boardContainerScaling = new Vector3(1 / boardSpriteContainer.parent.lossyScale.x, 1 / boardSpriteContainer.parent.lossyScale.y, 1 / boardSpriteContainer.parent.lossyScale.z);

        foreach (Andor.Player player in Game.gameState.getPlayers())
        {
            // Player Icons
            GameObject playerObject = Instantiate(playerPrefab, playerContainer);
            playerObjects.Add(player.getNetworkID(), playerObject);
            SpriteRenderer spriteRenderer = playerObject.GetComponent<SpriteRenderer>();
            
            spriteRenderer.sprite = Resources.Load<Sprite>("PlayerSprites/" + player.getHeroType());
            playerObject.transform.localScale = boardContainerScaling;

            if (!Game.getGame().playerLocations.ContainsKey(player.getNetworkID())){
                // Give a random position
                Debug.Log(player.getHeroType());
                //int startingTile = Game.RANDOM.Next(20, 40);
                int startingTile = player.getHeroRank();
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
            timeObject.transform.localScale = boardContainerScaling;

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
            Gor g = new Gor(Game.positionGraph.getNode(gorTile));
            //Debug.Log("Gor" + g);
            Game.gameState.addMonster(g);
            Game.gameState.addGor(g);
        }
        foreach (int skralTile in new int[]{9})
        {
            Skral s = new Skral(Game.positionGraph.getNode(skralTile));
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

    private void loadWells()
    {
        //foreach (int pos in new int[] {5, 35, 45, 55})
        //{
        //    Debug.Log("Added well at position: " + pos);
        //    Well w = new Well(Game.positionGraph.getNode(pos));
        //    Debug.Log(w);
        //    Debug.Log(w.getLocation());
        //    Game.gameState.addWell(w);
        //    //Debug.Log("Added well at position: " + pos);
        //}

        //foreach(Well well in Game.gameState.getWells().Keys)
        //{
        //    GameObject wellObject = Instantiate(well_front, tiles[well.getLocation()].getMiddle(), transform.rotation);
        //}
        foreach (int pos in new int[] { 5, 35, 45, 55 })
        {
            Debug.Log("Added well at position: " + pos);
            GameObject wellObject = Instantiate(well_front, tiles[pos].getMiddle(), transform.rotation);
            Well w = new Well(Game.positionGraph.getNode(pos),wellObject);
            Debug.Log(w);
            Debug.Log(w.getLocation());
            Game.gameState.addWell(w);
            //Debug.Log("Added well at position: " + pos);
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

    public void sendTradeRequest(string[] tradeType, string playerFrom, string playerTo)
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

        tradeRequestSent = true;
        playerTradeTo = playerTo;
        playerTradeFrom = playerFrom;
        string msg = "";
        if (tradeType[0].Equals("Gold"))
        {
            msg = Game.gameState.getPlayer(playerFrom).getHeroType() + " would like to give gold";
            
        }
        else if (tradeType[0].Equals("Gemstones"))
        {
            msg = Game.gameState.getPlayer(playerFrom).getHeroType() + " would like to give a gemstone";
            
        }
        else
        {
            msg = Game.gameState.getPlayer(playerFrom).getHeroType() + " would like to trade your " + tradeType[2]
                + " for " + Game.gameState.getPlayer(playerFrom).getHero().getPronouns()[2] + " " + tradeType[1];
            
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

    }

    #region buttonClicks
    //Logic for game tile clicks
    public void tileClick(BoardPosition tile)
    {
        if (moveSelected)
        {
            Game.sendAction(new Move(Game.myPlayer.getNetworkID(), Game.getGame().playerLocations[Game.myPlayer.getNetworkID()], tile.tileID));

            ColorBlock cb = moveButton.colors;
            cb.normalColor = new Color32(229, 175, 81, 255);
            cb.selectedColor = new Color32(229, 175, 81, 255);

            moveSelected = false;

            moveButton.colors = cb;

        }


    }


    public void moveClick()
    {
        ColorBlock cb = moveButton.colors;

        if (!moveSelected)
        {
            moveSelected = true;
            cb.normalColor = new Color32(255, 240, 150, 255);
            cb.selectedColor = new Color32(255, 240, 150, 255);

        }
        else
        {
            cb.normalColor = new Color32(229, 175, 81, 255);
            cb.selectedColor = new Color32(229, 175, 81, 255);

            moveSelected = false;
        }
        moveButton.colors = cb;
    }
    public void exitGameClick()
    {
        displayPauseMenu();
    }

    public void fightClick()
    {
        Debug.Log("fight clicked");
    }
    public void passClick()
    {
        Debug.Log("pass clicked");
        Game.sendAction(new PassTurn(Game.myPlayer.getNetworkID()));

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
}
