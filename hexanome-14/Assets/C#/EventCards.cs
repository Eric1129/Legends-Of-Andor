using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;


public class EventCards : MonoBehaviour
{
 
    public class Event
    {
        private int id;
        private string file;
        //private bool removeAtEOD = false;

        public Event(int id,string file)
        {
            this.id = id;
            this.file = file;
        }

        public int getID()
        {
            return this.id;
        }
        public string getFile()
        {
            return this.file;
        }

        //Some event cards apply a rule till the end of the day and are removed afterward
        //in the end day function we will need to call reset(id)
        public void reset(int id)
        {
            switch (id)
            {
                case 1:
                    //renable 10th hour
                    break;
                case 4:
                    //set boolean back to false
                    break;
                
                
            }
        }

    }

    private Stack<Event> deck = new Stack<Event>();
    private int numCards = 4;
    public EventCards(bool loadFromSave)
    {
        if (loadFromSave)
        {
            //@TODO DATABASE: go through _order_ database and update the deck
        }
        else
        {
            for (int i = 1; i <= numCards; i++)
            {

                Event e = new Event(i, "event" + i);
                deck.Push(e);
            }
            

            //@TODO: DATABASE store this ordering in the database _order_(filename PK, pos)
        }



    }
    public EventCards()
    {
      
        for (int i = 1; i <= numCards; i++)
        {

            Event e = new Event(i, "event" + i);
            deck.Push(e);
        }
      
        //@TODO: DATABASE store this ordering in the database _order_(filename PK, pos)


    }


    public void loadEventCard()
    {
        Event top = deck.Pop();
        //display this card

        string file = top.getFile();

        Sprite sprite;
        sprite = Resources.Load<Sprite>("Images/" + file);
        GameObject image = GameObject.Find("Image");
        image.GetComponent<Image>().sprite = sprite;

        
        //@TODO DATABASE: remove card from database and update order
        int id = top.getID();
        executeEvent(id);

    }

    //At the beginning of each day this met
    public int executeEvent(int id)
    {
        /*@TODO OTHER CLASSES: each card has a different effect on different classes
        might need to create a different function for each card :/
         */
        switch (id)
        {
            case 1:
                print("case 1!");
                //for each hero, disable the 10th hour
                return 1;
            case 2:
                print("case 2!");
                //find all heroes with strength <= 6
                //display each of these heroes as buttons
                //---allow for a selection of 2 heroes
                //increase strength of these heroes by 1
                return 2;
            case 3:
                print("case 3!");
                //loop through the heroes on the board
                //if the hero space is not 0, 71, 72 or a forest space (??) decrease willpower by 2
                return 3;
            case 4:
                print("case 4!");
                //in the class that effects overtime hours:
                //set some boolean
                //if that boolean is true then the wp loss is -3 instead of -2
                return 4;
            default:
                return 0;
          
            
        }
    }

    public void backToBoard()
    {
        //@TODO: load main game board scene
    }


    

    
}

