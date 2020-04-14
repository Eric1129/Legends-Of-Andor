using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LegendCard : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void advanceLegendCard(char caseNo, bool difficulty)
    {

        switch (caseNo)
        {
            case 'A':
                string textA1 = "Here’s a reminder before continuing to Legend 2: A hero always chooses between two options: Move or fight. " +
                    "Both cost time on the time track.Fighting costs 1 hour per battle round. Moving costs 1 hour per game board space. " +
                    "If the hero does not want to move or fight, he can “pass:’ That will also cost him 1 hour. " +
                    "The free actions: " +
                    "• Activate a fog token " +
                    "• Empty a well " +
                    "• Pick up or deposit gold / gemstones or articles from or onto a space " +
                    "• Trade or give gold/ gemstones or articles with or to another hero on the same space " +
                    "• Use articles " +
                    "Buy articles or strength points from a merchant " +
                    "None of these actions cost any hours on the time track. They can also be carried out when it isn’t the hero’s turn. " +
                    "A hero cannot perform them, however, if he has already ended his day. " +
                    "Now continue to Legend card A2.";

                string textA2 = "";

                break;
            case 'C':
                string textC1 = "The king’s scouts have discovered tile skral stronghold. " +
                    "A hero rolls one hero die and adds 50 to the number rolled. " +
                    "This total number indicates the number of the space on which the skral stronghold is located. " +
                    "Place a tower on this space and a skral on top of the tower. " +
                    "If there is another creature on this same space, it is immediately removed from the game. " +
                    "The heroes may enter and pass through this space. " +
                    "The skral does not move at sunrise. " +
                    "Other creatures that would move into the space are instead advanced along the arrow to the next space. " +
                    "The skral on the tower has 6 willpower points and the following number of strength points: ";

                if (difficulty)
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


                string textC2 = "Place gors on 27 and 31, and one skral on 29. " +
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
                break;

            case 'G':
                string textG = "Prince Thorald joins up with a scouting patrol with the intention of leaving for just a few days. " +
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

                break;
            case 'S':
                string textS = "You win！With their combined powers, the heroes were able to take the skral’s stronghold. " +
                    "The medicinal herb did its work as well, and King Brandur soon felt better. " +
                    "And yet, the heroes still felt troubled. The king’s son, Prince Thorald, had not yet returned. " +
                    "What was keeping him so long? In next Legend, you will find out.";

                break;

            case 'L':
                string textL = "You lose! Tips for next time: 1.Articles such as falcon and telescope can be very helpful. " +
                    "2.Prince Thorald’s extra strength can help in a battle against skrals. " +
                    "3.It is very important to find witch quickly. " +
                    "4.To save time, it is sometimes better for one hero to get the entire reward in gold. Then just one hero has to get to the merchant.";
                break;

            // Runestone legend card, really complicated, need to be fixed later
            case 'R':
                string textR = "";
                if (difficulty)
                {
                    textR += "Place a gor on 43 and a skral on 39. ";
                }
                else
                {

                }

                textR += "Now let a roll of the dice determine the positions of 5 of the 6 hidden rune stones. " +
                    "One hero rolls one red die and one hero die. The red die indicates the “tens” place of the number and the hero die indicates the “ones” place. " +
                    "Example: red 4, green 2 = a rune stone is placed face-do wit on space 42. Note: More than one rune stone may be on a single space. " +
                    "The heroes learn about an ancient magic that still holds power: rune stones! " +
                    "The rune stones can be collected in the small storage spaces of the hero boards. " +
                    "Just like fog tokens, they can be uncovered with the help of the telescope(but not when just passing through a space). " +
                    "Note: Rune stones can also be uncovered and collected when a creature is on the same space as the rune stone. " +
                    "If a hero has 3 different - colored rune stones on his hero board, he gets one black die, which has higher values than the hero dice. " +
                    "As long as the rune stones are on his board, he is allowed to use this black die in battle instead of his own dice. " +
                    "Note: The wizard can also use his special ability on the black die. ";

                break;

            case 'W':
                string textW = "Finally! There’s in the fog, one of the heroes discovers the witch named Reka." +
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

                break;
          
        }
        
    }
}
