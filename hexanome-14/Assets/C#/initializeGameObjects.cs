using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text.RegularExpressions;
using UnityEngine.U2D;

public class initializeGameObjects : MonoBehaviour
{
    private GameObject baseObject;
    private Vector3 sphereScale;

    public void init(GameObject baseObj, Vector3 sphere_scale)
    {
        baseObject = baseObj;
        sphereScale = sphere_scale;
    }

    public void createAll(string[] orderedPlayerIDs)
    {
        loadBoard();
        createHeros(orderedPlayerIDs);
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

    private void createHeros(string[] orderedPlayerIDs)
    {
        // iterate over all players added to turnManager
        // and create a hero per player.
        TurnManager turnManager = gameObject.GetComponent<TurnManager>();
        foreach(string playerID in orderedPlayerIDs)
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

        // GameObject heroObject = Instantiate(baseObject, position, Quaternion.identity);
        // heroObject.AddComponent<SpriteRenderer>();
        // heroObject.AddComponent<Hero>();
        // heroObject.tag = heroTag;


        // assignHeroIcon(heroObject.tag);
        // maybe change this to pass a ref heroObject, avoid multiple findWithTag calls!
        string sphereTag = createSphere(playerTag);

        GameObject playerObject = GameObject.FindWithTag(playerTag);
        // sets hero to be child of this player
        // heroObject.transform.parent = playerObject.transform;

        GameObject sphereObject = GameObject.FindWithTag(sphereTag);
        // Hero theHero = heroObject.GetComponent<Hero>();
        Player player = playerObject.GetComponent<Player>();
        player.setHero(getHeroObject(player.getHeroType()));
    }


    private Hero getHeroObject(string heroType)
    {
        switch(heroType)
        {
            case "Male-Archer":
                return new Archer();
            case "Male-Warrior":
                return new Warrior();
            case "Male-Dwarf":
                return new Dwarf();
            case "Male-Wizard":
                return new Wizard();
            default:
                Debug.Log("invalid hero type in getHeroObject switch statement");
                throw new System.ArgumentException("invalid hero type in getHeroObject switch statement");
                return null;
        }

    }


    private string createSphere(string playerTag)
    {
        GameObject sphereObject = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        // GameObject sphereObject = Instantiate(sphereObject, transform.position, Quaternion.identity);
        sphereObject.transform.localScale = sphereScale;
        sphereObject.SetActive(true);

        GameObject playerObject = GameObject.FindWithTag(playerTag);

        // sets the sphere as a child gameObject of the heroObject.
        sphereObject.transform.parent = playerObject.transform;
        Player player = playerObject.GetComponent<Player>();

        sphereObject.tag = "Sphere-" + player.getHeroType();
        return sphereObject.tag;
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
