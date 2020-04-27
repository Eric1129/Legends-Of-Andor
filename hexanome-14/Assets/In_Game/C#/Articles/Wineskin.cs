using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Wineskin : Article
{
    ArticleType article;
    private string description;
    public int numUsed;

    public Wineskin()
    {

        this.article = ArticleType.Wineskin;
        this.description = "When a hero opts for the “move” action, " +
            "he can use each side of the wineskin to advance 1 space " +
            "without having to move his time marker on the time track. " +
            "After the initial use of the wineskin, flip the token over to " +
            "its half-full side. After the second use, place the token back on" +
            " the equipment board. The hero can also use both sides of the wineskin" +
            " in one turn or use several wineskins at once.";
        this.numUsed = 0;
    }

    public void useArticle()
    {
        this.numUsed += 1;
    
    }

    public void reset()
    {
        this.numUsed = 0;
    }


    public int getNumUsed()
    {
        return this.numUsed;
    }
    public string articleToString()
    {
        return this.article.ToString();
    }

    public ArticleType getArticle()
    {
        return this.article;
    }

    public string getDescription()
    {
        return this.description;
    }
}