using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : Fightable, MoveStrategy
{
    private Node location;
    protected Object prefab;

    public Monster(Node startingPos, Object prefab)
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
    private Object getPrefab()
    {
        return prefab;
    }

}
