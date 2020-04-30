using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class DistributeArticlesScreen : MonoBehaviour
{

    //if PhotonNetwork.IsMasterClient then 

    int goldToDistribute = 5;
    int wineToDistribute = 2;
    public string[] players;
    private static List<string> myArticles;
    //the myArticlesList of each player will be changing (Gold/Wineskin)
    private string client;
    public int TotalGoldP1;
    public int TotalWineP1;
    public int TotalGoldP2;
    public int TotalWineP2;
    public int TotalGoldP3;
    public int TotalWineP3;
    public int TotalGoldP4;
    public int TotalWineP4;
    public Transform DistributeArticlesBoard;
    public Transform WaitingScreenPopup;


    public Transform player1Container;
    public Transform player2Container;
    public Transform player3Container;
    public Transform player4Container;
    public List<Transform> playerContainers;
    //increment this number 
    //public List<Hero> heroes = new List<Hero>(4);
    public List<Andor.Player> playersList;



    public void startDist()
    {

        Debug.Log("reached"); 

        playersList = Game.gameState.getPlayers();
        playerContainers = new List<Transform>();
        playerContainers.Add(player1Container);
        playerContainers.Add(player2Container);
        playerContainers.Add(player3Container);
        playerContainers.Add(player4Container);

        for (int i = 0; i < 4 - playersList.Count; i++)
        {
            playerContainers[(4 - i) - 1].gameObject.SetActive(false);
            playerContainers.RemoveAt((4 - i) - 1);
        }

        for (int i = 0; i < playersList.Count; i++)
        {
            playerContainers[i].GetChild(6).GetComponent<Text>().text = playersList[i].getHeroType();
        }


    }

    //===================AILISH REFACTOR===========================
    Dictionary<string, int> playerGold = new Dictionary<string, int>();
    Dictionary<string, int> playerWineskin = new Dictionary<string, int>();

    public void incrementGold(int buttonID)
    {
        if(goldToDistribute > 0)
        {
            Transform[] trs = DistributeArticlesBoard.GetComponentsInChildren<Transform>();
            foreach (Transform t in trs)
            {
                if (t.name == "Player" + buttonID)
                {
                    string playerID = playersList[buttonID - 1].getNetworkID();
                    Transform[] attrs = t.GetComponentsInChildren<Transform>();
                    foreach (Transform attr in attrs)
                    {
                        if (attr.name == "goldValue")
                        {
                            if (playerGold.ContainsKey(playerID))
                            {
                                playerGold[playerID] = playerGold[playerID] + 1;
                                
                            }
                            else
                            {
                                playerGold.Add(playerID, 1);
                            }

                            attr.GetComponent<Text>().text = playerGold[playerID].ToString();

                        }
                    }
                }
            }
            goldToDistribute--;
        }
        
    }

    public void incrementWineskin(int buttonID)
    {
        if (wineToDistribute > 0)
        {
            Transform[] trs = DistributeArticlesBoard.GetComponentsInChildren<Transform>();
            foreach (Transform t in trs)
            {
                if (t.name == "Player" + buttonID)
                {
                    string playerID = playersList[buttonID - 1].getNetworkID();
                    Transform[] attrs = t.GetComponentsInChildren<Transform>();
                    foreach (Transform attr in attrs)
                    {
                        if (attr.name == "wineValue")
                        {
                            if (playerWineskin.ContainsKey(playerID))
                            {
                                playerWineskin[playerID] = playerWineskin[playerID] + 1;

                            }
                            else
                            {
                                playerWineskin.Add(playerID, 1);
                            }

                            attr.GetComponent<Text>().text = playerWineskin[playerID].ToString();

                        }
                    }
                }
            }
            wineToDistribute--;
        }

    }


    public void decrementGold(int buttonID)
    {
        string playerID = playersList[buttonID - 1].getNetworkID();
        if (playerGold.ContainsKey(playerID))
        {
            if(playerGold[playerID] > 0)
            {
                Transform[] trs = DistributeArticlesBoard.GetComponentsInChildren<Transform>();
                foreach (Transform t in trs)
                {
                    if (t.name == "Player" + buttonID)
                    {
                        Transform[] attrs = t.GetComponentsInChildren<Transform>();
                        foreach (Transform attr in attrs)
                        {
                            if (attr.name == "goldValue")
                            {
                                
                                playerGold[playerID] = playerGold[playerID] - 1;

                                

                                attr.GetComponent<Text>().text = playerGold[playerID].ToString();

                            }
                        }
                    }
                }
                goldToDistribute++;

            }
        }
    }

    public void decrementWineskin(int buttonID)
    {
        string playerID = playersList[buttonID - 1].getNetworkID();
        if (playerWineskin.ContainsKey(playerID))
        {
            if (playerWineskin[playerID] > 0)
            {
                Transform[] trs = DistributeArticlesBoard.GetComponentsInChildren<Transform>();
                foreach (Transform t in trs)
                {
                    if (t.name == "Player" + buttonID)
                    {
                        Transform[] attrs = t.GetComponentsInChildren<Transform>();
                        foreach (Transform attr in attrs)
                        {
                            if (attr.name == "winValue")
                            {

                                playerWineskin[playerID] = playerWineskin[playerID] - 1;



                                attr.GetComponent<Text>().text = playerWineskin[playerID].ToString();

                            }
                        }
                    }
                }
                wineToDistribute++;

            }
        }
    }

    public void doneClick()
    {
        string[] players = new string[playersList.Count];
        for(int i=0; i< playersList.Count; i++)
        {
            players[i] = playersList[i].getNetworkID();
        }
        Game.sendAction(new DistributeBoard(players, playerGold, playerWineskin));
    }

    public void closeScreens()
    {
        DistributeArticlesBoard.gameObject.SetActive(false);
        WaitingScreenPopup.gameObject.SetActive(false);
    }

    public void decideWhichScreenToDisplay(bool host)
    {

        if (host)
        {

            DistributeArticlesBoard.gameObject.SetActive(true);
            WaitingScreenPopup.gameObject.SetActive(false); 
        }
        else
        {
            //set popup with wait display to active
            Debug.Log("Waiting Screen Case Reached");
            WaitingScreenPopup.gameObject.SetActive(true);
            DistributeArticlesBoard.gameObject.SetActive(false);
        }
    }


    public void incrGold(Transform buttonID)
    {
        for (int i = 0; i < buttonID.parent.childCount; i++)
        {
            //The text component also needs to be updated somewhere...? 

            if (buttonID.parent.GetChild(i).name.Equals("goldValue"))
            {

                for (int j = 1; j < 5; j++)
                {
                    if (buttonID.parent.name.Equals("Player" + j) && goldToDistribute !=0)
                    {
                        goldToDistribute--;
                        if (j == 1)
                        {
                            TotalGoldP1++;

                            buttonID.parent.Find("goldValue").gameObject.GetComponent<Text>().text = TotalGoldP1.ToString();
                        
                    }
                        else if (j == 2)
                        {
                            TotalGoldP2++;
                            buttonID.parent.Find("goldValue").gameObject.GetComponent<Text>().text = TotalGoldP2.ToString();
                        }
                        else if (j == 3)
                        {
                            TotalGoldP3++;
                            buttonID.parent.Find("goldValue").gameObject.GetComponent<Text>().text = TotalGoldP3.ToString();
                        }
                        else if (j == 4)
                        {
                            TotalGoldP4++;
                            buttonID.parent.Find("goldValue").gameObject.GetComponent<Text>().text = TotalGoldP4.ToString();
                        }
                    }
                }
            }
        }
    }

    public void decrGold(Transform buttonID)
    {
        for (int i = 0; i < buttonID.parent.childCount; i++)
        {
            //The text component also needs to be updated somewhere...? 

            if (buttonID.parent.GetChild(i).name.Equals("goldValue"))
            {

                for (int j = 1; j < 5; j++)
                {
                    if (buttonID.parent.name.Equals("Player" + j))
                    {
                        goldToDistribute++;
                        if (j == 1)
                        {
                            TotalGoldP1--;
                            buttonID.parent.Find("goldValue").gameObject.GetComponent<Text>().text = TotalGoldP1.ToString();
                        }
                        else if (j == 2)
                        {
                            TotalGoldP2--;
                            buttonID.parent.Find("goldValue").gameObject.GetComponent<Text>().text = TotalGoldP2.ToString();
                        }
                        else if (j == 3)
                        {
                            TotalGoldP3--;
                            buttonID.parent.Find("goldValue").gameObject.GetComponent<Text>().text = TotalGoldP3.ToString();
                        }
                        else if (j == 4)
                        {
                            TotalGoldP4--;
                            buttonID.parent.Find("goldValue").gameObject.GetComponent<Text>().text = TotalGoldP4.ToString();
                        }
                    }
                }
            }
        }
    }

    public void incrWine(Transform buttonID)
    {
        for (int i = 0; i < buttonID.parent.childCount; i++)
        {
            //The text component also needs to be updated somewhere...? 

            if (buttonID.parent.GetChild(i).name.Equals("wineValue"))
            {

                for (int j = 1; j < 5; j++)
                {
                    if (buttonID.parent.name.Equals("Player" + j) && wineToDistribute != 0 )
                    {
                        wineToDistribute--;
                        if (j == 1)
                        {
                            TotalWineP1++;
                            buttonID.parent.Find("wineValue").gameObject.GetComponent<Text>().text = TotalWineP1.ToString();
                        }
                        else if (j == 2)
                        {
                            TotalWineP2++;
                            buttonID.parent.Find("wineValue").gameObject.GetComponent<Text>().text = TotalWineP2.ToString();
                        }
                        else if (j == 3)
                        {
                            TotalWineP3++;
                            buttonID.parent.Find("wineValue").gameObject.GetComponent<Text>().text = TotalWineP3.ToString();
                        }
                        else if (j == 4)
                        {
                            TotalWineP4++;
                            buttonID.parent.Find("wineValue").gameObject.GetComponent<Text>().text = TotalWineP4.ToString();
                        }
                    }
                }
            }
        }
    }

    public void decrWine(Transform buttonID)
    {
        for (int i = 0; i < buttonID.parent.childCount; i++)
        {
            //The text component also needs to be updated somewhere...? 

            if (buttonID.parent.GetChild(i).name.Equals("wineValue"))
            {

                for (int j = 1; j < 5; j++)
                {
                    if (buttonID.parent.name.Equals("Player" + j))
                    {
                        wineToDistribute++;
                        if (j == 1)
                        {
                            TotalWineP1--;
                            buttonID.parent.Find("wineValue").gameObject.GetComponent<Text>().text = TotalWineP1.ToString();
                        }
                        else if (j == 2)
                        {
                            TotalWineP2--;
                            buttonID.parent.Find("wineValue").gameObject.GetComponent<Text>().text = TotalWineP2.ToString();
                        }
                        else if (j == 3)
                        {
                            TotalWineP3--;
                            buttonID.parent.Find("wineValue").gameObject.GetComponent<Text>().text = TotalWineP3.ToString();
                        }
                        else if (j == 4)
                        {
                            TotalWineP4--;
                            buttonID.parent.Find("wineValue").gameObject.GetComponent<Text>().text = TotalWineP4.ToString();
                        }
                    }
                }
            }
        }
    }


    /*

        public void decrWine(Transform buttonID)
        {
            GameObject p = GameObject.Find("scrollImage");
            Transform[] trs = p.GetComponentsInChildren<Transform>(true);
            //Transform[] trs = DistributeArticlesBoard.GetComponentsInChildren<Transform>(true);
            foreach (Transform t in trs)
            {
                if (t.name == "p" + buttonID + "valuewine")
                {
                    if (TotalWineP1 != 0)
                    {
                        wineToDistribute++;
                        TotalWineP1--;
                        t.GetComponent<Text>().text = TotalWineP1.ToString();
                        //increment gold
                        //t.game
                    }
                }
                else if (t.name == "p" + buttonID + "valuewine")
                {
                    if (TotalWineP2 != 0)
                    {
                        wineToDistribute++;
                        TotalWineP2--;
                        t.GetComponent<Text>().text = TotalWineP2.ToString();
                    }
                }
                else if (t.name == "p" + buttonID + "valuewine")
                {
                    if (TotalWineP3 != 0)
                    {

                        wineToDistribute++;
                        TotalWineP3--;
                        t.GetComponent<Text>().text = TotalWineP3.ToString();
                    }

                }
                else if (t.name == "adminvaluewine" + buttonID)
                {
                    if (TotalWineP4 != 0)
                    {

                        wineToDistribute++;
                        TotalWineP4--;
                        t.GetComponent<Text>().text = TotalWineP4.ToString();
                    }
                }

            }
        }
        */



    public void doneButton()
    {


        if (goldToDistribute == 0 && wineToDistribute == 0)
        {
            //ALLOW EXIT OUT OF POP-UP AND UPDATE HERO ATTRIBUTES 
            DistributeArticlesBoard.gameObject.SetActive(false);
            WaitingScreenPopup.gameObject.SetActive(false);


            for(int i = 0; i < playersList.Count; i++)
            {

                if (i == 0)
                {
                    //P1
                    playersList[i].getHero().setGold(TotalGoldP1);

                    for(int j = 0; j < TotalWineP1; j++)
                    {
                        Article wine = new Wineskin();
                        playersList[i].getHero().addArticle(wine); 

                    }



                }else if (i == 1)
                {

                    playersList[i].getHero().setGold(TotalGoldP2);
                    for (int j = 0; j < TotalWineP2; j++)
                    {
                        Article wine = new Wineskin();
                        playersList[i].getHero().addArticle(wine);

                    }

                }
                else if (i == 2)
                {

                    playersList[i].getHero().setGold(TotalGoldP3);
                    for (int j = 0; j < TotalWineP3; j++)
                    {
                        Article wine = new Wineskin();
                        playersList[i].getHero().addArticle(wine);

                    }

                }
                else if (i == 3)
                {
                    playersList[i].getHero().setGold(TotalGoldP4);
                    for (int j = 0; j < TotalWineP4; j++)
                    {
                        Article wine = new Wineskin();
                        playersList[i].getHero().addArticle(wine);

                    }

                }


            }

            GameController.instance.updateHeroStats();
        }

    }


    
}



           


