using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Bow : Article
{
    private ArticleType article;
    private string description;

    public Bow()
    {

        this.article = ArticleType.Bow;
        this.description = "A hero with a bow may attack a creature in an adjacent space. " +
            "He rolls each of his dice in turn and decides when to stop rolling. " +
            "Only the final roll counts. When a hero carrying a bow (not the archer) " +
            "is on the same space as a creature, he does not use the bow against the " +
            "creature, and rolls all of his dice at once.";
    }

    public void useArticle()
    {

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