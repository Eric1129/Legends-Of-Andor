using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Merchant : MonoBehaviour
{
   //@TODO: replace Articles with an enum and all "cost" with 2
   private Dictionary<string,List<Article>> merchArticles = new Dictionary<string, List<Article>>();
    int merchGold = 0;

    public Merchant()
    {

        //set up merchItems
        //4 shield
        this.addArticles("Shield", 4);
        //3 bow
        this.addArticles("Bow", 3);
        //2 falcon
        this.addArticles("Falcon", 2);
        //5 wineskin
        this.addArticles("Wineksin", 5);
        //2 telescope
        this.addArticles("Telescope", 2);
        //5 witch's brew
        this.addArticles("WitchBrew", 5);
        //3 helm
        this.addArticles("Helm", 3);
        //strength
        this.addArticles("Strength", 1);

    }

    private void addArticles(string articleName, int quantity)
    {
        //5 wineskin
        List<Article> ars = new List<Article>();
        for (int i = 0; i < quantity; i++)
        {
            Article ar = new Article(articleName, 2);
            ars.Add(ar);

        }
        merchArticles.Add(articleName, ars);
    }

    public void loadAvailItems(Hero hero)
    {
        int heroGold = hero.getGold();
        foreach(List<Article> articles in merchArticles.Values){

            if (articles.Count == 0)
            {
                //item out of stock
                Button articleButton = GameObject.Find(articles[0].getArticleName()).GetComponent<Button>();
                articleButton.GetComponentInChildren<Text>().text = articles[0].getArticleName() + "SOLD OUT";
                //disable button corresponding to item
                articleButton.interactable = false;
            }
            else if(articles[0].getCost() > heroGold)
            {
                //hero can't afford item
                Button articleButton = GameObject.Find(articles[0].getArticleName()).GetComponent<Button>();

                //disable button corresponding to item
                articleButton.interactable = false;
            
            }else if (articles[0].getArticleName().Equals("WitchBrew"))
            {
                //TODO: check if the hero is on the same space as the witch
            }
            else
            {
                Button articleButton = GameObject.Find(articles[0].getArticleName()).GetComponent<Button>();

                articleButton.interactable = true;
            }
        }
    }

    public void buyItem(Hero hero, Article ar)
    {
        int cost = 2;
        int heroGold = hero.getGold();
        heroGold -= cost;
        merchGold += cost;
        hero.setGold(heroGold);
        if (!ar.getArticleName().Equals("Strengh"))
        {
            this.removeItem(ar);
            hero.addArticle(ar);
        }
        else
        {
            int heroStrength = hero.getStrength();
            heroStrength += cost;//DONT KONW IF THIS IS TRUE
            hero.setStrength(heroStrength);
        }
        
     }

    private void removeItem(Article ar)
    {
        foreach (List<Article> items in merchArticles.Values)
        {
            if (items[0].getArticleName().Equals(ar.getArticleName()))
            {
                items.Remove(ar);
            }
        }
    }

   
}   
