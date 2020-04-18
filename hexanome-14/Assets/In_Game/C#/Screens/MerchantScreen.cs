using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;
public class MerchantScreen : MonoBehaviour
{
    private Merchant merchant;
    private string client;
    private string saleItem;
    public Transform merchantBoard;
    public Transform equipBoard;
    

    public MerchantScreen()
    {
        //client = Game.myPlayer.getNetworkID();
        //int clientLoc = Game.gameState.playerLocations[client];
        //merchant = Game.gameState.getMerchant(clientLoc);
        
    }

    public void displayAvailableItems()
    {
        client = Game.myPlayer.getNetworkID();
        int clientLoc = Game.gameState.playerLocations[client];
        merchant = Game.gameState.getMerchant(clientLoc);
        merchantBoard.gameObject.SetActive(true);
        updateStatusOfInventory();
        updateGold();

    }

    public void updateGold()
    {
        GameObject gold = GameObject.Find("HeroGold");
        Text heroGold = gold.GetComponent<Text>();
        heroGold.text = Game.gameState.getPlayer(client).getHero().getGold().ToString();
        if (Game.gameState.getPlayer(client).getHero().getGold() < 2)
        {
            foreach (string key in Game.gameState.getEquipmentBoard().Keys)
            {
                List<Article> articles = Game.gameState.getArticlesOfType(key);
                int quantity = articles.Count;

                Button articleBuyButton = GameObject.Find("Buy" + key).GetComponent<Button>();
                articleBuyButton.interactable = false;

                Text articleText = GameObject.Find(key).GetComponent<Text>();
                UnityEngine.Color zm = articleText.color;  //  makes a new color zm
                zm.a = 0.6f; // makes the color zm transparent
                articleText.color = zm;


            }
            Button strengthBuyButton = GameObject.Find("BuyStrength").GetComponent<Button>();
            strengthBuyButton.interactable = false;
        }
    }

    
    public void updateStatusOfInventory()
    {
        
        foreach(string key in Game.gameState.getEquipmentBoard().Keys)
        {
            List<Article> articles = Game.gameState.getArticlesOfType(key);
            int quantity = articles.Count;

            if(quantity > 0)
            {
                string articleName = articles[0].articleToString();
                Text articleText = GameObject.Find(key).GetComponent<Text>();
                articleText.text = quantity + " x " + articleName;
            }
            else
            {
                string articleName = key;
                Button articleBuyButton = GameObject.Find("Buy" + key).GetComponent<Button>();
                articleBuyButton.interactable = false;

                Text articleText = GameObject.Find(key).GetComponent<Text>();
                articleText.text = quantity + " x " + articleName;
                UnityEngine.Color zm = articleText.color;  //  makes a new color zm
                zm.a = 0.6f; // makes the color zm transparent
                articleText.color = zm;
            }
           
        }
    }

    public void close()
    {
        merchantBoard.gameObject.SetActive(false);
        client = "";
        
    }

    

    public void buyClick(string item)
    {
        saleItem = item;
        merchant.buyFromMerchant(client, saleItem);
        displayAvailableItems();
    }

    public void learnMore()
    {
        equipBoard.gameObject.SetActive(true);
    }

    public void back()
    {
        equipBoard.gameObject.SetActive(false);
    }
    
}
