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

    public Transform nextButton;
    public Transform actionText;
    public Transform tradeActionButton;
    public Button giveGoldActionButton;
    public Transform giveGemstoneActionButton;


    private bool selectHeroTradeActive;
    private bool selectHeroGiveActive;
    int tradeTypeIndex;
    //string[] e;
    List<string> eligibleTrade;
    List<string> eligibleGold;
    List<string> eligibleGemstone;
    string selectedHero;
    int giveItemIndex;
    int receiveItemIndex;

    private static List<string> myArticles;
    private static List<string> heroArticles;

    public string[] players;
    public string[] tradeType;

    public TradeScreen()
    {

        giveItemIndex = 0;
        receiveItemIndex = 0;
        tradeType = new string[3];
        players = new string[2];
        selectHeroTradeActive = false;
        selectHeroGiveActive = false;
        eligibleTrade = new List<string>();
        eligibleGold = new List<string>();
        eligibleGemstone = new List<string>();
    }


    public void displayTradeType()
    {
        selectTradeType.gameObject.SetActive(true);
        if (interactionPossible())
        {
            //check if trade is possible
            tradeActionButton.GetComponent<Button>().interactable = tradePossible();
            //check if gold is possible
            giveGoldActionButton.interactable = goldPossible();

            //check if gemstone is possible
            giveGemstoneActionButton.GetComponent<Button>().interactable = gemstonePossible();
        }
        else
        {
            actionText.GetComponent<Text>().text = "No actions available.";
        }
        Debug.Log("trade " + eligibleTrade.Count);
        Debug.Log("gold " + eligibleGold.Count);
        Debug.Log("gemstone " + eligibleGemstone.Count);

    }

    public bool interactionPossible()
    {
        //and set elible players
        bool interactionPossible = false;
        if (Game.myPlayer.getHero().hasArticle("Falcon"))
        {
            //has a falcon
            interactionPossible = true;
            foreach (Andor.Player p in Game.gameState.getPlayers())
            {
                if (!Game.myPlayer.Equals(p))
                {
                    //myPlayer can trade with all players as long as they have articles to give
                    if (p.getHero().allArticlesAsStringList().Count > 0)
                    {
                        eligibleTrade.Add(p.getNetworkID());
                    }
                    eligibleGold.Add(p.getNetworkID());
                    eligibleGemstone.Add(p.getNetworkID());
                }

            }
        }
        else
        {

            foreach (Andor.Player p in Game.gameState.getPlayers())
            {
                if (!Game.myPlayer.Equals(p))
                {
                    if (Game.gameState.playerLocations[p.getNetworkID()]
                    == Game.gameState.playerLocations[Game.myPlayer.getNetworkID()])
                    {
                        //if myPlayer doesn't have a falcon, they can interact with anyone who is on the same space
                        interactionPossible = true;
                        if (p.getHero().allArticlesAsStringList().Count > 0)
                        {
                            eligibleTrade.Add(p.getNetworkID());
                        }
                        eligibleGold.Add(p.getNetworkID());
                        eligibleGemstone.Add(p.getNetworkID());
                    }

                    else if (p.getHero().hasArticle("Falcon"))
                    {
                        //if myPlayer doesn't have a falcon, they can interact with anyone who has a falcon
                        interactionPossible = true;
                        if (p.getHero().allArticlesAsStringList().Count > 0)
                        {
                            eligibleTrade.Add(p.getNetworkID());
                        }

                    }
                }

            }
        }

        return interactionPossible;
    }


    public bool tradePossible()
    {
        bool trade = (Game.myPlayer.getHero().allArticlesAsStringList().Count > 0)
            && (eligibleTrade.Count > 0);
        Debug.Log("Trade possible  " + trade);
        //hero has something to trade and the other heroes have something to trade as well
        return trade;


    }


    public bool goldPossible()
    {

        //hero has gold
        return (Game.myPlayer.getHero().getGold() > 0);

    }

    public bool gemstonePossible()
    {
        return (Game.myPlayer.getHero().getGemstone() > 0);

    }

    public void setTradeType(int value)
    {

        tradeTypeIndex = value;
        actionText.gameObject.SetActive(true);
        switch (value)
        {
            case 0:
                actionText.GetComponent<Text>().text = "Action: trade";
                break;
            case 1:
                actionText.GetComponent<Text>().text = "Action: give gold";
                break;
            case 2:
                actionText.GetComponent<Text>().text = "Action: give gemstone";
                break;
            default:
                actionText.GetComponent<Text>().text = "Action: None selected";
                break;
        }
        nextButton.GetComponent<Button>().interactable = true;
        //Debug.Log(tradeTypeIndex);

    }
    public void nextClick()
    {
        if (tradeTypeIndex == 0)
        {
            //trade
            selectHeroTrade.gameObject.SetActive(true);
            int i = 1;
            foreach (string p in eligibleTrade)
            {
                Debug.Log("Eligible trade (nextClick()): " + Game.gameState.getPlayer(p).getHeroType());
                //display the hero
                displayPlayerInfo(Game.gameState.getPlayer(p), i);
                i++;
            }
        }
        else
        {
            //give
            selectHeroGive.gameObject.SetActive(true);
            GameObject parentObj = GameObject.Find("SelectHero");
            Transform[] trs = parentObj.GetComponentsInChildren<Transform>();
            foreach (Transform t in trs)
            {
                if (t.name == "HeaderText")
                {
                    Text giveText = t.gameObject.GetComponent<Text>();
                    string type = "";
                    if (tradeTypeIndex == 1)
                    {
                        type = "gold";
                    }

                    if (tradeTypeIndex == 2)
                    {
                        type = "a gemstone";
                    }
                    giveText.text = "Select a hero to give " + type + " to:";
                }
            }
            if (tradeTypeIndex == 1)
            {
                int i = 1;
                foreach (string p in eligibleGold)
                {
                    //display the hero
                    displayPlayerInfo(Game.gameState.getPlayer(p), i);
                    i++;
                }
            }

            if (tradeTypeIndex == 2)
            {
                int i = 1;
                foreach (string p in eligibleGemstone)
                {
                    //display the hero
                    displayPlayerInfo(Game.gameState.getPlayer(p), i);
                    i++;
                }
            }
        }
        //displayHeroTrade();


    }



    //public void displayHeroTrade()
    //{


    //    if (tradeTypeIndex == 0)
    //    {
    //        selectHeroTrade.gameObject.SetActive(true);
    //        selectHeroTradeActive = true;
    //    }
    //    else
    //    {

    //        selectHeroGive.gameObject.SetActive(true);
    //        selectHeroGiveActive = true;
    //        GameObject parentObj = GameObject.Find("SelectHero");
    //        Transform[] trs = parentObj.GetComponentsInChildren<Transform>();
    //        foreach(Transform t in trs)
    //        {
    //            if(t.name == "HeaderText")
    //            {
    //                Text giveText = t.gameObject.GetComponent<Text>();
    //                string type = "";
    //                if(tradeTypeIndex == 1)
    //                {
    //                    type = "gold";
    //                }

    //                if(tradeTypeIndex == 2)
    //                {
    //                    type = "a gemstone";
    //                }
    //                giveText.text = "Select a hero to give " +type +" to:";
    //            }
    //        }
    //    }

    //    //get all players on the same location
    //    //get the player that clicked on trade button
    //    if (!displayPlayers())
    //    {
    //        displayUnavailable("No available players", "There are no players available to trade wtih. You may trade if you are on the same tile as another player or if you or another player has a falcon.");
    //    }

    //    if(tradeTypeIndex == 1 && Game.myPlayer.getHero().getGold() < 1)
    //    {
    //        displayUnavailable("No Gold", "You do not have any gold to give.");
    //    }

    //    if (tradeTypeIndex == 2 && Game.myPlayer.getHero().getGemstone() < 1)
    //    {
    //        displayUnavailable("No gemstones", "You do not have any gemstones to give.");
    //    }
    //}

    //public bool displayPlayers()
    //{

    //    eligiblePlayers = new string[4];
    //    int i = 1;
    //    eligiblePlayers[0] = Game.myPlayer.getNetworkID();
    //    bool playersAvail = false;
    //    if (Game.myPlayer.getHero().hasArticle("Falcon"))
    //    {
    //        //if myPlayer has a falcon then all players are available
    //        foreach (Andor.Player p in Game.gameState.getPlayers())
    //        {
    //            eligiblePlayers[i] = p.getNetworkID();
    //            displayPlayerInfo(p, i);
    //            i++;
    //        }
    //        return true;
    //    }
    //    else
    //    {
    //        //otherwise need to check if they are on the same space or if anoother player has a falcon
    //        foreach (Andor.Player p in Game.gameState.getPlayers())
    //        {
    //            if (!Game.myPlayer.Equals(p))
    //            {
    //                if ((Game.gameState.playerLocations[p.getNetworkID()]
    //                == Game.gameState.playerLocations[Game.myPlayer.getNetworkID()]))
    //                {
    //                    //check on the same tile
    //                    eligiblePlayers[i] = p.getNetworkID();
    //                    displayPlayerInfo(p, i);
    //                    i++;
    //                    playersAvail = true;
    //                }
    //                else if (p.getHero().hasArticle("Falcon"))
    //                {
    //                    //check if the player has a falcon
    //                    eligiblePlayers[i] = p.getNetworkID();
    //                    i++;
    //                    playersAvail = true;

    //                }
    //            }

    //        }

    //    }


    //    //if (Game.gameState.playerLocations.TryGetValue(Game.myPlayer.getNetworkID(), out myLocation))
    //    //{
    //    //    foreach (Andor.Player p in Game.gameState.getPlayers())
    //    //    {
    //    //        int playerLocation = 0;
    //    //        if (Game.gameState.playerLocations.TryGetValue(p.getNetworkID(), out playerLocation))
    //    //        {
    //    //            if (playerLocation == myLocation && !Game.myPlayer.Equals(p))
    //    //            {
    //    //                eligiblePlayers[i] = p.getNetworkID();
    //    //                displayPlayerInfo(p, i);
    //    //                i++;
    //    //                playersAvail = true;


    //    //            }

    //    //        }
    //    //    }

    //    //}

    //    return playersAvail;
    //}

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
                        attr.GetComponent<Image>().useSpriteMesh = true;
                    }
                    if (attr.name == "HeroItems")
                    {
                        Debug.Log("Hero items");
                        Text heroitems = attr.GetComponent<Text>();
                        if (tradeTypeIndex == 0)
                        {
                            heroitems.text += "Articles: ";
                            heroitems.text += player.getHero().allArticlesAsString();
                        }
                        else if (tradeTypeIndex == 1)
                        {
                            heroitems.text = "Gold: " + player.getHero().getGold();
                        }
                        else
                        {
                            heroitems.text += "\nGemstones: " + player.getHero().getGemstone() + "\n";
                        }

                        //for(int j=0; j< heroAr.Count; j++)
                        //{
                        //    heroitems.text += heroAr[j];
                        //    if(j < heroAr.Count - 1)
                        //    {
                        //        heroitems.text += ", ";
                        //    }

                        //}
                    }
                }
            }
        }


    }

    public void back()
    {

        selectHeroTrade.gameObject.SetActive(false);
        selectHeroTradeActive = false;

        selectHeroGive.gameObject.SetActive(false);
        selectHeroGiveActive = false;

    }




    public void displayUnavailable(string title, string msg)
    {

        unavailable.gameObject.SetActive(true);
        Text title_text = unavailable.Find("Title").GetComponent<Text>();
        title_text.text = title;
        Text body_text = unavailable.Find("Body").GetComponent<Text>();
        body_text.text = msg;
        selectTradeType.gameObject.SetActive(false);
        selectHeroTrade.gameObject.SetActive(false);
        selectHeroGive.gameObject.SetActive(false);


    }

    //add to each Hero[i]
    public void onHeroClick(int index)
    {
        Debug.Log(index);
        if (tradeTypeIndex == 0)
        {
            selectedHero = eligibleTrade.ToArray()[index - 1];
        }
        else if (tradeTypeIndex == 1)
        {
            selectedHero = eligibleGold.ToArray()[index - 1];
        }
        else
        {
            selectedHero = eligibleGemstone.ToArray()[index - 1];
        }

        if (tradeTypeIndex == 0)
        {
            updateDropdowns();
            Debug.Log("trying to update dropdown");
        }

        GameObject parentObj = GameObject.Find("SelectHero");
        Transform[] trs = parentObj.GetComponentsInChildren<Transform>(true);
        foreach (Transform t in trs)
        {
            if (t.name == "SelectedHero")
            {
                Text selHerotxt = t.gameObject.GetComponent<Text>();

                selHerotxt.text = Game.gameState.getPlayer(selectedHero).getHeroType();
            }
            if (t.name == "SendRequest")
            {
                Button sendRequestButton = t.gameObject.GetComponent<Button>();
                sendRequestButton.interactable = true;
            }
        }


        //Game.sendAction(new InitiateTrade(playersInvolved));

    }

    public void updateDropdowns()
    {

        myArticles = Game.myPlayer.getHero().allArticlesAsStringList();
        Andor.Player selectedPlayer = Game.gameState.getPlayer(selectedHero);
        heroArticles = selectedPlayer.getHero().allArticlesAsStringList();


        GameObject parentObj = GameObject.Find("SelectHero");
        Transform[] trs = parentObj.GetComponentsInChildren<Transform>(true);
        foreach (Transform t in trs)
        {
            if (t.name == "myArticles")
            {

                Dropdown myArticlesMenu = t.gameObject.GetComponent<Dropdown>();
                myArticlesMenu.ClearOptions();
                myArticlesMenu.AddOptions(myArticles);
                //myArticlesMenu.GetComponent<Dropdown>().captionText.text = myArticles[0];
                Debug.Log("added it to dropdowns!");
            }
            if (t.name == "heroArticles")
            {
                t.gameObject.SetActive(true);
                Dropdown heroArticlesMenu = t.gameObject.GetComponent<Dropdown>();
                heroArticlesMenu.ClearOptions();
                heroArticlesMenu.AddOptions(heroArticles);
            }
            if (t.name == "HeroArText")
            {
                t.gameObject.SetActive(true);
                Text heroArText = t.gameObject.GetComponent<Text>();
                heroArText.text = selectedPlayer.getHeroType() + " articles";
            }
            //if(t.name == "SendRequest")
            //{
            //    Button sendRequestButton = t.gameObject.GetComponent<Button>();
            //    sendRequestButton.interactable = true;
            //}
        }

    }

    //add to myArticles dropdown obj
    public void setGiveItem(int val)
    {
        //GameObject parentObj = GameObject.Find("SelectHero");
        giveItemIndex = val;


    }


    //add to heroArticles dropdown obj
    public void setReceiveItem(int val)
    {
        //GameObject parentObj = GameObject.Find("SelectHero");
        receiveItemIndex = val;

    }

    public void sendRequest()
    {
        Debug.Log("sending request!");

        players[0] = Game.myPlayer.getNetworkID();
        players[1] = selectedHero;


        switch (tradeTypeIndex)
        {
            case 0:
                tradeType[0] = "trade";
                tradeType[1] = myArticles[giveItemIndex];
                tradeType[2] = heroArticles[receiveItemIndex];

                break;
            case 1:
                tradeType[0] = "Gold";
                break;
            case 2:
                tradeType[0] = "Gemstones";
                break;
            default:
                tradeType[0] = "";
                break;
        }
        selectTradeType.gameObject.SetActive(false);
        selectHeroGive.gameObject.SetActive(false);
        selectHeroTrade.gameObject.SetActive(false);
        unavailable.gameObject.SetActive(false);
        Game.sendAction(new InitiateTrade(players, tradeType));

    }

    public void accept(bool accept)
    {

        GameController.instance.setTradeRequest(false);
        Game.sendAction(new RespondTrade(players, tradeType, accept));


        //GameController.instance.sendNotif()
    }

    public void setTradeType(string[] tradeType)
    {
        this.tradeType = tradeType;
    }

    public void setPlayers(string[] players)
    {
        this.players = players;
    }

    public void closeTradeMenu()
    {
        selectTradeType.gameObject.SetActive(false);
        selectHeroGive.gameObject.SetActive(false);
        selectHeroTrade.gameObject.SetActive(false);
        unavailable.gameObject.SetActive(false);
        eligibleGemstone.Clear();
        eligibleGold.Clear();
        eligibleTrade.Clear();

        tradeTypeIndex = -1;
    }
}