using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MedicinalHerb : Article
{

	private Node location;
	public bool used;
	protected GameObject prefab;
    private ArticleType article;
    private string description;

    public MedicinalHerb(Node startingPos, GameObject prefab)
	{
		location = startingPos;
		this.prefab = prefab;
		used = false;
        this.article = ArticleType.MedicinalHerb;
    }

	private MedicinalHerb() { }

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
        throw new System.NotImplementedException();
    }

    ArticleType Article.getArticle()
    {
        throw new System.NotImplementedException();
    }

    string Article.articleToString()
    {
        throw new System.NotImplementedException();
    }

    string Article.getDescription()
    {
        throw new System.NotImplementedException();
    }
}
