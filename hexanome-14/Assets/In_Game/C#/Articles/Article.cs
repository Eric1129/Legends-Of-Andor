using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface Article 
{
    public enum ArticleType
    {
        Shield, Bow, Falcon, Wineskin, Telescope, WitchBrew, Helm
    }//add more if needed
    
    void useArticle();
    ArticleType getItem();
    string articleToString(); //return item.toString()
  

}
