using System;
public class Article
{
    private int id;
    private string articleName; //need to change this to an enum
    private int cost;

    public Article(string item, int cost)
    {
        
        this.articleName = item;
        this.cost = cost;



    }
    public int getCost()
    {
        return this.cost;
    }

    public string getArticleName()
    {
        return this.articleName;
    }

}
