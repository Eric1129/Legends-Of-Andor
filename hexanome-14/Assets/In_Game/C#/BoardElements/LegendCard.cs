using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LegendCard : MonoBehaviour
{
    
    public Text currentText;
    public bool easy;
    public char currentLegend;
    public int LegendNumber;
    
    // Start is called before the first frame update
    //void Start()
    //{
    //    currentLegend = 'A';
    //    LegendNumber = 1;
    //    easy = true;
    //    currentText.text = " A1:\n " +
    //        "Here’s a reminder before continuing to Legend 2:\n" +
    //        "A hero always chooses between two options: Move or fight.\n" +
    //        "Both cost time on the time track.Fighting costs 1 hour per battle round. Moving costs 1 hour per game board space.\n" +
    //        "If the hero does not want to move or fight, he can “pass:’ That will also cost him 1 hour.\n" +
    //        "The free actions:\n" +
    //        "• Activate a fog token\n" +
    //        "• Empty a well\n" +
    //        "• Pick up or deposit gold / gemstones or articles from or onto a space\n" +
    //        "• Trade or give gold/ gemstones or articles with or to another hero on the same space\n" +
    //        "• Use articles\n" +
    //        "• Buy articles or strength points from a merchant\n" +
    //        "None of these actions cost any hours on the time track.They can also be carried out when it isn’t the hero’s turn.\n" +
    //        "A hero cannot perform them, however, if he has already ended his day.\n" +
    //        "Now continue to Legend card A2.";
    //}

    // Update is called once per frame
    void Update()
    {
        
    }

    public void advanceLegendCard(int currentday)
    {

        switch (currentday)
        {
            case 3:
                currentLegend = 'C';
                LegendNumber = 1;
                string textC1 = "C1\n" +
                    "The king’s scouts have discovered tile skral stronghold. " +
                    "A hero rolls one hero die and adds 50 to the number rolled. " +
                    "This total number indicates the number of the space on which the skral stronghold is located. " +
                    "Place a tower on this space and a skral on top of the tower. " +
                    "If there is another creature on this same space, it is immediately removed from the game. " +
                    "The heroes may enter and pass through this space. " +
                    "The skral does not move at sunrise. " +
                    "Other creatures that would move into the space are instead advanced along the arrow to the next space. " +
                    "The skral on the tower has 6 willpower points and the following number of strength points: ";

                if (easy)
                {
                    textC1 += "for 2 heroes = 10, for 3 heroes = 20, for 4 heroes = 30";
                }
                else
                {
                    textC1 += "for 2 heroes = 20, for 3 heroes = 30, for 4 heroes =40";
                }

                textC1 += "Mark the strength point total with a star on the creature display. Task: The skral on the tower must be defeated. " +
                    "As soon as he is defeated, the Narrator is advanced to the letter “N” on the Legend track. " +
                    "And there’s more unsettling news: Rumors are circulating about cruel wardraks from the south. " +
                    "They have not yet been sighted, but more and more farmers are losing their courage, leaving their farmsteads, and seeking safety in the castle. " +
                    "Place a farmer token on space 28. " +
                    "Now continue to Legend card C2.";

                currentText.text = textC1;
                break;

            case 7:
                currentLegend = 'G';
                string textG = "G\n" +
                    "Prince Thorald joins up with a scouting patrol with the intention of leaving for just a few days. " +
                    "But he is not to be see again for quite a long time. " +
                    "Prince Thorald is removed from the game. Black shadows are moving in the moonlight. The rumors were right — the wardraks are corning! " +
                    "Place wardraks on spaces 26 and 27. " +
                    "If one of the spaces is already occupied by a creature, the new creature is moved along the arrow to the adjacent space. " +
                    "A wardrak has 10 strength and 7 willpower points, and uses 2 black dice in battle(see creature display). " +
                    "Identical dice values are added up.If the wardrak has fewer than 7 willpower points, it only has 1 black die available. " +
                    "These creatures are especially dangerous, because they move twice each sunrise, 1 space each time(see sunrise box). " +
                    "For every wardrak defeated, the heroes get a reward of 6 gold or 6 willpower points, or any combination of the two adding up to 6. " +
                    "Defeated wardraks are placed on space 0. " +
                    "Tip: To prepare for a collective battle against this powerful creature, " +
                    "calculate your collective strength points and place a star on a game board space with a number matching your total collective strength points.";
                currentText.text = textG;

                break;

            case 14:
                string textS = "You win！With their combined powers, the heroes were able to take the skral’s stronghold. " +
                    "The medicinal herb did its work as well, and King Brandur soon felt better. " +
                    "And yet, the heroes still felt troubled. The king’s son, Prince Thorald, had not yet returned. " +
                    "What was keeping him so long? In next Legend, you will find out.";
                currentText.text = textS;

                break;

            case 15:
                string textL = "You lose! Tips for next time: 1.Articles such as falcon and telescope can be very helpful. " +
                    "2.Prince Thorald’s extra strength can help in a battle against skrals. " +
                    "3.It is very important to find witch quickly. " +
                    "4.To save time, it is sometimes better for one hero to get the entire reward in gold. Then just one hero has to get to the merchant.";
                currentText.text = textL;
                break;

            // Runestone legend card, really complicated, need to be fixed later
            case 16:
                string textR;
                if (!easy)
                {
                    textR = "Runestones_hard:\n" +
                        "Place gors on 32 and 43 and one skral on 39. " +
                        "Important: Only continue reading this card if the witch has already been found.Otherwise, the card is removed from the game. " +
                        "Now let a roll of the dice determine the positions of 5 of the 6 hidden rune stones. " +
                        "One hero rolls one red die and one hero die. The red die indicates the “tens” place of the number and the hero die indicates the “ones” place. " +
                        "Example: red 4, green 2 = a rune stone is placed face - down on space 42.Note: " +
                        "More than one rune stone may be on a single space. " +
                        "The witch Reka tells the heroes about au ancient magic that still holds power: rune stones! " +
                        "The rune stones can be collected in the small storage spaces of the hero boards. " +
                        "Just like fog tokens, they can be uncovered with the help of the telescope(but not when just passing through a space). " +
                        "Note: Rune stones can also be uncovered and collected when a creature is on the same space as the rune stone. " +
                        "1.1 a hero has 3 different - colored rune stones on his hero board, he gets one black die, which has higher values than the hero dice. " +
                        "As long as the rune stones are on his board, he is allowed to use this black die in battle instead of his own dice. " +
                        "Note: The wizard can also use his special ability on the black die.";
                }
                else
                {
                    textR = "Runestones_easy:\n" +
                        "Place a gor on 43 and a skral on 39. " +
                        "Now let a roll of the dice determine the positions of 5 of the 6 hidden rune stones.One hero rolls one red die and one hero die. " +
                        "The red die indicates the “tens” place of the number and the hero die indicates the “ones” place. " +
                        "Example: red 4, green 2 = a rune stone is placed face -do wit on space 42. " +
                        "Note: " +
                        "More than one rune stone may be on a single space. " +
                        "The heroes learn about an ancient magic that still holds power: rune stones! " +
                        "The rune stones can be collected in the small storage spaces of the hero boards. " +
                        "Just like fog tokens, they can be uncovered with the help of the telescope(but not when just passing through a space). " +
                        "Note: Rune stones can also be uncovered and collected when a creature is on the same space as the rune stone. " +
                        "If a hero has 3 different - colored rune stones on his hero board, he gets one black die, which has higher values than the hero dice. " +
                        "As long as the rune stones are on his board, he is allowed to use this black die in battle instead of his own dice. " +
                        "Note: The wizard can also use his special ability on the black die.";
                }
                currentText.text = textR;
                break;

            case 17:
                string textW = "Witch\n" +
                                "Finally! There’s in the fog, one of the heroes discovers the witch named Reka." +
                                "The hero standing on the witch’s space activates the fog token and gets her magic potion for free. " +
                                "Place the witch on this space.From now on, a hero standing on the same space as the witch can buy her brew. " +
                                "The price depends on the number of heroes(see equipment board). Important: The archer always pays 1 gold less than the others. " +
                                "In a battle, the witch’s brew doubles the value of one die, and it can be used twice(front and rear side of the token). " +
                                "Reka knows where to find the medicinal herb to heal the king. " +
                                "One player rolls to determine the position of the medicinal herb: roll of 1 or 2 = medicinal herb on space 37. " +
                                "roll of 3 or 4 = medicinal herb on space 67. " +
                                "roll of 5 or 6 = medicinal herb on space 61. " +
                                "Place the medicinal herb on the space determined by the roll, and add a gor to the same space. " +
                                "The gor must be defeated before a hero can collect the herb. " +
                                "The gor takes the herb with him when he moves at sunrise. " +
                                "Task: When the Narrator reaches the letter “N” on the Legend track, the medicinal herb must be in space 0(the castle).";

                currentText.text = textW;
                break;
        }
    }

    public void nextLegend()
    {
        if(currentLegend == 'A')
        {
            LegendNumber++;
            if(LegendNumber == 2)
            {
                string textA2 = "A2\n" +
                                "You have probably already noticed that some of the cards in this Legend have two versions — one with a green background and the other with a normal background. " +
                                "That means that you can play the Legend at either of two different levels of difficulty. Just choose which you want before starting the game:" +
                                "A) If you use the cards with the green background, you will play the Legend at an easier level. OR " +
                                "B) If you use the cards with the regular tan background, you will play the Legend at a normal level of difficulty. " +
                                "Example: Card A3 below comes in a green version and a normal version. " +
                                "You decide which level of difficulty you want, and return the other A3 card to the box. " +
                                "Note: If a card does not have a green version, just use the normal version. " +
                                "Now continue to Legend card A3.";
                currentText.text = textA2;
            }

            if (LegendNumber == 3 && easy)
            {
                string textA3Easy = "A3_Easy\n" +
                                    "Start by carrying out the instructions on the Checklist card. Then, get the following materials ready and arrange them next to the game board: " +
                                    "• “The Witch” and “The Rune Stones” Legend cards, 1 witch figure, 1 horseman figure, “Prince Thorald,” 1 tower, I medicinal herb(any value) " +
                                    "• face - down and mixed up: 6 magic rune stones " +
                                    "Finally, place stars on letters C, G, and N of the Legend track. " +
                                    "A gloomy mood has fallen upon the people. Rumors are making the rounds that skrals have set up a stronghold in some undisclosed location. " +
                                    "The heroes have scattered themselves across the entire land in search of this location. " +
                                    "The defense of the castle is in their hands alone. " +
                                    "Place your heroes on the spaces corresponding to their rank numbers: dwarf on 7, warrior on 14, archer on 25, wizard on 34. " +
                                    "Place gors on spaces 8, 20, 21, 26,48 and one skral on 19. " +
                                    "Many farmers have asked for help and are seeking shelter behind the high walls of Riethurg Castle. " +
                                    "Place one farmer token on each of spaces 24 and 36. " +
                                    "Now continue to Legend card A4. ";
                currentText.text = textA3Easy;
            }
            if (LegendNumber == 3 && !easy)
            {
                string textA3Hard = "A3_Hard\n " +
                    "Start by carrying out the instructions on the Checklist card. Then, get the following materials ready and arrange them next to the game board: " +
                    "• “The Witch” and “The Rune Stones” Legend cards, 1 witch figure, 1 horseman figure, “Prince Thorald,” I tower, 1 medicinal herb(any value) " +
                    "• face - down and mixed up: 6 magic rune stones " +
                    "Finally, place stars on letters C, G, and N of the Legend track. " +
                    "A gloomy mood has fallen upon the people. Rumors are making the rounds that skrals have set up a stronghold in some undisclosed location. " +
                    "The heroes have scattered themselves across the entire land in search of this location.The defense of the castle is in their hands alone. " +
                    "Place your heroes on the spaces corresponding to their rank numbers: dwarf on 7, warrior on 14, archer on 25, wizard on 34 " +
                    "Place gors on spaces 8, 20, 21, 26,48 and one skral on 19. " +
                    "Many farmers have asked for help and are seeking shelter behind the high walls of Rietburg Castle. " +
                    "Place one farmer token on space 24. Now continue to Legend card A4.";

                currentText.text = textA3Hard;
            }
            if (LegendNumber == 4)
            {
                string textA4 = "A4\n" +
                                "This adventure starts with farmers who can be brought into the castle. " +
                                "The players can move their heroes to a farmer token and carry it along with their own figure. " +
                                "They can also do that if they just pass through a space with a farmer token. " +
                                "A hero can carry several farmer tokens at one time. " +
                                "If a hero carrying a farmer moves to a space with a creature or if a creature enters a space with a farmer, the farmer is immediately killed and removed from the game. " +
                                "The heroes can leave a farmer behind on a space at any time. " +
                                "Farmers who have been saved offer a great advantage: For each farmer brought into the castle, one extra creature can get into the castle without loss of the Legend. " +
                                "The farmer token is simply flipped onto its rear side and placed next to the golden shields. " +
                                "At first sunlight, the heroes receive a message: Old King Brandur’s willpower seems to have weakened with the passage of time. " +
                                "But there is said to be an herb growing in the mountain passes that can revive a person’s life. " +
                                "Task: The heroes must heal the king with the medicinal herb. To do that, they must find the witch. " +
                                "Only she knows the locations where this herb grows. The witch is hiding behind one of the fog tokens. " +
                                "Now continue to Legend card A5.";
                currentText.text = textA4;
            }
            if (LegendNumber == 5)
            {
                string textA5 = "A5\n" +
                                "When a hero enters a space with the fog token showing the witch’s brew, “The Witch” card is uncovered and read out loud. " +
                                "Note: There are 2 fog tokens that will bring a gor into play.When a hero activates one of those tokens, a gor is placed on that space. " +
                                "Now ft’s time to decide when “The Rune Stones” Legend card comes into play.One player rolls one of the hero dice. Note the little dice pips shown in the Legend track. " +
                                "Place “The Rune Stones” Legend card with its arrow pointing to the corresponding letter on the Legend track(matching the result o the rolled die). " +
                                "This card will be triggered when the Narrator reaches this letter. " +
                                "Important: From now on, any articles(in addition to strength points) may be purchased from the merchants(spaces 18, 57, and 71) for 2 gold each. " +
                                "See the equipment board for the functions of the articles. 1’Witch’s brew,” however, cannot be purchased from the merchant. " +
                                "Each hero starts with 2 strength points.The group gets 5 gold and 2 wineskins.You all decide together who gets what. " +
                                "The hero with the lowest rank will now begin.";
                currentText.text = textA5;
            }
        }
        if (currentLegend == 'C')
        {
            LegendNumber++;
            if (LegendNumber == 2)
            {
                string textC2 = "C2\n" +
                    "Place gors on 27 and 31, and one skral on 29. " +
                    "But there’s good news from the south too: Prince Tho raid, just back from a battle on the edge of the southern forest, " +
                    "is preparing himself to help the heroes. " +
                    "The players place Prince Thorald on the space with the tavern(72, to the right of space 23 in the southern forest). " +
                    "If the prince is standing on the same space as a creature, he counts as 4 extra strength points for the heroes in a battle with the creature. " +
                    "Instead of “fighting” or “moving’ a hero can now also choose the “move prince” action during his move. " +
                    "That will cost him 1 hour on the time track. " +
                    "He can move the prince up to 4 spaces.He can also do that several times during his turn(for example, move the prince up to 8 spaces at a cost of 2 hours). " +
                    "After the “move prince” action, it is the next hero’s turn. " +
                    "Note: Prince Thorald cannot collect any tokens or move any farmers. " +
                    "Prince Thorald accompanies the heroes up to letter on the Legend track. " +
                    "Legend goal: " +
                    "The Legend is won when the Narrator reaches letter “N” on the Legend track, and " +
                    "the castle has been defended, and " +
                    "the medicinal herb is on the castle space, and " +
                    "the skral on the tower has been defeated.";

                currentText.text = textC2;
            }

        }
    }

}
