using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node :  IEnumerable<int>
{

    // this is the same as the tag: ie 1||2, is the position on board.
    private int graphIndex;

    private int[] neighbours;
    private Node prev;


    // these allow IEnum fxns allow us to iterate over a list of nodes using
    // a foreach loop
    public IEnumerator<int> GetEnumerator()
    {
        for(int i = 0; i < neighbours.Length; i++)
            yield return neighbours[i];
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }


    public Node(int index, int[] neighbz)
    {
        graphIndex = index;
        neighbours = neighbz;
        // prev is set to a diff value each Graph.bfs call
    }

    public ref Node getPrev()
    {
        return ref prev;
    }

    public void setPrev(ref Node previous)
    {
        prev = previous;
    }


    public int[] getNeighbours()
    {
        return neighbours;
    }


    public int getIndex()
    {
        return graphIndex;
    }

}
