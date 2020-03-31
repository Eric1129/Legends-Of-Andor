using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnManager : MonoBehaviour
{

    // need a seperate queue for this to keep track of order players end their day in.
    private Queue sunriseBox;
    // maybe this is in a sunriseBox script ??
    private Queue turnOrder;
    // keys are player tags, values are their positions
    private BoardContents boardContents;


    public void initTurnManager(GameObject baseObject, string[] orderPlayers, string[] initialPositions)
    {
        gameObject.AddComponent<BoardContents>();
        boardContents = gameObject.GetComponent<BoardContents>();


        sunriseBox = new Queue();
        turnOrder = new Queue();
        initParams(orderPlayers, initialPositions);
    }


    private void initParams(string[] orderPlayers, string[] initialPositions)
    {
        Dictionary<string, string> playerPositions = new Dictionary<string, string>();
        for(int i = 0; i < orderPlayers.Length; i++)
        {
            string playerTag = orderPlayers[i];
            // fill turnorder queue with the tags of players.
            turnOrder.Enqueue(playerTag);

            // this class will store all known positions of heros.
            playerPositions[playerTag] = initialPositions[i];
        }
        boardContents = BoardContents.initAndGet(playerPositions);
        // boardContents.setPlayerPositions();
    }


    public void clickedBoardPosition(string boardPosTag, string playerTag, bool isPrince)
    {
    }


    public bool isTurn(string playerTag)
    {
        bool is_turn = ((string)turnOrder.Peek() == playerTag) ? true : false;
        return is_turn;
    }

    public IEnumerable<string> playerTagsInOrder()
    {
        foreach(string playerTag in turnOrder)
        {
            yield return playerTag;
        }
    }


    public bool alreadyOnSpace(string playerTag, string boardPosTag)
    {
        foreach(KeyValuePair<string, string> kvp in boardContents.getPlayerPos())
        {
            if (playerTag == kvp.Key && boardPosTag == kvp.Value)
                // then this player is already on this tile.
                // maybe there is a menu screen we should show them regardless.
                return true;
        }
        return false;
    }


    // not sure if this is needed, if I recall correctly the prince's position is bound to a player
    // and therefore only one player may move him (when it's this player's turn)
    // public bool movePrince(string playerTag, string boardPosTag)


    // maybe I should change this to return a string
    // then can return that the requested location is 1 hour too far etc.
    public bool canMoveToClickedSpot(string playerTag, string boardPosTag)
    {
        // this function returns true if the player is allowed to move to
        // the boardPosition represented by boardPosTag.
        if ( !isTurn(playerTag) || alreadyOnSpace(playerTag, boardPosTag) ){ return false; }

        // now do a bfs to calc the distance to spot
        // need to skip if the position is 0

        // int hoursInMove = boardGraph.getDistance(currentPos, boardPosTag);
        return true;
    }

}
