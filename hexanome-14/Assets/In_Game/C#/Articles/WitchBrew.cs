using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class WitchBrew : Article
{
	private ArticleType article;
	private string description;

	public WitchBrew()
	{

		this.article = ArticleType.WitchBrew;
		this.description = "Each side of the witch’s brew " +
			"token can be used to double a hero’s dice value " +
			"during a battle. Only one half may be used per battle " +
			"round, however. A hero is also not permitted to use " +
			"several brew tokens at one time. The hero must decide " +
			"immediately after his roll whether he wants to use the " +
			"witch’s brew. He can only use the brew for his own roll, " +
			"not for that of another hero. The witch’s brew cannot be " +
			"used in combination with a helm. So if the hero has both the " +
			"brew and the helm at his disposal, he must pick one or the other.";
	}

	public void useArticle()
	{

	}

	public string articleToString()
	{
		return "Witch's Brew";
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