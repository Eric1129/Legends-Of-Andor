using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Falcon : Article
{
	private ArticleType article;
	private string description;
    private bool usedToday;


    public Falcon()
	{

		article = ArticleType.Falcon;
		this.description = "Two heroes can exchange as many small articles (including a helm, witch’s brew, or poison), gold, or " +
			"gemstones at one time as they like even if they are not standing on the same space. One of them must be in possession of " +
			"the falcon. Large articles (shield or bow) cannot be traded in this way. The falcon can only be used once per day." +
			"Then the token is flipped onto its rear side, and at sunrise it is flipped back onto its front side. " +
			"The falcon can also be used in the mine in Legend 4.However, it cannot fly on or over spaces with rubble.The falcon " +
			"cannot be used during a battle.";
        usedToday = false;

    }

    public bool checkUsedToday()
    {
        return usedToday;
    }


    public void resetFalcon()
    {
        usedToday = false;
    }
    public void useArticle()
    {

        usedToday = true;
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