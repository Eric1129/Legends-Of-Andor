using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RoomLobbyController : MonoBehaviour
{
    public static RoomLobbyController instance;

    public Transform playerPanel;
    public Transform allPlayerContainer;

    public GameObject playerListingPrefab;

    private Dictionary<string, string> prevCharacterSelections = new Dictionary<string, string>();

    // Start is called before the first frame update
    void Start()
    {
        instance = this;

        Game.initGame(new Andor.Player());
    }

    public void playerListUpdate(List<Andor.Player> players)
    {
        
        Debug.Log("Listing Players");
        Debug.Log("Num Players: " + players.Count);
        this.removePlayers();

        foreach (Andor.Player player in players)
        {
            if (!prevCharacterSelections.ContainsKey(player.getNetworkID()))
            {
                prevCharacterSelections.Add(player.getNetworkID(), player.getHeroType());
            }
            else
            {
                // if character change
                Debug.Log(player.getNetworkID());
                Debug.Log(prevCharacterSelections);
                Debug.Log(prevCharacterSelections[player.getNetworkID()]);
                Debug.Log(player.getHeroType());
                if (!prevCharacterSelections[player.getNetworkID()].Equals(player.getHeroType()))
                {
                    removePlayerBorders();

                    foreach (Andor.Player p in Game.gameState.getPlayers())
                    {
                        highlightPlayer(p.getHeroType(), new Color32(player.color[0], player.color[1], player.color[2], 200));
                    }
                        
                }
            }
            listPlayer(player);
        }

    }
    public void listPlayer(Andor.Player player)
    {

        GameObject temp = Instantiate(playerListingPrefab, playerPanel);
        PlayerDetails pd = temp.GetComponent<PlayerDetails>();

        pd.mainContainer.GetComponent<Image>().color = new Color32(player.color[0], player.color[1], player.color[2], 130);
        pd.nameLabel.text = player.getNetworkID();
        pd.heroLabel.text = player.getHeroType();
        pd.setReady(player.ready);

        Debug.Log(player.getNetworkID() + " - " + player.ready);

        if (player.getNetworkID() != Game.myPlayer.getNetworkID())
        {
            Debug.Log("NetworkID: " + player.getNetworkID() + " != myPlayer NetworkID: " + Game.myPlayer.getNetworkID());
            pd.readyButton.interactable = false;
        }
        else
        {
            pd.readyButton.interactable = true;
        }
    }

    private void removePlayers()
    {

        // Remove previous player listings
        for (int i = playerPanel.childCount - 1; i >= 0; i--)
        {
            Destroy(playerPanel.GetChild(0).gameObject);

        }
        playerPanel.DetachChildren();

    }

    private void removePlayerBorders()
    {
        for(int i = 0; i<allPlayerContainer.childCount; i++)
        {
            allPlayerContainer.GetChild(i).GetComponent<Image>().color = Color.white;
        }
    }

    private void highlightPlayer(string hero, Color32 color)
    {
        Debug.Log(color.ToString());
        Debug.Log("Looking for " + hero);
        for (int i = 0; i < allPlayerContainer.childCount; i++)
        {
            Debug.Log(allPlayerContainer.GetChild(i).name);

            if (allPlayerContainer.GetChild(i).name.Equals(hero))
            {
                Debug.Log("MATCHING!");
                allPlayerContainer.GetChild(i).GetComponent<Image>().color = color;
                break;
            }
        }
    }

}
