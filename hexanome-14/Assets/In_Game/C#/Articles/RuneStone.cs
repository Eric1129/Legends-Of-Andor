using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RuneStone : PickDrop, Article
{
	private ArticleType article;
	private string description;
    private int numUsed;
    private string color;

    public RuneStone(Node startingPos, GameObject instance, string col) : base(startingPos, instance, false, "RuneStone")
    {
        startingPos.addInteractable(this);
        setDefaults(col);
    }

    private RuneStone() : base(null, null, false, "NULL") { }

    private void setDefaults(string col)
    {
        color = col;
        numUsed = 0;
        article = ArticleType.RuneStone;
        description = "If a hero has 3 different colored rune stones on their hero board, he gets a black die. This has a higher value than the hero dice. As long as he has the rune stones on their board, he may use this black die instead of one of their own The wizard can also apply their special ability to the black die. Anyone who has 3 rune stones on their can't have any other articles in their small storage spaces. Otherwise, he will have to give away or discard rune stones and put the black die back.";
    }

    // this should just pick up the runestone I think.
    public void useArticle()
    {
        Debug.Log("Tried to use the runestone article, does nothing rn.");
    }

    public ArticleType getArticle()
    {
        return ArticleType.RuneStone;
    }

    public string articleToString()
    {
        return article.ToString();
    }

    public string getDescription()
    {
        return description;
    }

    public override void pickedUpSpecial()
    {
        
    }

    public override void DroppedSpecial()
    {
        
    }
}
