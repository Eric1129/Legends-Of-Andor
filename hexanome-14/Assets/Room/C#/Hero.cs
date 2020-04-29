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

    //WILL CHANGE BACK TO 0, USING 10 FOR TESTING
    private int gold = 10;
    private int strength = 1;
    private int willpower = 7;

    private int hour = 0;
    //private List<string> articles = new List<string>();
    private int gemstones = 0;
    public Dictionary<string, List<Article>> heroArticles;
    public bool selectedWineskin;
    public int wineskinsides = 0;

    //for battle and article checks
    public bool inBattle = false;
    public bool usingWitchBrew = false;
    public bool usingHelm = false;
    public bool usingBow = false;
    public bool usingShield = false;
    public bool selectedArticle = false;
    public List<Interactable> playerInteractables;


    public Dictionary<int, int> numDice; //<wp, numberOfDice>

    public Hero()
    {
        
        pronouns = new string[3];
        heroArticles = new Dictionary<string, List<Article>>();
        numDice = new Dictionary<int, int>();
        playerInteractables = new List<Interactable>();
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
        this.strength = Mathf.Min(14, this.strength + strength);
    }
    public bool decreaseStrength(int strength)
    {
        this.strength = Mathf.Max(0, this.strength- strength);
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
        this.willpower = Mathf.Min(21, this.willpower+ willpower);
    }
    public void decreaseWillpower(int willpower)
    {
        this.willpower = Mathf.Max(0, this.willpower - willpower);
        
    }
    public int getHour()
    {
        return hour;
    }
    public void setHour(int hour)
    {
        Debug.Log("setting hour");
        this.hour = hour;
    }

    public string getHeroType()
    {
        return heroType;
    }
    public void setHeroType(string hero)
    {
        Debug.Log("SETTING HERO TYPE " + hero);
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

        if (hero.Equals("Female Wizard") || hero.Equals("Male Wizard") || hero.Equals("wizard"))
        {
            Debug.Log("wizard ");
            for (int i = 0; i<=20; i++)
            {
                if (numDice.ContainsKey(i))
                {
                    numDice[i] = 1;
                }
                else
                {
                    numDice.Add(i, 1);
                }
                
            }
        }

        if (hero.Equals("Female Warrior") || hero.Equals("Male Warrior") || hero.Equals("warrior"))
        {
            Debug.Log("WARRIOR ");
            for (int i = 0; i < 7; i++)
            {
                Debug.Log(i);
                if (numDice.ContainsKey(i))
                {
                    numDice[i] = 2;
                }
                else
                {
                    numDice.Add(i, 2);
                }
                
            }

            for (int i = 7; i < 14; i++)
            {
                Debug.Log(i);
                if (numDice.ContainsKey(i))
                {
                    numDice[i] = 3;
                }
                else
                {
                    numDice.Add(i, 3);
                }
            }

            for (int i = 14; i < 21; i++)
            {
                Debug.Log(i);
                if (numDice.ContainsKey(i))
                {
                    numDice[i] = 4;
                }
                else
                {
                    numDice.Add(i, 4);
                }
            }
        }

        if (hero.Equals("Female Dwarf") || hero.Equals("Male Dwarf")|| hero.Equals("dwarf"))
        {
            Debug.Log("DWARF ");
            for (int i = 0; i < 7; i++)
            {
                if (numDice.ContainsKey(i))
                {
                    numDice[i] = 1;
                }
                else
                {
                    numDice.Add(i, 1);
                }
            }

            for (int i = 7; i < 14; i++)
            {
                if (numDice.ContainsKey(i))
                {
                    numDice[i] = 2;
                }
                else
                {
                    numDice.Add(i, 2);
                }
            }

            for (int i = 14; i < 21; i++)
            {
                if (numDice.ContainsKey(i))
                {
                    numDice[i] = 3;
                }
                else
                {
                    numDice.Add(i, 3);
                }
            }
        }

        if (hero.Equals("Female Archer") || hero.Equals("Male Archer") || hero.Equals("archer"))
        {
            Debug.Log("ARCHER ");
            for (int i = 0; i < 7; i++)
            {
                if (numDice.ContainsKey(i))
                {
                    numDice[i] = 3;
                }
                else
                {
                    numDice.Add(i, 3);
                }
            }

            for (int i = 7; i < 14; i++)
            {
                if (numDice.ContainsKey(i))
                {
                    numDice[i] = 4;
                }
                else
                {
                    numDice.Add(i, 4);
                }
            }

            for (int i = 14; i < 21; i++)
            {
                if (numDice.ContainsKey(i))
                {
                    numDice[i] = 5;
                }
                else
                {
                    numDice.Add(i, 5);
                }
            }
        }

    }

    public int getNumDice()
    {
        Debug.Log("wp" + this.willpower);
        return numDice[this.willpower];
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

    public List<int> rollDice()
    {
        System.Random random = new System.Random();
        List<int> diceRolls = new List<int>();

        if (!(heroType.Equals("Male Archer") || heroType.Equals("Female Archer")))
        {
            Debug.Log("num dice" + getNumDice());
            for (int i = 0; i < getNumDice(); i++)
            {
                diceRolls.Add(random.Next(1, 7));
            }
        }
        else
        {
            //Archer only rolls one at a time
            diceRolls.Add(random.Next(1, 7));
        }

        

        return diceRolls;
    }

    

}
