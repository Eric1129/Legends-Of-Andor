using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Farmer : PickDrop
{
  

    public Farmer(Node startingPos, GameObject instance) : base(startingPos, instance, false)
    {
        startingPos.addInteractable(this);
    }

    private Farmer() : base(null, null, false) { }

}
