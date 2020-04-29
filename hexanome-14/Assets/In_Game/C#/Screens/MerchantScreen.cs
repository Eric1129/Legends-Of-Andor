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
        Debug.Log("FOUND GOLD");
        Text heroGold = gold.GetComponent<Text>();
        heroGold.text = Game.gameState.getPlayer(client).getHero().getGold().ToString();
        Debug.Log("GOLD: " + Game.gameState.getPlayer(client).getHero().getGold());
        if (Game.gameState.getPlayer(client).getHero().getGold() < 2)
        {
            foreach (string key in Game.gameState.getEquipmentBoard().Keys)
            {

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
        int small_articles = 0;
        foreach(String key in Game.myPlayer.getHero().allArticlesAsStringList())
        {
            if(key == "Wineskin")
            {
                small_articles += Game.myPlayer.getHero().getAllArticles()["Wineskin"].Count;
            }
            if (key == "Telescope")
            {
                small_articles += Game.myPlayer.getHero().getAllArticles()["Telescope"].Count;
            }
            if (key == "WitchBrew")
            {
                small_articles += Game.myPlayer.getHero().getAllArticles()["WitchBrew"].Count;
            }
        }

        int helm = 0;
        if (Game.myPlayer.getHero().hasArticle("Helm"))
        {
            helm = Game.myPlayer.getHero().getAllArticles()["Helm"].Count;
        }

        int large_articles = 0;
        foreach (String key in Game.myPlayer.getHero().allArticlesAsStringList())
        {
            if (key == "Shield")
            {
                large_articles += Game.myPlayer.getHero().getAllArticles()["Shield"].Count;
            }
            if (key == "Falcon")
            {
                large_articles += Game.myPlayer.getHero().getAllArticles()["Falcon"].Count;
            }
            if (key == "Bow")
            {
                large_articles += Game.myPlayer.getHero().getAllArticles()["Bow"].Count;
            }
        }


        //////////////////////////////////////////////////////////////////////////////////////////
        /////////////////////////////////fixed trade to fit storage/////////////////////////////
        foreach (string key in Game.gameState.getEquipmentBoard().Keys)
        {
            List<Article> articles = Game.gameState.getArticlesOfType(key);
            int quantity = articles.Count;

            if (quantity > 0)
            {
                string articleName = articles[0].articleToString();
                Text articleText = GameObject.Find(key).GetComponent<Text>();
                articleText.text = quantity + " x " + articleName;

                if(key != "WitchBrew")
                {
                    Debug.Log(large_articles + " " + small_articles);
                    bool valid = true;

                    if(key == "Shield")
                    {
                        if(large_articles >= 1)
                        {
                            valid = false;
                        }
                    }

                    if (key == "Falcon")
                    {
                        if (large_articles >= 1)
                        {
                            valid = false;
                        }
                    }
                    if (key == "Bow")
                    {
                        if (large_articles >= 1)
                        {
                            valid = false;
                        }
                    }
                    if (key == "Telescope")
                    {
                        if (small_articles >= 3)
                        {
                            valid = false;
                        }
                    }
                    if (key == "Wineskin")
                    {
                        if (small_articles >= 3)
                        {
                            valid = false;
                        }
                    }
                    if (key == "Helm")
                    {
                        if (helm >= 1)
                        {
                            valid = false;
                        }
                    }
                    //if ((large_articles >= 1 && (key != "Shield" && key != "Falcon" && key!="Bow")) || (small_articles >= 3 && (key != "Wineskin" && key != "Telescope" && key != "WitchBrew")) || (helm >=1 && (key != "Helm")) || large_articles < 1 || small_articles <3 || helm < 1){

                    if (valid)
                    {
                        Button articleBuyButton = GameObject.Find("Buy" + key).GetComponent<Button>();
                        articleBuyButton.interactable = true;
                        Debug.Log(key);
                    }
                    else
                    {
                        Button articleBuyButton = GameObject.Find("Buy" + key).GetComponent<Button>();
                        articleBuyButton.interactable = false;
                        articleText = GameObject.Find(key).GetComponent<Text>();
                        articleText.text = quantity + " x " + articleName;
                        UnityEngine.Color zm = articleText.color;  //  makes a new color zm
                        zm.a = 0.6f; // makes the color zm transparent
                        articleText.color = zm;
                    }
                        

                    
                }


    
            }
            else
            {
                string articleName = key;

                if (key != "WitchBrew")
                {
                    Button articleBuyButton = GameObject.Find("Buy" + key).GetComponent<Button>();
                    articleBuyButton.interactable = false;
                }
                    

                Text articleText = GameObject.Find(key).GetComponent<Text>();
                articleText.text = quantity + " x " + articleName;
                UnityEngine.Color zm = articleText.color;  //  makes a new color zm
                zm.a = 0.6f; // makes the color zm transparent
                articleText.color = zm;
            }

        }
    }

    public void resetBuyButtons()
    {
        foreach (string key in Game.gameState.getEquipmentBoard().Keys)
        {
            //List<Article> articles = Game.gameState.getArticlesOfType(key);
            //int quantity = articles.Count;
            if(key != "WitchBrew")
            {
                Debug.Log(key);
                Button articleBuyButton = GameObject.Find("Buy" + key).GetComponent<Button>();
                Debug.Log("Found button");
                articleBuyButton.interactable = true;
            }
           
            Text articleText = GameObject.Find(key).GetComponent<Text>();
            Debug.Log("Found text");
            UnityEngine.Color zm = articleText.color;  //  makes a new color zm
            zm.a = 1.0f; // makes the color zm transparent
            articleText.color = zm;


        }
        Button strengthBuyButton = GameObject.Find("BuyStrength").GetComponent<Button>();
        strengthBuyButton.interactable = true;
    }



    public void itemClick(string item)
    {
        saleItem = item;
    }

    //public void buyClick()
    //{
    //    merchant.buyFromMerchant(client, saleItem);
    //}

    public void close()
    {
        resetBuyButtons();
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
