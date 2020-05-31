using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrinceThorald
{
    private Node location;
    protected GameObject prefab;

    public PrinceThorald(Node startingPos, GameObject prefab)
    {
        location = startingPos;
        this.prefab = prefab;
    }

    private PrinceThorald() { }

    public int getLocation()
    {
        return location.getIndex();
    }

    public void setLocationNode(Node x)
    {
        location = x;
    }

    public Node getLocationNode()
    {
        return location;
    }

    public GameObject getPrefab()
    {
        return prefab;
    }
    
}
