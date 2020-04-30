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

    public override void DroppedSpecial()
    {
        if(this.location.getIndex() == 0)
        {
            Game.gameState.maxMonstersAllowedInCastle++;
        }
        Debug.Log("Max Monsters: " + Game.gameState.maxMonstersAllowedInCastle);
    }

    public override void pickedUpSpecial()
    {
        if (this.location.getIndex() == 0)
        {
            Game.gameState.maxMonstersAllowedInCastle--;
        }
        Debug.Log("Max Monsters: " + Game.gameState.maxMonstersAllowedInCastle);
    }
}
