using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Farmer : Article
{
    private Node location;
    public bool pickedUp;
    protected GameObject prefab;

    public Farmer(Node startingPos, GameObject prefab)
    {
        location = startingPos;
        this.prefab = prefab;
        pickedUp = false;
    }

    private Farmer() { }

    public void picked()
    {
        this.pickedUp = true;
        this.prefab.GetComponent<Renderer>().enabled = false;
        //assign to player who picked it up

    }

    public void dropped()
    {
        this.pickedUp = false;
        this.prefab.GetComponent<Renderer>().enabled = true;
        //remove from player who initially picked it up 
    }

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
