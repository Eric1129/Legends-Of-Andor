using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Farmer : PickDrop
{
  

    public Farmer(Node startingPos, GameObject instance) : base(startingPos, instance, false, "Farmer")
    {
        startingPos.addInteractable(this);
    }

    private Farmer() : base(null, null, false, "NULL") { }

}
