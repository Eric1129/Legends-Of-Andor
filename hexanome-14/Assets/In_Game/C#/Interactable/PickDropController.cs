using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PickDropController : MonoBehaviour
{
    public Transform pickUpContent;
    public Transform dropContent;

    public GameObject interactableDetailsPrefab;

    private List<GameObject> dropInteractableDetails;
    private List<Interactable> dropableItems;
    private List<GameObject> pickupInteractableDetails;
    private List<Interactable> pickUpItems;



    public void updateInteractables()
    {
        // Remove previous items
        for (int i = 0; i < dropContent.childCount; i++)
        {
            Destroy(dropContent.GetChild(i).gameObject);
        }
        for (int i = 0; i < pickUpContent.childCount; i++)
        {
            Destroy(pickUpContent.GetChild(i).gameObject);
        }

        // Initiate Drop from game state
        List<Interactable> interactables = Game.gameState.getInteractables(Game.myPlayer.getNetworkID());
        dropableItems = new List<Interactable>();
        dropInteractableDetails = new List<GameObject>();

        foreach (Interactable i in interactables)
        {
            if(i is PickDrop)
            {
                dropableItems.Add(i);
                GameObject temp = Instantiate(interactableDetailsPrefab, dropContent);
                dropInteractableDetails.Add(temp);
                temp.GetComponent<PickUpDetails>().label.text = ((PickDrop)i).name;
                Debug.Log("Adding to drop list: " + ((PickDrop)i).name);
            }
        }


        // Initiate Pick-Up from game state
        interactables = Game.gameState.positionGraph.getNode(Game.gameState.playerLocations[Game.myPlayer.getNetworkID()]).getInteractables();
        pickUpItems = new List<Interactable>();
        pickupInteractableDetails = new List<GameObject>();

        foreach (Interactable i in interactables)
        {
            if (i is PickDrop)
            {
                pickUpItems.Add(i);
                GameObject temp = Instantiate(interactableDetailsPrefab, pickUpContent);
                pickupInteractableDetails.Add(temp);
                temp.GetComponent<PickUpDetails>().label.text = ((PickDrop)i).name;

                Debug.Log("Adding to pick up list: " + ((PickDrop)i).name);
            }
        }


    }
    public void dropClick()
    {
        for(int i = 0; i<dropContent.childCount; i++)
        {
            if (dropContent.GetChild(i).GetComponent<PickUpDetails>().Selected)
            {
                Debug.Log("Dropping ITEM!");
                Game.sendAction(new Interact(Game.myPlayer.getNetworkID(), dropableItems[i].getInteractableID(), -1));
                break;
            }
        }
        updateInteractables();
    }

    public void pickupClick()
    {
        for (int i = 0; i < pickUpContent.childCount; i++)
        {
            if (pickUpContent.GetChild(i).GetComponent<PickUpDetails>().Selected)
            {
                Debug.Log("Picking up ITEM!");
                Game.sendAction(new Interact(Game.myPlayer.getNetworkID(), pickUpItems[i].getInteractableID(), Game.gameState.playerLocations[Game.myPlayer.getNetworkID()]));
                break;
            }
        }
        updateInteractables();
    }

    public void clickExit()
    {
        transform.parent.parent.gameObject.SetActive(false);
    }
}
