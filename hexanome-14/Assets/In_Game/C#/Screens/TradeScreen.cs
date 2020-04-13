using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;


public class TradeScreen : MonoBehaviour
{
    public Transform selectTradeType;
    public Transform selectHeroTrade;
    public Transform selectHeroGive;
    public Transform unavailable;

    private bool selectHeroTradeActive;
    private bool selectHeroGiveActive;
    int tradeTypeIndex;
    string[] playersInvolved;
    string selectedHero;
    string giveItem;
    string receiveItem;

    public TradeScreen()
    {
        //GameObject tradeContainer = GameObject.Find("TradeContainer");
        //Transform[] screens = tradeContainer.GetComponentsInChildren<Transform>(true);
        //foreach (Transform screen in screens)
        //{
        //    if (screen.name == "SelectTradeType")
        //    {
        //        selectTradeType = screen;
        //    }
        //    //if (screen.name == "SelectHeroGive")
        //    //{
        //    //    selectHeroGive = screen;
        //    //}

        //    //if (screen.name == "SelectHeroTrade")
        //    //{
        //    //    selectHeroTrade = screen;
        //    //}
        //}
    }

    public void displayTradeType()
    {
        GameController.instance.selectTradeType.gameObject.SetActive(true);
    }
    public void nextClick()
    {
        Debug.Log("next clicked");
        displayHeroTrade();

    }

    public void displayHeroTrade()
    {

        if (tradeTypeIndex == 0)
        {
            GameController.instance.selectHeroTrade.gameObject.SetActive(true);
            selectHeroTradeActive = true;
        }
        else
        {
            GameController.instance.selectHeroGive.gameObject.SetActive(true);
            selectHeroGiveActive = true;
            GameObject parentObj = GameObject.Find("SelectHero");
            Transform[] trs = parentObj.GetComponentsInChildren<Transform>();
            foreach(Transform t in trs)
            {
                if(t.name == "HeaderText")
                {
                    Text giveText = t.gameObject.GetComponent<Text>();
                    string type = "";
                    if(tradeTypeIndex == 1)
                    {
                        type = "gold";
                    }

                    if(tradeTypeIndex == 2)
                    {
                        type = "a gemstone";
                    }
                    giveText.text = "Select a hero to give " +type +" to:";
                }
            }
        }
        
        //get all players on the same location
        //get the player that clicked on trade button
        int myLocation = 0;
        playersInvolved = new string[4];
        int i = 1;
        playersInvolved[0] = Game.myPlayer.getNetworkID();
        bool playersAvail = false;
        if (Game.gameState.playerLocations.TryGetValue(Game.myPlayer.getNetworkID(), out myLocation))
        {
            foreach (Andor.Player p in Game.gameState.getPlayers())
            {
                int playerLocation = 0;
                if (Game.gameState.playerLocations.TryGetValue(p.getNetworkID(), out playerLocation))
                {
                    if (playerLocation == myLocation && !Game.myPlayer.Equals(p))
                    {
                        playersInvolved[i] = p.getNetworkID();
                        displayPlayerInfo(p, i);
                        i++;
                        playersAvail = true;

                    }

                }
            }

            if (!playersAvail)
            {
                displayUnavailablePlayers();
            }
        }
    }
    public void displayPlayerInfo(Andor.Player player, int i)
    {

        GameObject selectHero = GameObject.Find("SelectHero");
        GameObject herogameobj;
        Transform[] trs = selectHero.GetComponentsInChildren<Transform>(true);
        //Transform[] heroattr = new Transform[3];
        foreach (Transform t in trs)
        {

            if (t.name == ("Hero" + i))
            {
                herogameobj = t.gameObject;
                t.gameObject.SetActive(true);
                Transform[] heroattr = herogameobj.GetComponentsInChildren<Transform>(true);
                foreach (Transform attr in heroattr)
                {
                    attr.gameObject.SetActive(true);
                    if (attr.name == "Name")
                    {

                        Text heroname = attr.GetComponent<Text>();
                        heroname.text = player.getHeroType();
                    }
                    if (attr.name == "Image")
                    {
                        Debug.Log("Image");
                        Sprite herosprite = Resources.Load<Sprite>("PlayerSprites/" + player.getHeroType());
                        attr.GetComponent<Image>().sprite = herosprite;
                    }
                    if (attr.name == "HeroItems")
                    {
                        Debug.Log("Hero items");
                        Text heroitems = attr.GetComponent<Text>();
                        heroitems.text = "Gold: " + player.getHero().getGold() + "\n";
                        heroitems.text += "\nGemstones: " + player.getHero().getGemstone() + "\n";
                        heroitems.text += "\nArticles: ";
                        List<string> heroAr = player.getHero().getArticles();
                        foreach (string ar in heroAr)
                        {
                            heroitems.text += (ar + " ");
                        }
                    }
                }
            }
        }


    }

    public void back()
    {
        if (selectHeroTradeActive)
        {
            selectHeroTrade.gameObject.SetActive(false);
            selectHeroTradeActive = false;
        }
        if (selectHeroGiveActive)
        {
            selectHeroGive.gameObject.SetActive(false);
            selectHeroGiveActive = false;
        }
    }

    public void setTradeType(int menuIndex)
    {
        
        tradeTypeIndex = menuIndex;
        Debug.Log("setting trade type" + tradeTypeIndex);
        
        //Debug.Log(tradeTypeIndex);

    }


    public void displayUnavailablePlayers()
    {
        Debug.Log("unavailable");
        unavailable.gameObject.SetActive(true);
        selectTradeType.gameObject.SetActive(false);
        selectHeroTrade.gameObject.SetActive(false);
        selectHeroGive.gameObject.SetActive(false);


    }

    //add to each Hero[i]
    public void onHeroClick(int i)
    {
        int index = i;
        selectedHero = playersInvolved[index];
        if (tradeTypeIndex == 0)
        {
            //update the dropdowns
            updateDropdowns();
        }

        //Game.sendAction(new InitiateTrade(playersInvolved));

    }

    public void updateDropdowns()
    {

        List<string> myArticles = Game.myPlayer.getHero().getArticles();
        Andor.Player selectedPlayer = Game.gameState.getPlayer(selectedHero);
        List<string> heroArticles = selectedPlayer.getHero().getArticles();

        
        GameObject parentObj = GameObject.Find("SelectHero");
        Transform[] trs = parentObj.GetComponentsInChildren<Transform>(true);
        foreach(Transform t in trs)
        {
            if(t.name == "myArticles")
            {
                
                Dropdown myArticlesMenu = t.gameObject.GetComponent<Dropdown>();
                myArticlesMenu.ClearOptions();
                myArticlesMenu.AddOptions(myArticles);
            }
            if (t.name == "heroArticles")
            {
                t.gameObject.SetActive(true);
                Dropdown heroArticlesMenu = t.gameObject.GetComponent<Dropdown>();
                heroArticlesMenu.ClearOptions();
                heroArticlesMenu.AddOptions(heroArticles);
            }
            if(t.name == "HeroArText")
            {
                t.gameObject.SetActive(true);
                Text heroArText = t.gameObject.GetComponent<Text>();
                heroArText.text = selectedPlayer.getHeroType() + " articles";
            }
            if(t.name == "SendRequest")
            {
                Button sendRequestButton = t.gameObject.GetComponent<Button>();
                sendRequestButton.interactable = true;
            }
        }
        
    }

    //add to myArticles dropdown obj
    public void setGiveItem(int val)
    {
        GameObject parentObj = GameObject.Find("SelectHero");
        Transform[] trs = parentObj.GetComponentsInChildren<Transform>(true);
        foreach (Transform t in trs)
        {
            if (t.name == "myArticles")
            {
                Dropdown myArticlesMenu = t.gameObject.GetComponent<Dropdown>();
                List<Dropdown.OptionData> menuOptions = myArticlesMenu.GetComponent<Dropdown>().options;
                giveItem = menuOptions[val].text;
            }

        }

    }


    //add to heroArticles dropdown obj
    public void setReceiveItem(int val)
    {
        GameObject parentObj = GameObject.Find("SelectHero");
        Transform[] trs = parentObj.GetComponentsInChildren<Transform>(true);
        foreach (Transform t in trs)
        {
            if (t.name == "heroArticles")
            {
                Dropdown myArticlesMenu = t.gameObject.GetComponent<Dropdown>();
                List<Dropdown.OptionData> menuOptions = myArticlesMenu.GetComponent<Dropdown>().options;
                receiveItem = menuOptions[val].text;
            }

        }
    }
   
    public void sendRequest()
    {
        Debug.Log("sending request!");
        string[] players = new string[2];
        players[0] = Game.myPlayer.getNetworkID();
        players[1] = selectedHero;
        string[] tradeType = new string[3];

        switch (tradeTypeIndex)
        {
            case 0:
                tradeType[0] = "trade";
                tradeType[1] = giveItem;
                tradeType[2]= receiveItem;
                break;
            case 1: tradeType[0] = "Gold";
                break;
            case 2: tradeType[0] = "Gemstones";
                break;
            default: tradeType[0] = "";
                break;
        }
       
        Game.sendAction(new InitiateTrade(players, tradeType));
    }

    public void closeTradeMenu()
    {
        selectTradeType.gameObject.SetActive(false);
        selectHeroGive.gameObject.SetActive(false);
        selectHeroTrade.gameObject.SetActive(false);
        unavailable.gameObject.SetActive(false);
        tradeTypeIndex = -1;
    }
}



    
