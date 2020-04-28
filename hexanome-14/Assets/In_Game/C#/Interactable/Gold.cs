using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gold : PickDrop
{
    private int value;

	public Gold(Node startingPos) : base(startingPos, true, "Gold")
	{
		startingPos.addInteractable(this);
        value = 0;
	}

    public void setGold(int gold)
    {
        this.value = gold;
    }

    public int getGold()
    {
        return this.value;
    }

    private Gold() : base(null, true, "NULL") { }

}
