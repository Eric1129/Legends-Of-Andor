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
        createBoardAndHeroes(orderedPlayerIDs);
        createTurnManager(orderedPlayerIDs, startingPositions);

        // TODO: map the integers in startingPositions to vector3 positions
        // maybe the best idea is for each "Node" to store it's position
        // then we can do GameObject.FindWithTag(this.graphIndex)
        // then we can retrieve the vector3 location of the click and map
        // it to it's node.graphIndex to view information about the spot clicked

        // turnManager = new TurnManager(orderedPlayerIDs, startingPositions);
        return;
    }

    private void createBoardAndHeroes(string[] orderedPlayerIDs)
    {
        gameObject.AddComponent<initializeGameObjects>();
        initializeGameObjects creator = gameObject.GetComponent<initializeGameObjects>();

        creator.init(baseObject, sphereScale);
        creator.createAll(orderedPlayerIDs);

    }

    private void setScriptParameters()
    {
        gameObject.tag = "Master";
        baseObject.tag = "based";
        // squished in z direction
        sphereScale = new Vector3(2.2f, 2.2f, 0.12f);
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



}
