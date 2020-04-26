using System;
using System.Collections.Generic;

public class Merchant   
{
    public int location; 
    private Dictionary<string, List<Article>> inventory;

    public Merchant(int location)       
    {
        {
            this.location = location; 
            inventory = Game.gameState.getEquipmentBoard();
        }
    }


    public void buyFromMerchant(string client, string item) 
    {
        {
            //Game.sendAction(new BuyFromMerchant())
            string[] players = new string[1];
            players[0] = client;
            Game.sendAction(new BuyFromMerchant(players, item));
        }
    }
}	
