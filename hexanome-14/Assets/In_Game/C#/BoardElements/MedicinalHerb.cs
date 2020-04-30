using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MedicinalHerb : PickDrop, Article
{

	public bool used;
	//protected GameObject prefab;
    private ArticleType article;
    private string description;

    public MedicinalHerb(Node startingPos, GameObject prefab) : base(startingPos, prefab, false, "Medical Herb")
	{
		location = startingPos;
		this.prefab = prefab;
		used = false;
        this.article = ArticleType.MedicinalHerb;
    }

	private MedicinalHerb() : base(null, false, "NULL") { }

	public int getLocation()
	{
		return location.getIndex();
	}

	public void setLocationNode(Node x)
	{
		location = x;
	}

	public Node getLocationNode()
	{
		return location;
	}

	public GameObject getPrefab()
	{
		return prefab;
	}

    public void pickUp()
    {
        //turn renderer off
        //set an owner
        //mark as pickedUp 
    }

    void Article.useArticle()
    {
        //throw new System.NotImplementedException();
    }

    ArticleType Article.getArticle()
    {
        return ArticleType.MedicinalHerb;
    }

    string Article.articleToString()
    {
        return "Medical Herb";
    }

    string Article.getDescription()
    {
        return "Medical Herb Description";
    }

    public override void pickedUpSpecial()
    {
        
    }

    public override void DroppedSpecial()
    {
        
    }
}
