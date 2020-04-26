using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Shield: Article
{
    private ArticleType article;
    private string description;

    public Shield()
    {
        article = ArticleType.Shield;
        this.description = "Each side of the shield can be used once to help a " +
            "hero avoid losing willpower points after a battle round. Even in a " +
            "collective battle, a hero’s shield can only be used to prevent the loss " +
            "of his own willpower points, not the loss of points of any other hero taking " +
            "part in the battle. Alternatively, the shield can be used to fend off a negative" +
            " event card for the entire group of heroes. Any event card with a shield depicted " +
            "on it can be fended off in this way(including at sunrise).A shield can also be used " +
            "with a Legend 5 “Dragon Battle” card.It will only prevent the negative effect of " +
            "the card on the entire group of heroes, however. It will not prevent the card from " +
            "being returned to the bottom of the stack. \n\n After a shield is used for the " +
            "first time, turn it over so its opposite, damaged, side is up. After the " +
            "second use, it is returned to the equipment board.";
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
        //return ArticleType.Shield;
        return this.article;
    }

    public string getDescription()
    {
        return this.description;
    }
}
