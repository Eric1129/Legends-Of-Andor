using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MerchantScreen : MonoBehaviour
{
    private Merchant merchant;
    private string client;
    private string saleItem;
    public Transform merchantBoard;

    public MerchantScreen()
    {
        client = Game.myPlayer.getNetworkID();
        int clientLoc = Game.gameState.playerLocations[client];
        merchant = Game.gameState.getMerchant(clientLoc);
    }

    public void displayAvailableItems()
    {
        merchantBoard.gameObject.SetActive(true);

    }

    public void itemClick(string item)
    {
        saleItem = item;
    }

    public void buyClick()
    {
        merchant.buyFromMerchant(saleItem, client);
    }

    public void close()
    {
        merchantBoard.gameObject.SetActive(false);
        client = "";
        saleItem = "";
    }
}
