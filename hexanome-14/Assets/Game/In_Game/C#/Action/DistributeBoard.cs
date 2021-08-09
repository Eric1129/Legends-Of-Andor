using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

[System.Serializable]
public class DistributeBoard : Action
{
    string[] players;
    Type type;
    Dictionary<string, int> playerGold = new Dictionary<string, int>();
    Dictionary<string, int> playerWineskin = new Dictionary<string, int>();

    string rewardType;

    public DistributeBoard(string[] players, Dictionary<string, int> playerGold, Dictionary<string, int> playerWineskin)
    {
        type = Type.DistributeBoard;
        this.players = players;
        this.playerGold = playerGold;
        this.playerWineskin = playerWineskin;
    }

    public string[] playersInvolved()
    {
        return this.players;
    }
    public Type getType()
    {
        return this.type;
    }

    public void execute(GameState gs)
    {
        foreach(string p in playerGold.Keys)
        {

            Hero h = Game.gameState.getPlayer(p).getHero();
            Debug.Log(h.getHeroType() + " gets " + playerGold[p] + " gold. ");
            h.setGold(playerGold[p]);
        }

        foreach (string p in playerWineskin.Keys)
        {
            Hero h = Game.gameState.getPlayer(p).getHero();
            Debug.Log(h.getHeroType() + " gets " + playerWineskin[p] + " wineskin. ");
            for (int i =0; i< playerWineskin[p]; i++)
            {
                h.addArticle(new Wineskin());
            }
            //h.setWineskin(playerWineskin[p]);
        }
        GameController.instance.updateHeroStats();
        GameController.instance.das.closeScreens();
        if(PhotonNetwork.IsMasterClient){
            GameController.instance.rollDieForRunestoneLegend.SetActive(true);
        }  
    }

    public bool isLegal(GameState gs)
    {
        return true;
    }

   
}