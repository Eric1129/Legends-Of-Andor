using System;
using Andor;
using UnityEngine;

public abstract class PickDrop : MonoBehaviour, Interactable, TileObject
{
    private bool pickedUp = false;
    protected Node location = null;
    private int interactID;
    protected GameObject prefab;
    public string name;

    public PickDrop(Node location, GameObject prefab, bool pickedUp, string name)
    {
        this.location = location;
        this.prefab = prefab;
        this.pickedUp = pickedUp;
        this.name = name;
    }

    public PickDrop(Node location, bool pickedUp, string name)
    {
        this.location = location;
        this.pickedUp = pickedUp;
        this.name = name;
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
            DroppedSpecial();
        }
        else            // Picking up the object
        {
            pickedUpSpecial();
            hide();
            Game.gameState.addPlayerInteractable(player.getNetworkID(), this);
            location.removeInteractable(this);
        }

        pickedUp = !pickedUp;
    }
    public abstract void pickedUpSpecial();
    public abstract void DroppedSpecial();

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
        
        if(this.name == "gold1")
        {
            Instantiate(GameController.instance.gold1, GameController.instance.tiles[Game.gameState.getPlayerLocations()[Game.myPlayer.getNetworkID()]].getMiddle(), transform.rotation);
        }
        else
        {
            this.prefab.GetComponent<Renderer>().enabled = true;
        }
    }

    public void hide()
    {
        this.prefab.GetComponent<Renderer>().enabled = false;
    }
}
