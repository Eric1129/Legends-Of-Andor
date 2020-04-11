using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Troll : Monster
{
    public Troll(Node startingLocation) : base(startingLocation, Resources.Load<GameObject>("Monster/Troll"))
    {
    }
}
