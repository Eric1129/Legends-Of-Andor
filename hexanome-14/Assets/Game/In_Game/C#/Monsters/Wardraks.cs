using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class Wardrak : Monster
{
    public Wardrak(Node startingLocation) : base(startingLocation, Resources.Load<GameObject>("Monster/Wardraks"))
    {
        
    }
    
}
