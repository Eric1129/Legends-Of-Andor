using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{

    private string userName;

    // ie Player-Male-Dwarf
    // also encodes the corresponding hero type
    // and Sphere-Male-Dwarf. sphere object is attached to this script.
    private string myTag;
    private string heroType;
    private Hero myHero;

    // Will need to use this to verify things like: 
    // showTradeRequest() { if player.lookingAt != Battle then showTradeRequest() }
    // private Screen lookingAt;


    public void setTag(string ID)
    {
        myTag = ID;
        setHeroType();
    }

    public void setHero(Hero hero)
    {
        myHero = hero;
    }

    public string getPlayerTag()
    {
        return myTag;
    }

    public string getHeroType()
    {
        return heroType;
    }

    void Update()
    {
        // first check if Input.GetButtonDown("buttonName") -- based on screens (iterate over button in screen)
        // ie left-click
        if (Input.GetMouseButtonDown(0))
        {
             Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
             RaycastHit hitInfo;
             if (!Physics.Raycast(ray, out hitInfo))
             {
                 return;
             }

            string colliderTag = hitInfo.collider.gameObject.tag;
            if (colliderTag == null)
            {
                return;
            }
            // loop over button tags and then check if player.currSelected == player.selectedBoardPos

            // GameObject gameObj = GameObject.FindWithTag("Master");
            // masterClass master = GetComponent<masterClass>();
            // master.notifyClick();
        }
    }

    // extracts the hero tag from our own tag
    // EX.)   if (myTag == "Player-Male-Dwarf") -> output "Male-Dwarf"
    private void setHeroType()
    {
        // must initialize before adding strings
        heroType = "";
        int ct = 0;
        string[] partsOfTag = myTag.Split('-');
        if (partsOfTag.Length != 3)
        {
            Debug.Log("Found a bad tag in Player.setHeroType: " + myTag);
        }

        heroType = partsOfTag[1] + "-" + partsOfTag[2];
        //foreach(string partOfTag in myTag.Split('-'))
        //{
        //    // skip first val: "Player"
        //    if (ct++ == 0) continue;

        //    heroType += partOfTag;
        //}
    }


}
