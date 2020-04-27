using System;
using Andor;
using UnityEngine;

public class PickDrop : Interactable, TileObject
{
    private bool pickedUp = false;
    private Node location = null;
    private int interactID;
    private GameObject prefab;

    public PickDrop(Node location, GameObject prefab, bool pickedUp)
    {
        this.location = location;
        this.prefab = prefab;
        this.pickedUp = pickedUp;
    }


    public void interact(Player player)
    {
        if (pickedUp)   // Dropping the object
        {
            display();
            Game.gameState.removePlayerInteractable(player.getNetworkID(), this);
            location = Game.gameState.positionGraph.getNode(Game.gameState.getPlayerLocations()[player.getNetworkID()]);
            prefab.transform.position = GameController.instance.tiles[location.getIndex()].getMiddle();
            location.addInteractable(this);
        }
        else            // Picking up the object
        {
            hide();
            Game.gameState.addPlayerInteractable(player.getNetworkID(), this);
            location.removeInteractable(this);
        }

        pickedUp = !pickedUp;
    }

    public int getInteractableID()
    {
        return interactID;
    }

    public void setInteractableID(int id)
    {
        interactID = id;
    }

    public void display()
    {
        this.prefab.GetComponent<Renderer>().enabled = true;
    }

    public void hide()
    {
        this.prefab.GetComponent<Renderer>().enabled = false;
    }
}
