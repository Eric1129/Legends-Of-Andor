using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{

    public static GameController instance;

    public Transform boardSpriteContainer;
    public Transform playerContainer;
    public Button moveButton;

    public GameObject emptyPrefab;
    public GameObject playerPrefab;
    public Sprite fullBoardSprite;

    public Dictionary<int, BoardPosition> tiles;
    public Dictionary<string, GameObject> playerObjects;


    private bool moveSelected = false;

    // Start is called before the first frame update
    void Start()
    {

        Sprite sprite = Resources.Load<Sprite>("TimeSprites/time-1");
        
        GameObject sObject = Instantiate(emptyPrefab, boardSpriteContainer);
        SpriteRenderer sr = sObject.AddComponent<SpriteRenderer>();
        sr.sprite = sprite;

        Debug.Log(transform.position);
        

        Game.started = true;
        Game.createPV();

        tiles = new Dictionary<int, BoardPosition>();
        playerObjects = new Dictionary<string, GameObject>();

        instance = this;

        loadBoard();
    }

    void Update()
    {
        foreach(Andor.Player player in Game.gameState.getPlayers())
        {
            moveToNewPos(player);
        }
    }
    public void moveToNewPos(Andor.Player player)
    {
        Vector3 playerPos = playerObjects[player.getNetworkID()].transform.position;
        Vector3 cellPos = tiles[Game.gameState.playerLocations[player.getNetworkID()]].getMiddle();
        playerObjects[player.getNetworkID()].transform.position =
            new Vector3(Mathf.MoveTowards(playerPos.x, cellPos.x, 1), Mathf.MoveTowards(playerPos.y, cellPos.y, 1), -1);
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
            print("Could not load sprites");

        foreach (Sprite sprite in sprites)
        {
            createBoardPosition(sprite);
        }


        // load players
        loadPlayers();

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
            GameObject playerObject = Instantiate(playerPrefab, playerContainer);
            playerObjects.Add(player.getNetworkID(), playerObject);
            SpriteRenderer spriteRenderer = playerObject.GetComponent<SpriteRenderer>();
            
            spriteRenderer.sprite = Resources.Load<Sprite>("PlayerSprites/" + player.getHeroType());


            // Give a random position
            int startingTile = Game.RANDOM.Next(20, 40);
            playerObject.transform.position = tiles[startingTile].getMiddle();
            Game.getGame().playerLocations.Add(player.getNetworkID(), startingTile);
        }
    }

    /*public void movePlayer(string player, int tile)
    {
        Debug.Log("Moving Player (" + player + ") to tile " + tile);
        Vector3 middle = tiles[tile].getMiddle();
        playerObjects[player].transform.position = new Vector3(middle.x, middle.y, -10);
    }*/

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
    public void fightClick()
    {
        Debug.Log("fight");
    }
    public void passClick()
    {
        Debug.Log("pass");
    }
    public void endDayClick()
    {
        Debug.Log("end day");
    }
    public void tradeClick()
    {
        Debug.Log("trade");
    }


    #endregion


}
