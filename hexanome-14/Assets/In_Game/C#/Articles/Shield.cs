using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Shield: Article
{
    ArticleType article;
  
    public Shield()
    {
        article = ArticleType.Shield;
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
        return ArticleType.Shield;
    }

}
