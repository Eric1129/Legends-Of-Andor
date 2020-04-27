using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;

[System.Serializable]
[JsonObject(MemberSerialization.Fields)]
public class Hero // : MonoBehaviour, Movable, Fightable
{
    // private GameObject sphere;
    // private SpriteRenderer spriteRenderer;
    private string
        heroType = "";
    private string myTag;
    private string[] pronouns;

    // this is the tag of the sphere gameObject which we show
    // as a hero's position! (currently the gray squished sphere)
    private string sphereTag;

    //private MoveStrategy moveStrat;
    //private DiceRollStrategy diceRollStrat;

    // don't care which farmers, just that this number exists on our space.
    private int numFarmers;
    private bool hasThorald;
    private int heroRank;


    private int gold = 10;
    private int strength = 0;
    private int willpower = 0;

    private int hour = 0;
    //private List<string> articles = new List<string>();
    private int gemstones = 0;
    public Dictionary<string, List<Article>> heroArticles;
    public bool selectedWineskin;
    public int wineskinsides = 0;

    public Dictionary<int, int> numDice; //<wp, numberOfDice>

    public Hero()
    {
        
        pronouns = new string[3];
        heroArticles = new Dictionary<string, List<Article>>();
        numDice = new Dictionary<int, int>();
    }

    public string allArticlesAsString()
    {
        string articles = "";
        foreach (string key in this.heroArticles.Keys)
        {
            int quantity = heroArticles[key].Count;
            articles += quantity + " x " + key +  " \n";
        }

        return articles;
        //List<string> articles = new List<string>();
        //foreach(string key in this.heroArticles.Keys)
        //{
        //    articles.Add(key);
        //}
        //return articles;
    }

    public List<string> allArticlesAsStringList()
    {
        List<string> articles = new List<string>();
        foreach (string key in this.heroArticles.Keys)
        {
            articles.Add(key);
        }
        return articles;
    }

    public bool hasArticle(string key)
    {
        List<Article> articles;
        if (heroArticles.TryGetValue(key, out articles))
        {
            return heroArticles[key].Count > 0;

        }
        else
        {
            return false;
        }
    }



    public void addArticle(Article article)
    {
        if (heroArticles.ContainsKey(article.ToString()))
        {
            heroArticles[article.ToString()].Add(article);
        }
        else
        {
            List<Article> articles = new List<Article>();
            articles.Add(article);
            heroArticles.Add(article.ToString(), articles);
        }

    }
    public Dictionary<string,List<Article>>  getAllArticles()
    {
        return heroArticles;
    }

    public int getGold()
    {
        return gold;
    }
    public void setGold(int gold)
    {
        this.gold = gold;
    }
    public void increaseGold(int gold)
    {
        this.gold += gold;
    }
    public void decreaseGold(int gold)
    {
        this.gold -= gold;
    }
    public int getStrength()
    {
        return strength;
    }
    public void setStrength(int strength)
    {
        this.strength = strength;
    }
    public void increaseStrength(int strength)
    {
        this.strength += strength;
    }
    public bool decreaseStrength(int strength)
    {
        this.strength -= strength;
        return true;
    }
    public int getWillpower()
    {
        return willpower;
    }
    public void setWillpower(int willpower)
    {
        this.willpower = willpower;
    }
    public void increaseWillpower(int willpower)
    {
        this.willpower += willpower;
    }
    public void decreaseWillpower(int willpower)
    {
        this.willpower -= willpower;
    }
    public int getHour()
    {
        return hour;
    }
    public void setHour(int hour)
    {
        this.hour = hour;
    }

    public string getHeroType()
    {
        return heroType;
    }
    public void setHeroType(string hero)
    {
        this.heroType = hero;

        if (hero.StartsWith("Male"))
        {
            pronouns[0] = "he";
            pronouns[1] = "him";
            pronouns[2] = "his";
        }

        if (hero.StartsWith("Female"))
        {
            pronouns[0] = "she";
            pronouns[1] = "hers";
            pronouns[2] = "her";
        }

        if (hero.Contains("Mage"))
        {
            for(int i = 0; i<=20; i++)
            {
                numDice.Add(i, 1);
            }
        }

        if (hero.Contains("Warrior"))
        {
            for (int i = 0; i < 7; i++)
            {
                numDice.Add(i, 2);
            }

            for (int i = 7; i < 14; i++)
            {
                numDice.Add(i, 3);
            }

            for (int i = 14; i < 21; i++)
            {
                numDice.Add(i, 4);
            }
        }

        if (hero.Contains("Dwarf"))
        {
            for (int i = 0; i < 7; i++)
            {
                numDice.Add(i, 1);
            }

            for (int i = 7; i < 14; i++)
            {
                numDice.Add(i, 2);
            }

            for (int i = 14; i < 21; i++)
            {
                numDice.Add(i, 3);
            }
        }

        if (hero.Contains("Archer"))
        {
            for (int i = 0; i < 7; i++)
            {
                numDice.Add(i, 3);
            }

            for (int i = 7; i < 14; i++)
            {
                numDice.Add(i, 4);
            }

            for (int i = 14; i < 21; i++)
            {
                numDice.Add(i, 5);
            }
        }

    }

    public int getHeroRank()
    {
        return heroRank;
    }
    public void setHeroRank(int rank)
    {
        this.heroRank = rank;

    }

    public string[] getPronouns()
    {
        return this.pronouns;
    }

    public int getGemstone()
    {
        return this.gemstones;
    }


    public Article removeArticle(string articleName)
    {
        int numArticles = heroArticles[articleName].Count;
        Article removedArticle = heroArticles[articleName][numArticles - 1];
        if (numArticles > 1)
        {
            heroArticles[articleName].Remove(heroArticles[articleName][numArticles - 1]);
        }
        else
        {
            heroArticles.Remove(articleName);
        }


        return removedArticle;
    }


    public Article removeArticle2(string articleName, Article article)
    {

        heroArticles[articleName].Remove(article);
       
        return article;
       
    }


    //public string allArticles()
    //{
    //    string s_articles = "";
    //    foreach(string ar in this.articles)
    //    {
    //        s_articles += (ar + " ");
    //    }
    //    return s_articles;
    //}

    public void incGold(int amount)
    {
        this.gold += amount;
    }

    public void decGold(int amount)
    {
        this.gold -= amount;
    }

    public void updateStrength(int numPoints)
    {
        this.strength += numPoints;
    }

    

}
