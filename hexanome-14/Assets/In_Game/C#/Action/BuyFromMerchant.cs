using System;
using UnityEngine;

[System.Serializable]
public class BuyFromMerchant : Action
{
    string[] players;
    Type type;

    public BuyFromMerchant()
    {
        type = Type.BuyFromMerchant;
    }

    public Type getType()
    {
        return this.type;
    }
    public string[] playersInvolved()
    {
        return players;
    }

    public bool isLegal(GameState gs)
    {
        //only if they are on the same space as a merchant
        return true;
    }

    public void execute(GameState gs)
    {

    }

}
