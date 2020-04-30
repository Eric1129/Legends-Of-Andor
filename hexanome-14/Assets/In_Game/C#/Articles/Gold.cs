using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gold : PickDrop, Article
{
	private ArticleType article;
	private string description;
    private int numUsed;
    // 1, 2, or 5
    private int dollars;

    public Gold(Node startingPos, GameObject instance, int numDollars) : base(startingPos, instance, false, "Gold")
    {
        startingPos.addInteractable(this);
        setDefaults(numDollars);
    }

    public Gold(Node startingPos, int numDollars) : base(startingPos, null, false, "Gold")
    {
        startingPos.addInteractable(this);
        setDefaults(numDollars);
    }

    private Gold() : base(null, null, false, "NULL") { }

    private void setDefaults(int numDollars)
    {
        dollars = numDollars;
        numUsed = 0;
        article = ArticleType.Gold;
        description = "This gold token is worth $" + dollars + ". It can be used to purchase articles from stores such as the merchant or the witch.";
    }

    public void setGold(int gold)
    {
        dollars = gold;
    }

    public int getGold()
    {
        return dollars;
    }


    // this should just pick up the runestone I think.
    public void useArticle()
    {
        Debug.Log("Tried to use the gold article, does nothing rn.");
        // maybe: this.dollars = 0;
    }

    public ArticleType getArticle()
    {
        return ArticleType.Gold;
    }

    public string articleToString()
    {
        return article.ToString();
    }


    public string getDescription()
    {
        return description;
    }
}
