using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ArticleType
{
    Shield, Bow, MedicinalHerb, Falcon, Wineskin, Telescope, WitchBrew, Helm, MedicinaHerb, Farmer
}//add more if needed
public interface Article 
{
    //each class should have a field ArticleType article

    void useArticle();
    ArticleType getArticle();
    string articleToString(); //return item.toString()
    string getDescription();
   


}
