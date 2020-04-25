using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class EmptyWell : Action
{
    //private string[] players;
    private Type type;
    private string[] players;


    public EmptyWell(string playerID){
        this.type = Type.EmptyWell;
        players = new string[] { playerID };

    }

    public Type getType()
    {
        return type;
    }

    public bool isLegal(GameState gs)
    {
        return true;
    }

    public string[] playersInvolved()
    {
        return players;
    }

    public void execute(GameState gs)
    {
        Dictionary<string, int> playerss = new Dictionary<string, int>();
        playerss = gs.getPlayerLocations();
        int loc = playerss[players[0]];
        Debug.Log(loc);
        foreach (Well w in gs.getWells().Keys)
        {
            if (w.getLocation() == loc && !w.used)
            {
                w.emptyWell();
                int currWillpower = gs.getPlayer(players[0]).getHero().getWillpower();
                Debug.Log(currWillpower);
                gs.getPlayer(players[0]).getHero().setWillpower(currWillpower + 3);
                GameController.instance.updateGameConsoleText(gs.getPlayer(players[0]).getHeroType() +" has emptied the well!"); Debug.Log("emptied well");

                //well.GetComponent<MeshRenderer>().material.SetColor("_Color", UnityEngine.Color.grey);
                // w.getPrefab().GetComponent<Renderer>().enabled = false;

            }
        }
        GameController.instance.emptyWellButton.gameObject.SetActive(false);

    }

    // Staris called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
