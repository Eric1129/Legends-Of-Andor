using System.Collections.Generic;
using UnityEngine;
public class Helm : Article
{
    private ArticleType article;
    private string description;

    public Helm()
    {

        this.article = ArticleType.Helm;
        this.description = "A hero can use a helm to let him total up all " +
            "identical dice valuesin a battle. For an archer or a hero using a " +
            "bow, the helm offers no advantage because only the final rolled die " +
            "counts.The helm also offers no advantage to the wizard, who just rolls " +
            "one die.But these heroes can still purchase and carry a helm and later " +
            "give it to a warrior or dwarf, for whom a helm is very valuable. " +
            "A helm cannot be combined with the witch’s brew.";
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