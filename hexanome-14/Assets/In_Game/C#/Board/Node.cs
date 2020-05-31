using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node
{

    // this is the same as the tag: ie 1||2, is the position on board.
    private int graphIndex;
    private List<Node> adjacentNodes;
    private List<Interactable> interactables;


    public Node(int index)
    {
        graphIndex = index;
        this.adjacentNodes = new List<Node>();
        interactables = new List<Interactable>();
    }
    public void addAdjacentNode(Node adjacentNode)
    {
        adjacentNodes.Add(adjacentNode);
    }


    public Node toCastleNode()
    {
        return adjacentNodes[0];
    }

    public List<Node> getAdjacentNodes()
    {
        return adjacentNodes;
    }


    public int getIndex()
    {
        return graphIndex;
    }
    public override string ToString()
    {
        return "Node: " + graphIndex;
    }

    public void addInteractable(Interactable interactable)
    {
        interactables.Add(interactable);
        interactable.setInteractableID(interactables.Count - 1);
    }
    public List<Interactable> getInteractables()
    {
        return interactables;
    }
    public void moveInteractable(Interactable interactable, Node node)
    {
        node.addInteractable(interactable);
        removeInteractable(interactable);
    }
    public void removeInteractable(Interactable interactable)
    {
        interactables.Remove(interactable);

        // Update interactable IDs
        for (int i = 0; i < this.interactables.Count; i++)
        {
            interactables[i].setInteractableID(i);
        }
    }

}
