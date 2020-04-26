using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ArticleType
{
    Shield, MedicinalHerb, Falcon, Wineskin, Telescope, WitchBrew, Helm, MedicinaHerb
}//add more if needed
public interface Article 
{
    //each class should have a field ArticleType article

    void useArticle();
    ArticleType getArticle();
    string articleToString(); //return item.toString()
    string getDescription();


}
