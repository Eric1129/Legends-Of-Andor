using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node
{

    // this is the same as the tag: ie 1||2, is the position on board.
    private int graphIndex;
    private List<Node> adjacentNodes;


    public Node(int index)
    {
        graphIndex = index;
        this.adjacentNodes = new List<Node>();
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

}
