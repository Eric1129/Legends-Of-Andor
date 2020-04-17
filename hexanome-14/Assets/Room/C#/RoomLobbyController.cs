using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;

public class RoomLobbyController : MonoBehaviour
{
    public static RoomLobbyController instance;
    public static GameState preLoadedGameState;

    public Transform playerPanel;
    public Transform allPlayerContainer;

    public GameObject playerListingPrefab;
    public GameObject playerListingPrefabLoaded;

    public Dictionary<string, int> playerMatches = new Dictionary<string, int>();

    public Button startButton;
    public Text legendLabel;

    private Dictionary<string, string> prevCharacterSelections = new Dictionary<string, string>();
    private bool allReady = false;


    // Start is called before the first frame update
    void Start()
    {
        instance = this;
        Game.started = false;

        if (PhotonNetwork.IsMasterClient)
        {
            startButton.gameObject.SetActive(true);

        }
        else
        {
            startButton.gameObject.SetActive(false);
        }

        Game.createPV();
        Game.initGame(new Andor.Player());
        if (preLoadedGameState == null)
        {
            allPlayerContainer.gameObject.SetActive(true);
        }
        else
        {
            Game.PREGAMEupdateGameState(preLoadedGameState);
            allPlayerContainer.gameObject.SetActive(false);

        }

        // Update Legend
        if (PhotonNetwork.IsMasterClient)
        {
            Game.updateDifficulty(Create_Game.DIFFICULTY);
        }
    }

    public void playerListUpdate(List<Andor.Player> players)
    {
        
        Debug.Log("Listing Players");
        Debug.Log("Num Players: " + players.Count);
        this.removePlayers();

        allReady = true;

        foreach (Andor.Player player in players)
        {
            if (!player.ready || player.getHeroType().Equals(""))
            {
                allReady = false;
            }

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
                        highlightPlayer(p.getHeroType(), new Color32((byte)(p.getColor().r-50), (byte)(p.getColor().g - 50), (byte)(p.getColor().b - 50), 200));
                    }
                    // Enables/Disables player selection
                    //characterControl();

                }
            }
            listPlayer(player);
        }
        // Make sure there are more than 2 players in the game
        if(Game.gameState.getPlayers().Count < 2)
        {
            // NOTE: UNCOMMENT FOR REAL GAME
            //allReady = false;
        }

        if (PhotonNetwork.IsMasterClient)
        {
            if (allReady)
            {
                startButton.interactable = true;
            }
            else
            {
                startButton.interactable = false;
            }
        }
        

    }
    public void listPlayer(Andor.Player player)
    {

        GameObject temp = Instantiate(playerListingPrefab, playerPanel);
        PlayerDetails pd = temp.GetComponent<PlayerDetails>();

        pd.mainContainer.GetComponent<Image>().color = player.getColor(130);
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


    // Disables heros that are already chosen 
    private void characterControl()
    {
        // Set every button to interactable
        for (int i = 0; i < allPlayerContainer.childCount; i++)
        {
            allPlayerContainer.GetChild(i).GetChild(0).GetComponent<Button>().interactable = true;
        }

        // Disable interactions based on the players picked
        foreach (Andor.Player player in Game.gameState.getPlayers())
        {
            for (int i = 0; i < allPlayerContainer.childCount; i++)
            {
                Transform child = allPlayerContainer.GetChild(i);

                if (child.name.Split(' ')[1].Equals(player.getHeroType().Split(' ')[1]))
                {
                    child.GetChild(0).GetComponent<Button>().interactable = false;
                }
            }
        }
    }

    public void leaveRoomClick()
    {
        RoomLobbyController.preLoadedGameState = null;
        PhotonNetwork.LeaveRoom();
    }
    public void startGameClick()
    {
        if(preLoadedGameState != null)
        {
            Game.PREGAMEstartGame();
        }
        Debug.Log("HERE");
        Game.destroyPV();
        PhotonNetwork.LoadLevel("Game");
    }



    #region fromSavedGameLogic
    public void playerListUpdateLOADED(List<Andor.Player> players)
    {
        Debug.Log("DEBUG LINE");
        RoomLobbyController.preLoadedGameState.displayPlayers();
        Game.gameState.displayPlayers();

        Debug.Log("Listing Players");
        Debug.Log("Num Players: " + players.Count);
        this.removePlayers();

        allReady = true;
        List<int> choosenPlayers = new List<int>();

        foreach (Andor.Player player in players)
        {
            Debug.Log("~~~~~");
            Debug.Log(player);
            if (!playerMatches.ContainsKey(player.getNetworkID()))
            {
                playerMatches.Add(player.getNetworkID(), 0);
            }
            playerMatches[player.getNetworkID()] = player.getHero().getGold();

            if (!player.ready || choosenPlayers.Contains(playerMatches[player.getNetworkID()]) || playerMatches[player.getNetworkID()] == 0)
            {
                allReady = false;
            }
            else
            {
                choosenPlayers.Add(playerMatches[player.getNetworkID()]);
            }
            

            listPlayerLOADED(player);
        }
        if (PhotonNetwork.IsMasterClient)
        {
            if (allReady)
            {
                startButton.interactable = true;
            }
            else
            {
                startButton.interactable = false;
            }
        }


    }

    public void listPlayerLOADED(Andor.Player player)
    {

        GameObject temp = Instantiate(playerListingPrefabLoaded, playerPanel);
        PlayerDetailsLoaded pdl = temp.GetComponent<PlayerDetailsLoaded>();

        pdl.mainContainer.GetComponent<Image>().color = player.getColor(130);
        pdl.nameLabel.text = player.getNetworkID();
        pdl.setReady(player.ready);
        pdl.init(player);

        if (player.getNetworkID() != Game.myPlayer.getNetworkID())
        {
            pdl.readyButton.interactable = false;
        }
        else
        {
            pdl.readyButton.interactable = true;
        }

    }




    #endregion

}
