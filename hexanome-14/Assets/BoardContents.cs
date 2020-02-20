using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.Collections;

public class BoardContents : Graph
{

    /* This class inherits graph, a class which provides basic functions
     * for querying distance between positions on the board.
     *
     * The purpose of this child class is to do ^ operations
     * and store per position information such as the locations
     * of all wells, players.
     * */

    // store: (player tag, current boardPosition tag) for each player to keep track of their positions
    private Dictionary<string, string> playerPositions;
    private Dictionary<string, string> monsterPositions;

    private static BoardContents _singleton;

    // wanted to use arraylist but the microsoft docs say it sucks, recommended this.
    // private List<Usable> articlesOnPos;
    // this should be a dictionary: <int, List<Usable>>

    private bool initialized = false;

    public static BoardContents getInstance
    {
        get { return _singleton; }
    }


    // called when a script with this object attached is loaded (before Start())
    private void Awake()
    {
        if (_singleton != null && _singleton != this)
        {
        // only allow one gameObject with this script as a component
            Destroy(this.gameObject);
            return;
        }

        _singleton = this;
        // this allows this gameObject to persist between different scenes.
        DontDestroyOnLoad(this.gameObject);
    }



    // this private constructor just calls the superclass constructor
    private BoardContents() : base()
    {
    }

    public static BoardContents initAndGet(Dictionary<string, string> playerPos)
    {
        _singleton.init(playerPos);
        return _singleton;
    }

    public string getPlayerPosition(string playerTag)
    {
        return _singleton.playerPositions[playerTag];
    }


    public void setNewPlayerPosition(string playerTag, string posTag)
    {
        _singleton.playerPositions[playerTag] = posTag;
    }


    private void init(Dictionary<string, string> playerPos)
    {
        playerPositions = playerPos;
        initialized = true;
    }


    // the ienumerable just allows me to do:
    // foreach(KeyValuePair<string, string> kvp in boardContents.getPlayerPos())
    // in the turnmanager class
    public IEnumerable<KeyValuePair<string, string>> getPlayerPos()
    {
        foreach(KeyValuePair<string, string> kvp in playerPositions)
        {
            yield return kvp;
        }
    }
}
