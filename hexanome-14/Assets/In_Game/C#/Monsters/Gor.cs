using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gor : Monster
{
    public Gor(Node startingLocation) : base(startingLocation, Resources.Load<GameObject>("Monster/Gor"))
    {

    }
}
