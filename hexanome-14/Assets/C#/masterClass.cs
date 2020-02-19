using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text.RegularExpressions;
using UnityEngine.U2D;

public class masterClass : MonoBehaviour
{

    [SerializeField]
    private GameObject sphere;
    [SerializeField]
    private SpriteAtlas heroAtlas;
    [SerializeField]
    private GameObject baseObject;

    private GameObject[] players;
    private int playerCount;
    private string turn;
    private string currentlySelectedBoardPos;
    private Vector3 sphereScale;



    public void initMasterClass(string callingObjectTag, GameObject theSphere, SpriteAtlas theHeroAtlas, GameObject theBaseObject)
    {
        GameObject callingObject = GameObject.FindWithTag(callingObjectTag);
        // don't allow child objects to manipulate this class.
        if (callingObject.transform.IsChildOf(transform))
            return;
        sphere = theSphere;
        // theHeroAtlas.
        heroAtlas = theHeroAtlas;
        baseObject = theBaseObject;

        beginSetup();

        return;
    }


    private void beginSetup()
    {
        // flat in z direction
        sphereScale = new Vector3(2.2f, 2.2f, 0.12f);
        currentlySelectedBoardPos = "8";
        players = new GameObject[8];
        // incremented each time a new player is created
        // and a player is added to this class's player[]
        playerCount = 0;
        turn = "First";

        Sprite[] sprites =  Resources.LoadAll<Sprite>("Sprites");
        // Requirement: have Resources/Sprites folder under Assets
        if (sprites == null)
            print("oh no");

        foreach(Sprite sprite in sprites)
        {
            createBoard(sprite);
        }

    }


    private void createBoard(Sprite sprite)
    {
        // the regex returns the first string of consecutive numbers in "sprite.name"
        int gameCellNumber = convertToInt(Regex.Match(sprite.name, @"\d+").Value);

        GameObject newGameObject = Instantiate(baseObject, transform.position, transform.rotation);
        newGameObject.tag = gameCellNumber.ToString();
        newGameObject.name = "position-" + gameCellNumber.ToString();
        newGameObject.transform.parent = gameObject.transform;

        newGameObject.AddComponent<SpriteRenderer>();
        //newGameObject.AddComponent<SpriteAtlas>();
        SpriteRenderer spriteRenderer = newGameObject.GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = sprite;
        newGameObject.AddComponent<LineRenderer>();


        newGameObject.AddComponent<PolygonCollider2D>();
        newGameObject.AddComponent<BoardPosition>();
        BoardPosition boardPosition = newGameObject.GetComponent<BoardPosition>();
        boardPosition.init();

        if (gameCellNumber == 8)
        {
            string hero_name = "Warrior";
            createHero(hero_name);
        }
    }


    private void createHero(string hero_name)
    {
        Vector3 position = new Vector3(-29.13f, 24.02f, 0.0f);
        Quaternion rotation = Quaternion.identity;

        GameObject heroObject = Instantiate(baseObject, position, rotation);
        GameObject playerObject = Instantiate(baseObject, transform.position, transform.rotation);
        GameObject sphereObject = GameObject.CreatePrimitive(PrimitiveType.Sphere);

        heroObject.AddComponent<SpriteRenderer>();
        heroObject.AddComponent<Hero>();

        // this adds the hero's image to their sphere
         Sprite heroSprite = heroAtlas.GetSprite(hero_name);
         SpriteRenderer spriteRenderer = heroObject.GetComponent<SpriteRenderer>();
         spriteRenderer.sprite = heroSprite;
       
            


        playerObject.AddComponent<Player>();
        playerObject.AddComponent<Hero>();

        Player player = playerObject.GetComponent<Player>();
        player.setParameters(playerCount, "player" + playerCount, true);
        playerObject.tag = getPlayerTagFromTurn();
        playerObject.name = "Player-" + playerObject.tag;

        heroObject.name = "Hero-" + playerObject.tag;

        // sets player to be child of this script
        playerObject.transform.parent = gameObject.transform;
        // and hero child of a player
        heroObject.transform.parent = player.transform;

        // GameObject sphereObject = Instantiate(sphere, position, rotation);
        // sphere base objects must be off by default or they will show before the map does
        sphereObject.transform.localScale = sphereScale;
        sphereObject.SetActive(true);

        sphereObject.tag = "sphere_childOf_" + playerObject.tag;
        sphereObject.transform.parent = heroObject.transform;

        Hero theHero = heroObject.GetComponent<Hero>();
        theHero.init(sphereObject, hero_name);

        players[playerCount] = playerObject;
        playerCount++;
    }


    public bool isCurrentlySelected(string newlySelected)
    {
        return newlySelected.Equals(currentlySelectedBoardPos);
    }


    // called on MouseDown (a click) by the corresponding boardPiece
    // sets the persistently highlighted boardPiece to be the
    // calling boardPieces's.
    public void requestHighlight(string newlySelected)
    {
        GameObject previousSelection = GameObject.FindWithTag(currentlySelectedBoardPos);
        BoardPosition boardPosition = previousSelection.GetComponent<BoardPosition>();
        boardPosition.hideBorderHighlight();

        currentlySelectedBoardPos = newlySelected;
    }


    public void setNewHeroLocation(Vector3 newLocation, string boardPositionNumber)
    {
        // Todo: make this better for turns, need to take who clicked as input somehow
        GameObject currentTurnSphere = GameObject.FindWithTag("sphere_childOf_" + turn);
        currentTurnSphere.transform.position = newLocation;
    }


    public void notifyClick()
    {
        // Todo: make this be when the hero is moved, not in above method
    }


    private string getPlayerTagFromTurn()
    {
        // I know we only have 4 players but maybe we all wanna play at once for testing
        switch(playerCount)
        {
            case 0:
                return "First";
            case 1:
                return "Second";
            case 2:
                return "Third";
            case 3:
                return "Fourth";
            case 4:
                return "Fifth";
            case 5:
                return "Sixth";
            case 6:
                return "Seventh";
            case 7:
                return "Eighth";
            default:
                return "BadInput!";
        }
    }


    private int convertToInt(string prev)
    {
        int newInt;
        bool success = Int32.TryParse(prev, out newInt);
        if(!success)
        {
            Console.WriteLine("StackTrace: '{0}'", Environment.StackTrace);
            print("\n --- Error converting int to string, check stackTrace");
            return -12345;
        }
        return newInt;
    }

}



    // void Start()
    // {
    //     currentlySelectedBoardPos = "8";
    //     players = new GameObject[8];
    //     // incremented each time a new player is created
    //     // and a player is added to this class's player[]
    //     playerCount = 0;
    //     turn = "First";
    //     Sprite[] sprites =  Resources.LoadAll<Sprite>("Sprites");
    //     // Requirement: have Resources/Sprites folder under Assets
    //     if (sprites == null)
    //         print("oh no");
    //     foreach(Sprite sprite in sprites)
    //     {
    //         createBoard(sprite);
    //     }
    // }


