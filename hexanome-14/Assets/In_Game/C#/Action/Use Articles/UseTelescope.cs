using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;
using System.Reflection;




[System.Serializable]
public class UseTelescope : Action
{
    private Type type;
    private string[] players;

    public UseTelescope(string playerID)
    {
        type = Type.UseTelescope;
        players = new string[] { playerID };
    }

    public Type getType()
    {
        return type;
    }

    public string[] playersInvolved()
    {
        return players;
    }


    public bool isLegal(GameState gs)
    {
        if (gs.getPlayer(players[0]).getHero().allArticlesAsStringList().Contains("Telescope")){ 
            return true;
        }
        else
        {
            return false;
        }
    }

    public void execute(GameState gs)
    {
        int loc = gs.getPlayerLocations()[players[0]];
        GameController.instance.tele(loc);
        //List<Node> neighbors = Game.positionGraph.getNode(loc).getAdjacentNodes();
        //foreach(Node n in neighbors)
        //{
        //    int nodeIndex = n.getIndex();
        //    foreach(KeyValuePair<FogToken, int> f in gs.getFogTokens())
        //    {
        //        if (f.Value == nodeIndex)
        //        {
        //            GameObject fogToken = f.Key.getPrefab();
        //            UnityEngine.Object.Destroy(f.Key.getPrefab());
        //            if(f.Key.type == "brew")
        //            {
        //               fogToken = UnityEngine.Object.Instantiate(GameController.instance.brewToken, GameController.instance.tiles[nodeIndex].getMiddle(), transform.rotation);
                     
        //            }
        //            if (f.Key.type == "gor")
        //            {
        //                fogToken = UnityEngine.Object.Instantiate(GameController.instance.gorToken, GameController.instance.tiles[nodeIndex].getMiddle(), transform.rotation);
        //            }
        //            if (f.Key.type == "willpower2")
        //            {
        //                fogToken = UnityEngine.Object.Instantiate(GameController.instance.willpower2Token, GameController.instance.tiles[nodeIndex].getMiddle(), transform.rotation);
        //            }
        //            if (f.Key.type == "willpower3")
        //            {
        //                fogToken = UnityEngine.Object.Instantiate(GameController.instance.willpower3Token, GameController.instance.tiles[nodeIndex].getMiddle(), transform.rotation);
        //            }
        //            if (f.Key.type == "wineskin")
        //            {
        //                fogToken = UnityEngine.Object.Instantiate(GameController.instance.wineskinToken, GameController.instance.tiles[nodeIndex].getMiddle(), transform.rotation);
        //            }
        //            if (f.Key.type == "event")
        //            {
        //                fogToken = UnityEngine.Object.Instantiate(GameController.instance.eventToken, GameController.instance.tiles[nodeIndex].getMiddle(), transform.rotation);
        //            }
        //            if (f.Key.type == "gold1")
        //            {
        //                fogToken = UnityEngine.Object.Instantiate(GameController.instance.gold1Token, GameController.instance.tiles[nodeIndex].getMiddle(), transform.rotation);
        //            }
        //            f.Key.setPrefab(fogToken);

        //        }
        //    }
        //    //if (gs.getFogTokens().ContainsValue(loc))
        //    //{
        //    //    gs.getFogTokens().
        //    //}
        //}

    }
}
