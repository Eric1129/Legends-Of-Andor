using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterSelect : MonoBehaviour
{
    static public string character = "";
    static public Transform currentChar;
    static public Image currentBorder;


    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void onCharacterSelectClick()
    {
        currentChar = this.gameObject.transform.parent;
        character = currentChar.name;
        Debug.Log("Player: " + character);
        if(character == "Female Archer" || character == "Male Archer")
        {
            Game.myPlayer.setHeroType("archer");
            Game.myPlayer.setHeroRank(25);
        }
        else if (character == "Female Dwarf" || character == "Male Dwarf")
        {
            Game.myPlayer.setHeroType("dwarf");
            Game.myPlayer.setHeroRank(7);
        }
        else if (character == "Female Warrior" || character == "Male Warrior")
        {
            Game.myPlayer.setHeroType("warrior");
            Game.myPlayer.setHeroRank(14);
        }
        else if (character == "Female Wizard" || character == "Male Wizard")
        {
            Game.myPlayer.setHeroType("wizard");
            Game.myPlayer.setHeroRank(34);
        }
        /*
        if(currentBorder != null)
            currentBorder.color = new Color(255, 255, 255);
        currentBorder = currentChar.GetComponent<Image>();
        currentBorder.color = new Color(255, 0, 0);*/

        Game.myPlayer.setHeroType(character);
        Game.updatePlayer(Game.myPlayer);
    }
}
