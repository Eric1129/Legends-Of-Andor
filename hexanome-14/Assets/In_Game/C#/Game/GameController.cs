﻿using System;
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

    public Transform boardSpriteContainer;
    public Transform playerContainer;
    public Transform playerTimeContainer;
    public Transform monsterContainer;

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
    public Dictionary<Monster, GameObject> monsterObjects;

    private bool pauseMenuActive = false;
    private bool moveSelected = false;

    private Transform initTransform;


    // Start is called before the first frame update
    void Start()
    {

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
    }
    public void moveToNewPos(Andor.Player player)
    {
        Vector3 playerPos = playerObjects[player.getNetworkID()].transform.position;
        Vector3 cellPos = tiles[Game.gameState.playerLocations[player.getNetworkID()]].getMiddle();
        playerObjects[player.getNetworkID()].transform.position = moveTowards(playerPos, cellPos, 0.5f);
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
            TileBounds tb = new TileBounds(temp.AddComponent<PolygonCollider2D>(), boardSpriteContainer.transform.lossyScale);
            Bounds b = tb.createBounds();
            Debug.Log(b);
            timeTileBounds.Add(Int32.Parse(sprite.name.Split('-')[1]), b);
            Destroy(temp);
        }

    }

    private void createBoardPosition(Sprite sprite)
    {
        int cellNumber = Int32.Parse(sprite.name.Split('_')[1]);

        GameObject cellObject = Instantiate(emptyPrefab, transform.position, transform.rotation, boardSpriteContainer);
        //cellObject.transform.localScale = boardSpriteContainer.transform.localScale;
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

    public void GameSetup()
    {
        // load players
        if (Game.gameState != null)
        {
            loadPlayers();

            loadMonsters();
        }

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

    private void loadMonsters()
    {
        //created all the monsters for Legend 2
        foreach (int gorTile in gorLocations())
        {
            Gor g = new Gor(Game.positionGraph.getNode(gorTile));
            Game.gameState.addMonster(g);
            Game.gameState.addGor(g);
        }
        foreach (int skralTile in skralLocations())
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
        }

    }

    private int[] gorLocations()
    {
        return new int[]{
            8,
            20,
            21,
            26,
            48
        };
    }

    private int[] skralLocations()
    {
        return new int[]{
            9,
        };
    }


    public void setTime(string PlayerID, int hour)
    {
        rndPosInTimeBox[PlayerID] = getRandomPositionInBounds(timeTileBounds[hour], timeObjectBounds, new Vector3());
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
        Debug.Log("trade clicked");
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
