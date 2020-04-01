using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RoomLobbyController : MonoBehaviour
{
    public static RoomLobbyController instance;

    public Transform playerPanel;

    public GameObject playerListingPrefab;

    // Start is called before the first frame update
    void Start()
    {
        instance = this;

        Game.initGame(new Andor.Player());
    }

    public void playerListUpdate(List<Andor.Player> players)
    {
        Debug.Log("Listing Players");
        this.removePlayers();

        foreach (Andor.Player player in players)
        {
            Debug.Log(player.ToString());

            listPlayer(player);
        }

    }
    public void listPlayer(Andor.Player player)
    {
        GameObject temp = Instantiate(playerListingPrefab, playerPanel);
        PlayerDetails pd = temp.GetComponent<PlayerDetails>();

        pd.nameLabel.text = player.getNetworkID();
        pd.heroLabel.text = player.getHeroType();
        pd.setReady(player.ready);

        if (player.getNetworkID() != Game.myPlayer.getNetworkID())
        {
            Debug.Log("NetworkID: " + player.getNetworkID() + " != myPlayer NetworkID: " + Game.myPlayer.getNetworkID());
            pd.readyButton.gameObject.SetActive(false);
        }
        else
        {
            pd.readyButton.gameObject.SetActive(true);
        }
    }

    public void removePlayers()
    {
        for (int i = 0; i< 10; i++)
        {
            if(playerPanel.childCount == 0)
            {
                break;
            }
            Destroy(playerPanel.GetChild(0).gameObject);
        }
    }

}
