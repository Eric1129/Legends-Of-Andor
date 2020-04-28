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
    string selectedHero;
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
    public int TotalGoldAdmin;
    public int TotalWineAdmin;
    public Transform DistributeArticlesBoard;
    public Transform WaitingScreenPopup;
    //increment this number 
    int PlayerNumber;
    public List<Hero> heroes = new List<Hero>(4);
   
   

    public void decideWhichScreenToDisplay() { 

        client = Game.myPlayer.getNetworkID();


        if (client.Equals(PhotonNetwork.IsMasterClient))
        {
            //set popup with decision display to active 
            DistributeArticlesBoard.gameObject.SetActive(true);

            int i = 1;
            //loop through each player in game and display their hero name on the screen board
            foreach (Andor.Player p in Game.gameState.getPlayers())
            {

                if (p.getNetworkID().Equals(client))
                {
                    displayPlayerName(p, 4);
                }
                else
                {
                    displayPlayerName(p, i);
                    i++;
                }
            }
        }else{
            //set popup with wait display to active
            WaitingScreenPopup.gameObject.SetActive(true);
        }
    }

    public void displayPlayerName(Andor.Player p, int i)
    {
        Transform[] trs = DistributeArticlesBoard.GetComponentsInChildren<Transform>(true);
        foreach(Transform t in trs)
        {
            if(t.name == "Player" + i + "Name")
            {
                t.GetComponent<Text>().text = p.getHeroType();//displays the text
                heroes[i-1] = p.getHero(); //stores the hero at index i-1 (0->3)
            }
        }
    }


    public void incrGold(int buttonID)
    {
        Transform[] trs = DistributeArticlesBoard.GetComponentsInChildren<Transform>(true);
        foreach (Transform t in trs)
        {
            if (t.name == "p1valuegold" + buttonID)
            {

                if (goldToDistribute != 0) { 

                goldToDistribute--;
                TotalGoldP1++;
                //CHANGE THIS FOR ALL 
                t.GetComponent<Text>().text = TotalGoldP1.ToString();
            }
            }
            else if (t.name == "p2valuegold" + buttonID)
            {
                if (goldToDistribute != 0)
                {
                    goldToDistribute--;
                    TotalGoldP2++;
                    t.GetComponent<Text>().text = TotalGoldP2.ToString();
                }
            }
            else if (t.name == "p3valuegold" + buttonID)
            {
                if (goldToDistribute != 0)
                {

                    goldToDistribute--;
                    TotalGoldP3++;
                    t.GetComponent<Text>().text = TotalGoldP3.ToString();
                }

            }
            else if (t.name == "adminvaluegold" + buttonID)
            {
                if (goldToDistribute != 0)
                {

                    goldToDistribute--;
                    TotalGoldAdmin++;
                    t.GetComponent<Text>().text = TotalGoldAdmin.ToString();
                }
            }

        }
    }


    public void decrGold(int buttonID)
    {
        Transform[] trs = DistributeArticlesBoard.GetComponentsInChildren<Transform>(true);
        foreach (Transform t in trs)
        {
            if (t.name == "p1valuegold" + buttonID)
            {
                if (TotalGoldP1 != 0)
                {

                    goldToDistribute++;
                    TotalGoldP1--;
                    t.GetComponent<Text>().text = TotalGoldP1.ToString();
                    //increment gold
                    //t.game
                }
            }
            else if (t.name == "p2valuegold" + buttonID)
            {
                if (TotalGoldP2 != 0)
                {
                    goldToDistribute++;
                    TotalGoldP2--;
                    t.GetComponent<Text>().text = TotalGoldP2.ToString();
                }
            }
            else if (t.name == "p3valuegold" + buttonID)
            {
                if (TotalGoldP3 != 0)
                {

                    goldToDistribute++;
                    TotalGoldP3--;
                    t.GetComponent<Text>().text = TotalGoldP3.ToString();
                }

            }
            else if (t.name == "adminvaluegold" + buttonID)
            {
                if (TotalGoldAdmin != 0)
                {

                    goldToDistribute++;
                    TotalGoldAdmin--;
                    t.GetComponent<Text>().text = TotalGoldAdmin.ToString();
                }
            }

        }
    }

    public void incrWine(int buttonID)
    {
        Transform[] trs = DistributeArticlesBoard.GetComponentsInChildren<Transform>(true);
        foreach (Transform t in trs)
        {
            if (t.name == "p1valuewine" + buttonID)
            {
                if (wineToDistribute != 0)
                {
                    wineToDistribute--;
                    TotalWineP1++;
                    t.GetComponent<Text>().text = TotalWineP1.ToString();
                    //increment gold
                    //t.game
                }
            }
            else if (t.name == "p2valuewine" + buttonID)
            {
                if (wineToDistribute != 0)
                {
                    wineToDistribute--;
                    TotalWineP2++;
                    t.GetComponent<Text>().text = TotalWineP2.ToString();
                }
            }
            else if (t.name == "p3valuewine" + buttonID)
            {
                if (wineToDistribute != 0)
                {
                    wineToDistribute--;
                    TotalWineP3++;
                    t.GetComponent<Text>().text = TotalWineP3.ToString();
                }

            }
            else if (t.name == "adminvaluewine" + buttonID)
            {
                if (wineToDistribute != 0)
                {
                    wineToDistribute--;
                    TotalWineAdmin++;
                    t.GetComponent<Text>().text = TotalWineAdmin.ToString();
                }
            }

        }
    }

    public void decrWine(int buttonID)
    {
        Transform[] trs = DistributeArticlesBoard.GetComponentsInChildren<Transform>(true);
        foreach (Transform t in trs)
        {
            if (t.name == "p1valuewine" + buttonID)
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
            else if (t.name == "p2valuewine" + buttonID)
            {
                if (TotalWineP2 != 0)
                {
                    wineToDistribute++;
                    TotalWineP2--;
                    t.GetComponent<Text>().text = TotalWineP2.ToString();
                }
            }
            else if (t.name == "p3valuewine" + buttonID)
            {
                if (TotalWineP3 != 0)
                {

                    wineToDistribute++;
                    TotalWineP3--;
                    t.GetComponent<Text>().text = TotalWineP3.ToString();
                }

            }
            else if (t.name == "adminvaluegold" + buttonID)
            {
                if (TotalWineAdmin != 0)
                {

                    wineToDistribute++;
                    TotalWineAdmin--;
                    t.GetComponent<Text>().text = TotalWineAdmin.ToString();
                }
            }

        }
    }


    public void doneButton()
    {
        if (goldToDistribute == 0 && wineToDistribute == 0)
        {
            //ALLOW EXIT OUT OF POP-UP AND UPDATE HERO ATTRIBUTES 
            DistributeArticlesBoard.gameObject.SetActive(false);
            WaitingScreenPopup.gameObject.SetActive(false);


            for(int i = 0; i < heroes.Count; i++)
            {
                if (i == 0)
                {
                    //player 1
                    
                    heroes[i].setWillpower(2); 
                    //every player starts off w 2 willpower points

                    for(int j = 0; j < TotalWineP1; j++)
                    {
                        Article wine = new Wineskin();
                        heroes[i].addArticle(wine); 
                    }
                }else if (i == 1)
                {
                    
                    heroes[i].setGold(TotalGoldP2);
                    //every player starts off w 2 willpower points

                    for (int j = 0; j < TotalWineP2; j++)
                    {
                        Article wine = new Wineskin();
                        heroes[i].addArticle(wine);
                    }





                }
                else if (i == 2)
                {
                    
                    heroes[i].setGold(TotalGoldP3);
                    //every player starts off w 2 willpower points

                    for (int j = 0; j < TotalWineP3; j++)
                    {
                        Article wine = new Wineskin();
                        heroes[i].addArticle(wine);
                    }


                }
                else if (i == 3)
                {
                    
                    heroes[i].setGold(TotalGoldAdmin);
                    //every player starts off w 2 willpower points

                    for (int j = 0; j < TotalWineAdmin; j++)
                    {
                        Article wine = new Wineskin();
                        heroes[i].addArticle(wine);
                    }

                }
            }
          
        }else{

            //Display message that not all WINESKINS/GOLD were distributed ????

            DistributeArticlesBoard.gameObject.SetActive(true);
            WaitingScreenPopup.gameObject.SetActive(true);

        }


    }

}
