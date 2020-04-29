using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Reflection;

public class eventCards : MonoBehaviour
{
    public static Andor.Player archer;
    public static Andor.Player warrior;
    public static Andor.Player wizard;
    public static Andor.Player dwarf;

    public bool hasArcher;
    public bool hasWarrior;
    public bool hasWizard;
    public bool hasDwarf;
    public static ArrayList borderingRiver = new ArrayList() { 15, 9, 8, 11, 12, 13, 116, 32, 38, 28, 29, 30, 33, 31, 27, 26, 49, 48, 47, 56, 63, 64, 46, 44, 42, 39, 40, 41, 37 };

    public static eventCards instance;

    private void setUp()
    {
        foreach (Andor.Player player in Game.gameState.getPlayers())
        {
            if (player.getHeroType() == "Male Archer" || player.getHeroType() == "Female Archer")
            {
                archer = player;
                hasArcher = true;
            }
            if (player.getHeroType() == "Male Warrior" || player.getHeroType() == "Female Warrior")
            {
                warrior = player;
                hasWarrior = true;
            }
            if (player.getHeroType() == "Male Dwarf" || player.getHeroType() == "Female Dwarf")
            {
                dwarf = player;
                hasDwarf = true;
            }
            if (player.getHeroType() == "Male Wizard" || player.getHeroType() == "Female Wizard")
            {
                wizard = player;
                hasWizard = true;
            }
        }

    }

    IEnumerator consoleCoroutine(string message)
    {
        GameController.instance.updateGameConsoleText(message);
        //yield on a new YieldInstruction that waits for 5 seconds.
        yield return new WaitForSeconds(3);
    }
    // Start is called before the first frame update
    void Start()
    {
        setUp();
    }

    public static void execute(int num)
    {
        // Reset everything before going to event card
        Game.gameState.TIME_endTime = 10;
        Game.gameState.TIME_overtimeCost = 2;
        Game.gameState.TIME_overtime = 8;





        // Apply event card
        if (num == 1)
        {
            eventCard1();
        }
        if (num == 2)
        {
            eventCard2();
        }
        if (num == 3)
        {
            eventCard3();
        }
        if (num == 4)
        {
            eventCard4();
        }
        if (num == 11)
        {
            eventCard11();
        }
        if (num == 9)
        {
            eventCard9();
        }
        if (num == 13)
        {
            eventCard13();
        }
        if (num == 14)
        {
            eventCard14();
        }
        if (num == 15)
        {
            eventCard15();
        }
        if (num == 17)
        {
            eventCard17();
        }
        if (num == 19)
        {
            eventCard19();
        }
        if (num == 22)
        {
            eventCard22();
        }
        if (num == 24)
        {
            eventCard24();
        }
        if (num == 28)
        {
            eventCard28();
        }
        if (num == 31)
        {
            eventCard31();
        }
        if (num == 32)
        {
            eventCard32();
        }
        if (num == 39)
        {
            eventCard39();
        }
        if (num == 40)
        {
            eventCard40();
        }
        //string meth = "eventCard" + num.ToString();
        //MethodInfo method = eventCards.instance.GetType().GetMethod("eventCard" + num.ToString());
        //method.Invoke(eventCards.instance, null);
    }
    //     //EVENT CARD - 1

    //     //EVENT CARD - 2
    public static void eventCard2()
    {
        GameController.instance.updateGameConsoleText("2-  Each hero standing on a space with a number between 0 and 20 now loses 3 willpower points.");
        foreach (Andor.Player player in Game.gameState.getPlayers())
        {
            player.getNetworkID();

            Dictionary<string, int> players = new Dictionary<string, int>();
            players = Game.gameState.getPlayerLocations();
            int location = players[player.getNetworkID()];
            if (location <= 20 && location >= 0)
            {
                int currWillpower = player.getHero().getWillpower();
                player.getHero().setWillpower(currWillpower - 3);
            }
        }
    }


    //     //EVENT CARD - 3
    public static void eventCard3()
    {
        List<Andor.Player> p = new List<Andor.Player>();
        Andor.Player playerOnTree = new Andor.Player();
        int rank = -1000000;
        int i = 0;
        GameController.instance.updateGameConsoleText("3 - A hero who enters the Tree of Songs space or is already standing there gets 1 strength point");
        foreach(Andor.Player player in Game.gameState.getPlayers())
        {
            i++;
            Dictionary<string, int> players = new Dictionary<string, int>();
            players = Game.gameState.getPlayerLocations();
            int location = players[player.getNetworkID()];
            if (location == 57)
            {
                p.Add(player);
            }

        }

        //if someone is already on the spot
        if(p.Count >= 1)
        {
            foreach(Andor.Player play in p)
            {
                if(play.getHero().getHeroRank() > rank)
                {
                    rank = play.getHeroRank();
                    playerOnTree = play;
                }
            }
            playerOnTree.getHero().increaseStrength(1);
        }

        //else
        //Game.gameState.eventcard3 = true;

    }

    //     //EVENT CARD - 4
    public static void eventCard4()
    {
        GameController.instance.updateGameConsoleText("4 - The well token on space 55 is removed from the game");
        foreach (KeyValuePair<Well, int> w in Game.gameState.getWells())
        {
            if (w.Value == 55)
            {
                Object.Destroy(w.Key.getPrefab());
                Dictionary<Well, int> wells = new Dictionary<Well, int>();
                wells = Game.gameState.getWells();
                wells.Remove(w.Key);
                Game.gameState.setWells(wells);
                break;
            }
        }
    }


    //     //EVENT CARD - 5

    //     //EVENT CARD - 6
    public static void eventCard6()
    {
        //GameConsole.displayMessage("6- Hero with the lowest rank gets to decide if he wants to roll one of his hero dice.");
    }

    //     //EVENT CARD - 7
    //      public static void eventCard7(Array playerList){
    //         GameConsole.displayMessage("7-  The hero with the lowest rank rolls one of his hero dice.The group loses the rolled number of willpower points.");
    //         //TO DO 
    //     }

    //     //EVENT CARD - 8

    //     //EVENT CARD - 9
    public static void eventCard9()
    {
        GameController.instance.updateGameConsoleText("On this day, no hero is allowed to use a 10th hour.");
        Game.gameState.TIME_endTime = 9;
    }

    //     //EVENT CARD - 10
    public static void eventCard10()
    {
        //GameConsole.displayMessage("10 - Each hero can now purchase 3 willpower points in exchange for 1 gold");
    }

    //EVENT CARD - 11
    public static void eventCard11()
    {
        GameController.instance.updateGameConsoleText("11- The creatures gather their strength...");
        GameController.instance.updateGameConsoleText("11- Each creature has 1 extra strength point.");
        List<Monster> monsters = new List<Monster>();
        monsters = Game.gameState.getMonsters();
        foreach (Monster m in monsters)
        {
            int currStrength = m.getStrength();
            m.setStrength(currStrength + 1);
        }
    }



    //EVENT CARD - 12


    //EVENT CARD - 13
    public static void eventCard13()
    {
        GameController.instance.updateGameConsoleText("13 - The lovely sound of a horn echoes across the land...");
        GameController.instance.updateGameConsoleText("13-  Each Hero who has fewer than 10 willpower points can immediately raise his total to 10.");
        foreach (Andor.Player player in Game.gameState.getPlayers())
        {
            if (player.getHero().getWillpower() < 10)
            {
                player.getHero().setWillpower(10);
            }
        }
    }

    //EVENT CARD - 14
    public static void eventCard14()
    {
        GameController.instance.updateGameConsoleText("14 - A fragment of a very old sculpture has been found. Not all of the heroes are able to appreciate that kind of handiwork...");
        GameController.instance.updateGameConsoleText("14 - The Dwarf and Warrior immediately get 3 willpower points each.");
        if (instance.hasDwarf)
        {
            int currWillpower = dwarf.getHero().getWillpower();
            Debug.Log("14");
            dwarf.getHero().setWillpower(currWillpower + 3);
        }

        if (instance.hasWarrior)
        {
            int currWillpower = warrior.getHero().getWillpower();
            warrior.getHero().setWillpower(currWillpower + 3);
        }
    }


    //     //EVENT CARD - 15
    public static void eventCard15()
    {
        GameController.instance.updateGameConsoleText("15 - The well token on space 35 is removed from the game");
        foreach (KeyValuePair<Well, int> w in Game.gameState.getWells())
        {
            if (w.Value == 35)
            {
                Object.Destroy(w.Key.getPrefab());
                Dictionary<Well, int> wells = new Dictionary<Well, int>();
                wells = Game.gameState.getWells();
                wells.Remove(w.Key);
                Game.gameState.setWells(wells);
                break;
            }
        }
    }

    //     //EVENT CARD - 16

    //     //EVENT CARD - 17
    //EVENT CARD - 24
    public static void eventCard17()
    {
        GameController.instance.updateGameConsoleText("17-  Heavy weather moves across the land.");
        GameController.instance.updateGameConsoleText("17-  Each hero with more than 12 willpower points immediately reduced his point total to 12");
        foreach (Andor.Player player in Game.gameState.getPlayers())
        {
            if (player.getHero().getStrength() > 12)
            {
                int currStrength = player.getHero().getStrength();
                player.getHero().setStrength(12);
            }
        }
    }

    //     //EVENT CARD - 18

    //     //EVENT CARD - 19
    public static void eventCard19()
    {
        //Game.gameState.eventcard19 = true;
        GameController.instance.updateGameConsoleText("19 - On this day, the 9th and 10th hours will each cost 3 willpower points instead of two");
        Game.gameState.TIME_overtimeCost = 3;
    
    }

    //     //EVENT CARD - 20


    //     //EVENT CARD - 24

    //     public static void eventCard24(Array playerList){
    //         GameConsole.displayMessage("24-  Any hero not on a forest space loses 2 wllpower points.");
    //         foreach (string playerTag in playerList){
    //         	int playerPos = convertToInt(GameObject.FindWithTag(playerTag).GetComponent<BoardPosition>.position);
    //             if (playerPos = 71 || playerPos = 72 || playerPos = 0){
    //                 GameObject.FindWithTag(playerTag).GetComponent<Hero>.willpower -= 2;
    //             }
    //         }
    //     }



    public static void eventCard22()
    {
        GameController.instance.updateGameConsoleText("22 - The well token on space 45 is removed from the game");
        foreach (KeyValuePair<Well, int> w in Game.gameState.getWells())
        {
            if (w.Value == 45)
            {
                Object.Destroy(w.Key.getPrefab());
                Dictionary<Well, int> wells = new Dictionary<Well, int>();
                wells = Game.gameState.getWells();
                wells.Remove(w.Key);
                Game.gameState.setWells(wells);
                break;
            }
        }
    }


    //EVENT CARD - 26
    public static void eventCard26()
    {
        GameController.instance.updateGameConsoleText("26-  The ministrels sing a ballad about the deeds of the heroes, strengthening their determination.");
        GameController.instance.updateGameConsoleText("26-  On this, day the 8th hour costs no willpower points.");
        Game.gameState.TIME_overtime = 9;
    }

    //EVENT CARD - 24
    public static void eventCard24()
    {
        GameController.instance.updateGameConsoleText("24-  A storm moves across the countryside and weighs upon the mood of the heroes...");
        GameController.instance.updateGameConsoleText("24-  Any hero not on a forest space loses 2 wllpower points.");
        foreach (Andor.Player player in Game.gameState.getPlayers())
        {
            player.getNetworkID();

            Dictionary<string, int> players = new Dictionary<string, int>();
            players = Game.gameState.getPlayerLocations();
            int location = players[player.getNetworkID()];
            if (location == 71 || location == 72 || location == 0)
            {
                int currWillpower = player.getHero().getWillpower();
                player.getHero().setWillpower(currWillpower - 2);
            }
        }
    }

    //EVENT CARD - 28
    public static void eventCard28()
    {
        GameController.instance.updateGameConsoleText("28 - A beautifully clear, starry night gives the heroes confidence...");
        GameController.instance.updateGameConsoleText("28 - Every Hero whose time marker is presently in the sunrise box gets 2 willpower points");
        foreach (Andor.Player player in Game.gameState.getPlayers())
        {
            if (player.getHero().getHour() == 0)
            {
                int currWillpower = player.getHero().getWillpower();
                player.getHero().setWillpower(currWillpower + 2);
            }
        }
    }


    //EVENT CARD - 31
    public static void eventCard31()
    {
        GameController.instance.updateGameConsoleText("31-  Hot rain from the south lashes the land...");
        GameController.instance.updateGameConsoleText("31-  Any hero not on a forest space loses 2 wllpower points.");
        foreach (Andor.Player player in Game.gameState.getPlayers())
        {
            player.getNetworkID();

            Dictionary<string, int> players = new Dictionary<string, int>();
            players = Game.gameState.getPlayerLocations();
            int location = players[player.getNetworkID()];
            if (location == 71 || location == 72 || location == 0)
            {
                int currWillpower = player.getHero().getWillpower();
                player.getHero().setWillpower(currWillpower - 2);
            }
        }
    }


    //EVENT CARD - 32
    public static void eventCard32()
    {
        GameController.instance.updateGameConsoleText("32: A sleepless night awaits the heroes...");
        GameController.instance.updateGameConsoleText("32: Each Hero whose time marker is presently in the sunrise box loses 2 willpower points");
        foreach (Andor.Player player in Game.gameState.getPlayers())
        {
            if (player.getHero().getHour() == 0)
            {
                int currWillpower = player.getHero().getWillpower();
                player.getHero().setWillpower(currWillpower - 2);
            }
        }
    }


    //EVENT CARD - RANDOM
    public static void eventCard1()
    {
        GameController.instance.updateGameConsoleText("Not Sure 3 -  Each hero standing on a space with a number between 37 and 70 now loses 3 willpower points.");
        foreach (Andor.Player player in Game.gameState.getPlayers())
        {
            player.getNetworkID();

            Dictionary<string, int> players = new Dictionary<string, int>();
            players = Game.gameState.getPlayerLocations();
            int location = players[player.getNetworkID()];
            if (location >= 37 || location <= 70)
            {
                int currWillpower = player.getHero().getWillpower();
                player.getHero().setWillpower(currWillpower - 3);
            }
        }
    }

    //EVENT CARD - Random2
    public static void eventCard39()
    {
        GameController.instance.updateGameConsoleText("39 - The wizard and the archer each immediately get 3 willpower points.");
        if (instance.hasWizard)
        {
            int willpower = wizard.getHero().getWillpower();
            wizard.getHero().setWillpower(willpower + 3);
        }
        if (instance.hasArcher)
        {
            int willpower = archer.getHero().getWillpower();
            archer.getHero().setWillpower(willpower + 3);
        }
    }


    //random - bordering river
    public static void eventCard40()
    {
        GameController.instance.updateGameConsoleText("idk - Each hero is now standing on a space bordering the river gets a wineskin");
        foreach (Andor.Player player in Game.gameState.getPlayers())
        {
            Dictionary<string, int> players = new Dictionary<string, int>();
            players = Game.gameState.getPlayerLocations();
            int location = players[player.getNetworkID()];
            if (borderingRiver.Contains(location))
            {
                player.getHero().addArticle(new Wineskin());
                Game.gameState.removeFromEquimentBoard("Wineskin");

            }

        }

    }

}