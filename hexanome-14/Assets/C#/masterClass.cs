using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text.RegularExpressions;
using UnityEngine.U2D;

public class masterClass : MonoBehaviour
{

    // TODO: create/store a Dict<playerTag, playerState> object
    // then all state changes will be done in one place.
    // all hero code will just forward info Requests to master.playerStates

    [SerializeField]
    private SpriteAtlas heroAtlas;
    [SerializeField]
    private GameObject baseObject;

    private string currentlySelectedBoardPos;

    // this is used when instantiating hero's associated sphere objects
    private Vector3 sphereScale;

    private GameObject turnManagerObj;

    // private TurnManager turnManager;


    public void initMasterClass(string callingObjectTag, SpriteAtlas theHeroAtlas,  GameObject theBaseObject, string[] orderedPlayerIDs, string[] startingPositions)
    {
        GameObject callingObject = GameObject.FindWithTag(callingObjectTag);

        // don't allow child objects to manipulate this class.
        if (callingObject.transform.IsChildOf(transform))
            return;

        // set private variables
        heroAtlas = theHeroAtlas;
        baseObject = theBaseObject;

        setScriptParameters();
        createTurnManager(orderedPlayerIDs, startingPositions);
        loadBoard();

        // TODO: map the integers in startingPositions to vector3 positions
        // maybe the best idea is for each "Node" to store it's position
        // then we can do GameObject.FindWithTag(this.graphIndex)
        // then we can retrieve the vector3 location of the click and map
        // it to it's node.graphIndex to view information about the spot clicked

        // turnManager = new TurnManager(orderedPlayerIDs, startingPositions);
        createHeros();
        return;
    }

    private void setScriptParameters()
    {
        gameObject.tag = "Master";
        baseObject.tag = "based";
        // squished in z direction
        sphereScale = new Vector3(2.2f, 2.2f, 0.12f);

        // what to do about this? should be per player..
        // TODO: make a PlayerState Object that each player owns.
        // stores strength, willpower, screen, sphereTag, currentlySelectedBoardPos
        // all the mutable things.
        //
        // -- allows me to make simple common checks like don't move if currentlySelected = currentPosition
        // all within one class.
        //
        // -- actually a great idea since the cleaner the hero class's the better !
        currentlySelectedBoardPos = "8";
    }

    private void createTurnManager(string[] orderedPlayerIDs, string[] initialPositions)
    {
        // GameObject turnManagerObj = Instantiate(baseObject, transform.position, transform.rotation);
        // turnManagerObj.AddComponent<TurnManager>();
        // turnManagerObj.tag = "TurnManager";

        // TurnManager turnManager = turnManagerObj.GetComponent<TurnManager>();

        this.gameObject.AddComponent<TurnManager>();
        TurnManager tm = this.gameObject.GetComponent<TurnManager>();
        tm.initTurnManager(baseObject, orderedPlayerIDs, initialPositions);
    }


    // creates a object for each sprite ie game tile
    private void loadBoard()
    {
        Sprite[] sprites =  Resources.LoadAll<Sprite>("BoardSprites");
        // Requirement: have Resources/Sprites folder under Assets
        if (sprites == null)
            print("oh no");

        foreach(Sprite sprite in sprites)
        {
            int gameCellNumber = createBoardPosition(sprite);

            // ie hero is placed at game tile 8, Todo: not hard-code this
            // I am pretty sure we don't need this anymore -- should be handled by creating this class's priv turnManager
            // and passing in the initialPositions
            if (gameCellNumber == 8)
            {
            }
        }
    }

    private void createHeros()
    {
        // iterate over all players added to turnManager
        // and create a hero per player.
        TurnManager turnManager = gameObject.GetComponent<TurnManager>();
        foreach(string playerID in turnManager.playerTagsInOrder())
        {
            Debug.Log("player id: " +playerID);
            GameObject playerGameObject = GameObject.FindWithTag(playerID);
            Player currPlayer = playerGameObject.GetComponent<Player>();

            string heroTag = currPlayer.getHeroType();

            createHero(heroTag, currPlayer.getPlayerTag());
        }
    }


    // called in a loop, creates a object for each sprite ie game tile
    // and adds components: sprite, pollygonCollider, boardPosition script
    // and a LineRenderer
    private int createBoardPosition(Sprite sprite)
    {
        // the regex returns the first string of consecutive numbers in "sprite.name"
        int gameCellNumber = convertToInt(Regex.Match(sprite.name, @"\d+").Value);

        GameObject newGameObject = Instantiate(baseObject, transform.position, transform.rotation);
        newGameObject.tag = gameCellNumber.ToString();
        newGameObject.name = "position-" + gameCellNumber.ToString();

        newGameObject.AddComponent<SpriteRenderer>();
        SpriteRenderer spriteRenderer = newGameObject.GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = sprite;

        newGameObject.AddComponent<LineRenderer>();
        newGameObject.AddComponent<PolygonCollider2D>();
        newGameObject.AddComponent<BoardPosition>();

        BoardPosition boardPosition = newGameObject.GetComponent<BoardPosition>();
        boardPosition.init();
        // so that calling (begingSetup) method can optionally create a hero
        // at this position.
        return gameCellNumber;
    }




    // this used to overlay hero Images on their spheres, stopped working, fix one day.
    private void assignHeroIcon(string heroTag)
    {
        // GameObject heroObject = GameObject.FindWithTag(theirPlayersTag);
        // this adds the hero's image to their sphere:
        // Sprite heroSprite = heroAtlas.GetSprite(hero_name);
        // SpriteRenderer spriteRenderer = heroObject.GetComponent<SpriteRenderer>();
        // spriteRenderer.sprite = heroSprite;
    }


    private void createHero(string heroTag, string playerTag)
    {
        // create a turnmanager before this which will store the starting positions for each hero.

        // later will pass in the "middle" parameter of the boardPosition
        // object we created before calling this method.
        Vector3 position = new Vector3(-29.13f, 24.02f, 0.0f);
        // this is the initialPosition of this hero, see how it is passed to Instantiate below

        GameObject heroObject = Instantiate(baseObject, position, Quaternion.identity);
        heroObject.AddComponent<SpriteRenderer>();
        heroObject.AddComponent<Hero>();
        heroObject.tag = heroTag;


        // assignHeroIcon(heroObject.tag);
        // maybe change this to pass a ref heroObject, avoid multiple findWithTag calls!
        string sphereTag = createSphere(playerTag, heroObject.tag);

        GameObject playerObject = GameObject.FindWithTag(playerTag);
        // sets hero to be child of this player
        heroObject.transform.parent = playerObject.transform;

        GameObject sphereObject = GameObject.FindWithTag(sphereTag);
        Hero theHero = heroObject.GetComponent<Hero>();

        theHero.init(sphereObject.tag, heroTag);
    }


    private string createSphere(string playerTag, string heroTag)
    {
        GameObject sphereObject = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        // GameObject sphereObject = Instantiate(sphereObject, transform.position, Quaternion.identity);
        sphereObject.transform.localScale = sphereScale;
        sphereObject.SetActive(true);

        GameObject heroObject = GameObject.FindWithTag(heroTag);

        // sets the sphere as a child gameObject of the heroObject.
        sphereObject.transform.parent = heroObject.transform;

        sphereObject.tag = "Sphere-" + heroTag;
        return sphereObject.tag;
    }


    // called by boardPositions to check if they should call requestHighlight
    // they pass in their gameObject's tag
    // we use the tag to check if its already selected
    public bool isCurrentlySelected(string newlySelected)
    {
        // this should be obsolete and unnecessary now.

        // this should maybe be moved to player class since they all have a diff currently selected piece
        return newlySelected.Equals(currentlySelectedBoardPos);
    }

    // TODO: move this to playerState class since currentlySelectedBoardPos
    //       will be a component of the player's state.

    // called on MouseDown (a click) by the corresponding boardPiece
    // sets the persistently highlighted boardPiece to be the
    // calling boardPieces's.
    public void requestHighlight(string newlySelected)
    {
        GameObject previousSelection = GameObject.FindWithTag(currentlySelectedBoardPos);
        BoardPosition boardPosition = previousSelection.GetComponent<BoardPosition>();
        boardPosition.hideBorderHighlight();

        currentlySelectedBoardPos = newlySelected;

        // this means a player has clicked this piece.
        // we should then (globally?) select the region as highlighted in the players chosen color
        // if global, then other users can see where a player plans on moving and can say its a bad move or not.

        // locally we should defs show whatever relevant info at this point like the option to move
        // to this point if its their turn.

    }


    // note this is being moved to turnmanager.boardContents
    // public void setNewHeroLocation(Vector3 newLocation, string boardPositionNumber)
    // {
        // Todo: make this better for turns, need to take who clicked as input somehow
        // will do this by adding all polygonColliders to each players gameObject
        // then they can pass in their tags to this object so we can verify
        // which player is requesting to move

        // doesn't work anymore !! on purpose!!
        // GameObject currentTurnSphere = GameObject.FindWithTag("Sphere-" + turn);
        // currentTurnSphere.transform.position = newLocation;
    // }


    public void notifyClick()
    {
        // Todo: make this be where we check if the player who clicked may move their hero there.
    }



    private int convertToInt(string prev)
    {
        int newInt;
        bool success = Int32.TryParse(prev, out newInt);
        if(!success)
        {
            Console.WriteLine("StackTrace: '{0}'", Environment.StackTrace);
            print("\n --- Error converting int to string, check stackTrace.\n --- Called from convertToInt in 'masterClass.cs' script.");
            return -12345;
        }
        return newInt;
    }

}

    // maybe re-add this if we want to set players as the child of this script.

    // // this script somehow needs to access the player attributes created
    // // when the player signs up. (and pass them to player.setAttributes
    // private string createPlayer()
    // {
    //     GameObject playerObject = Instantiate(baseObject, transform.position, transform.rotation);
    //     playerObject.AddComponent<Player>();
    //     playerObject.AddComponent<Hero>();

    //     Player player = playerObject.GetComponent<Player>();
    //     bool isTheirTurn = false;
    //     if (playerCount == 0)
    //     // in this implementation, a random player goes first.
    //     {
    //         isTheirTurn = true;
    //     }
    //     // player count meant for what ?
    //     // maybe we can use it to arbitrarily assign player tags tho !
    //     player.setAttributes(playerCount, "iD-yo", isTheirTurn);

    //     // ie tag is "First", "Second", etc.
    //     // should change this since the turn-order changes every round.
    //     // Do we have something that assigns a unique player id yet?
    //     playerObject.tag = getPlayerTag();
    //     playerObject.name = "Player-" + playerObject.tag;

    //     // sets player to be child of this script
    //     playerObject.transform.parent = gameObject.transform;
    //     return playerObject.tag;
    // }

