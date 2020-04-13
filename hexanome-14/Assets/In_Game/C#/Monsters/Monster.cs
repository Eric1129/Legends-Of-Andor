using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : Fightable, MoveStrategy
{
    private Node location;
    protected GameObject prefab;

    public Monster(Node startingPos, GameObject prefab)
    {
        location = startingPos;
        this.prefab = prefab;
    }
    private Monster() { }

    public void attack(int damage)
    {
        throw new System.NotImplementedException();
    }

    public void move()
    {
        location = location.toCastleNode();
    }
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
