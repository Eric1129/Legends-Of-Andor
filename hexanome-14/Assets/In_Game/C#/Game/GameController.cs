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
    public Transform selectTradeType;
    public Transform selectHeroTrade;
    public Transform selectHeroGive;
    public Transform tradeRequest;
    
    public Transform boardSpriteContainer;
    public Transform playerContainer;
    public Transform playerTimeContainer;
    public Button moveButton;
    public Text turnLabel;

    public GameObject emptyPrefab;
    public GameObject playerPrefab;
    public Sprite fullBoardSprite;
    public GameObject circlePrefab;

    public Dictionary<int, BoardPosition> tiles;
    public Dictionary<string, GameObject> playerObjects;
    public Dictionary<string, GameObject> timeObjects;
    public Dictionary<int, Bounds> timeTileBounds;
    public Bounds timeObjectBounds;
    public Dictionary<string, Vector3> rndPosInTimeBox;

    private bool pauseMenuActive = false;
    private bool moveSelected = false;

    private bool tradeRequestSent = false;
    private string playerTradeTo;
    private string playerTradeFrom;
    private string tradeMsg;
    

    //private int tradeTypeIndex = -1;
    public TradeScreen ts;

    // Start is called before the first frame update
    void Start()
    {
        ts = new TradeScreen();

        Game.started = true;
        Game.createPV();

        tiles = new Dictionary<int, BoardPosition>();
        playerObjects = new Dictionary<string, GameObject>();
        timeObjects = new Dictionary<string, GameObject>();
        timeTileBounds = new Dictionary<int, Bounds>();
        rndPosInTimeBox = new Dictionary<string, Vector3>();

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
        while(Game.gameState.turnManager == null)
        {
            StartCoroutine(Game.sleep(0.01f));
            if(timeout <= 0)
            {
                throw new Exception("Could not initialize TurnManager!");
            }
            timeout--;
        }
        Debug.Log(Game.gameState.turnManager.currentPlayerTurn());

        turnLabel.text = Game.gameState.turnManager.currentPlayerTurn();
        if (Game.gameState.turnManager.currentPlayerTurn().Equals(Game.myPlayer.getNetworkID())){
            turnLabel.color = Game.myPlayer.getColor();
        }
        else
        {
            turnLabel.color = UnityEngine.Color.black;
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
            foreach (Andor.Player player in Game.gameState.getPlayers())
            {
                moveToNewPos(player);

                timeObjects[player.getNetworkID()].transform.position =
                    moveTowards(timeObjects[player.getNetworkID()].transform.position, rndPosInTimeBox[player.getNetworkID()], 1);
            }

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
            //foreach(Andor.Player p in Game.gameState.getPlayers())
            //{
            //    if (p.getNetworkID().Equals(playerTradeTo))
            //    {
            //        processTradeRequest();
            //    }
            //}

            //Debug.Log("MY PLAYER " + Game.myPlayer.getHeroType());
            if (Game.myPlayer.getNetworkID().Equals(playerTradeTo))
            {
                processTradeRequest();
            }
        }
    }
    public void moveToNewPos(Andor.Player player)
    {
        Vector3 playerPos = playerObjects[player.getNetworkID()].transform.position;
        Vector3 cellPos = tiles[Game.gameState.playerLocations[player.getNetworkID()]].getMiddle();
        playerObjects[player.getNetworkID()].transform.position = moveTowards(playerPos, cellPos, 1);
    }


    private void loadBoard()
    {

        // load background board
        Debug.Log(transform.position);
        Debug.Log(boardSpriteContainer.position);

        GameObject fullBoard = Instantiate(emptyPrefab, boardSpriteContainer.transform.position - boardSpriteContainer.transform.parent.position, transform.rotation, boardSpriteContainer);
        fullBoard.name = "full-Board";

        fullBoard.AddComponent<SpriteRenderer>();
        SpriteRenderer spriteRenderer = fullBoard.GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = fullBoardSprite;

        
        // load sprites
        Sprite[] sprites = Resources.LoadAll<Sprite>("BoardSprites");
        // Requirement: have Resources/Sprites folder under Assets
        if (sprites == null)
            print("Could not load board tile sprites");

        foreach (Sprite sprite in sprites)
        {
            createBoardPosition(sprite);
        }

        sprites = Resources.LoadAll<Sprite>("TimeSprites");
        // Requirement: have Resources/Sprites folder under Assets
        if (sprites == null)
            print("Could not load time tile sprites");

        foreach (Sprite sprite in sprites)
        {
            GameObject temp = Instantiate(emptyPrefab);
            temp.AddComponent<SpriteRenderer>().sprite = sprite;
            TileBounds tb = new TileBounds(temp.AddComponent<PolygonCollider2D>());
            Bounds b = tb.createBounds();
            Debug.Log(b);
            timeTileBounds.Add(Int32.Parse(sprite.name.Split('-')[1]), b);
        }


        // load players
        if (Game.gameState != null)
        {
            loadPlayers();
        }

    }

    private void createBoardPosition(Sprite sprite)
    {
        int cellNumber = Int32.Parse(sprite.name.Split('_')[1]);

        GameObject cellObject = Instantiate(emptyPrefab, transform.position, transform.rotation, boardSpriteContainer);
        cellObject.transform.localScale = boardSpriteContainer.transform.localScale;
        cellObject.tag = cellNumber.ToString();
        cellObject.name = "position-" + cellNumber.ToString();

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


    private void loadPlayers()
    {
        foreach(Andor.Player player in Game.gameState.getPlayers())
        {
            // Player Icons
            GameObject playerObject = Instantiate(playerPrefab, playerContainer);
            playerObjects.Add(player.getNetworkID(), playerObject);
            SpriteRenderer spriteRenderer = playerObject.GetComponent<SpriteRenderer>();
            
            spriteRenderer.sprite = Resources.Load<Sprite>("PlayerSprites/" + player.getHeroType());

            if (!Game.getGame().playerLocations.ContainsKey(player.getNetworkID())){
                // Give a random position
                int startingTile = Game.RANDOM.Next(20, 40);
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

            Vector3 timePos = getRandomPositionInBounds(timeTileBounds[0], timeObjectBounds, transform.position);
            timeObject.transform.position = timePos;
            rndPosInTimeBox[player.getNetworkID()] = timePos;
        }
    }

    public void GameSetup()
    {

    }

    public void setTime(string PlayerID, int hour)
    {
        rndPosInTimeBox[PlayerID] = getRandomPositionInBounds(timeTileBounds[hour], timeObjectBounds, transform.position);
    }

    

    public void sendTradeRequest(string[] tradeType, string playerFrom, string playerTo)
    {
        tradeRequestSent = true;
        playerTradeTo = playerTo;
        playerTradeFrom = playerFrom;
        string msg = "";
        if (tradeType[0].Equals("Gold"))
        {
            msg = Game.gameState.getPlayer(playerFrom).getHeroType() + " would like to give gold";
            selectHeroGive.gameObject.SetActive(false);
        }
        else if (tradeType[0].Equals("Gemstones"))
        {
            msg = Game.gameState.getPlayer(playerFrom).getHeroType() + " would like to give a gemstone";
            selectHeroGive.gameObject.SetActive(false);
        }
        else
        {
            msg = Game.gameState.getPlayer(playerFrom).getHeroType() + " would like to trade your " + tradeType[2]
                + " for " + Game.gameState.getPlayer(playerFrom).getHero().getPronouns()[2] + " " + tradeType[1];
            selectHeroTrade.gameObject.SetActive(false);
        }

        tradeMsg = msg;
        selectHeroTrade.gameObject.SetActive(false);
    }

    public void processTradeRequest()
    {
        Debug.Log("processing trade request!");

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

    //public void setTradeType(int menuIndex)
    //{
    //    Debug.Log("setting trade type");
    //    tradeTypeIndex = menuIndex;

    //}

    //public void nextClick()
    //{
    //    Debug.Log("next clicked");

    //    if(tradeTypeIndex == 0)
    //    {
    //        tradeActive = true;
    //        selectHeroTrade.gameObject.SetActive(true);
    //        //get all players on the same location
    //        //get the player that clicked on trade button
    //        int myLocation = 0;
    //        string[] playersInvolved = new string[4];
    //        int i = 1;
    //        playersInvolved[0] = Game.myPlayer.getNetworkID();
    //        if (Game.gameState.playerLocations.TryGetValue(Game.myPlayer.getNetworkID(), out myLocation))
    //        {
    //            foreach (Andor.Player p in Game.gameState.getPlayers())
    //            {
    //                int playerLocation = 0;
    //                if (Game.gameState.playerLocations.TryGetValue(p.getNetworkID(), out playerLocation))
    //                {
    //                    if (playerLocation == myLocation && !Game.myPlayer.Equals(p))
    //                    {
    //                        playersInvolved[i] = p.getNetworkID();
    //                        displayPlayerInfo(p, i);
    //                        i++;

    //                    }

    //                }
    //            }
    //        }
    //        Game.sendAction(new InitiateTrade(playersInvolved));

    //    }

    //}

    //public void displayPlayerInfo(Andor.Player player, int i)
    //{

    //    GameObject selectHero = GameObject.Find("SelectHero");
    //    GameObject herogameobj;
    //    Transform[] trs = selectHero.GetComponentsInChildren<Transform>(true);
    //    //Transform[] heroattr = new Transform[3];
    //    foreach (Transform t in trs)
    //    {

    //        if (t.name == ("Hero" + i))
    //        {
    //            herogameobj = t.gameObject;
    //            t.gameObject.SetActive(true);
    //            Transform[] heroattr = herogameobj.GetComponentsInChildren<Transform>(true);
    //            foreach (Transform attr in heroattr)
    //            {
    //                attr.gameObject.SetActive(true);
    //                if (attr.name == "Name")
    //                {

    //                    Text heroname = attr.GetComponent<Text>();
    //                    heroname.text = player.getHeroType();
    //                }
    //                if (attr.name == "Image")
    //                {
    //                    Debug.Log("Image");
    //                    Sprite herosprite = Resources.Load<Sprite>("PlayerSprites/" + player.getHeroType());
    //                    attr.GetComponent<Image>().sprite = herosprite;
    //                }
    //                if (attr.name == "HeroItems")
    //                {
    //                    Debug.Log("Hero items");
    //                    Text heroitems = attr.GetComponent<Text>();
    //                    heroitems.text = "Gold: " + player.getHero().getGold() + "\n";
    //                    heroitems.text += "\nGemstones: " + player.getHero().getGemstone() + "\n";
    //                    heroitems.text += "\nArticles: ";
    //                    List<string> heroAr = new List<string>();
    //                    foreach (string ar in heroAr)
    //                    {
    //                        heroitems.text += (ar + " ");
    //                    }
    //                }
    //            }
    //        }
    //    }




    //}

    public void closeTradeMenu()
    {
        
        selectTradeType.gameObject.SetActive(false);
        
    }

    //public void onHeroClick()
    //{
    //    Debug.Log("On hero click");
    //    string tradeWith = this.gameObject.transform.name;
    //    Debug.Log("tradeWith " + tradeWith);
    //    //int index = -1;
    //    if (char.IsDigit(tradeWith[4]))
    //    {
    //        int index = tradeWith[4];
    //        if(tradeTypeIndex == 0)
    //        {

    //        }
    //        //setToTradeHero(players[index]);
    //    }
    //    //Game.sendAction(new InitiateTrade(playersInvolved));

    //}
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
}
