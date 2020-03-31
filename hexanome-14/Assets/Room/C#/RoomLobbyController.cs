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
        removePlayers();

        foreach (Andor.Player player in players)
        {
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

        if(player.getNetworkID() != Game.myPlayer.getNetworkID())
        {
            pd.readyButton.interactable = false;
        }
    }

    public void removePlayers()
    {
        while(playerPanel.childCount != 0)
        {
            Destroy(playerPanel.GetChild(0).gameObject);
        }
    }

}
