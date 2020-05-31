using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class Skral : Monster
{
    
    public Skral(Node startingLocation) : base(startingLocation, Resources.Load<GameObject>("Monster/Skral"))
    {
        
    }
}
