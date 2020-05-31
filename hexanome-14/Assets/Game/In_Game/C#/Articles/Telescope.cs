using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Telescope : Article
{
    private ArticleType article;
    private string description;

    public Telescope()
    {

        this.article = ArticleType.Telescope;
        this.description = "The telescope can only be used while " +
            "its owner is stopped on a space. It cannot be used while " +
            "just passing through. A hero may turn over and reveal all " +
            "tokens on spaces adjacent to the one he is on. This does not, " +
            "however, activate these tokens. This can also be done when it " +
            "isn’t the hero’s turn.";
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