using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System;
using UnityEngine.SceneManagement;


namespace Andor
{
    [System.Serializable]
    public class Player
    {

        // [HideInInspector]
        // public GameObject sphere;

        //private Vector3 newPos;

        // ie Player-Male-Dwarf
        // also encodes the corresponding hero type
        // and Sphere-Male-Dwarf. sphere object is attached to this script.
        private string myTag;
        private string networkID;
        private string heroType;

        private HeroS myHero;
        // Will need to use this to verify things like: 
        // showTradeRequest() { if player.lookingAt != Battle then showTradeRequest() }
        // private Screen lookingAt;

        public Player() {
            myHero = new HeroS();
            networkID = PhotonNetwork.NickName;
        }

        public string getNetworkID()
        {
            return networkID;
        }
        public void setTag(string ID)
        {
            myTag = ID;
        }
        public string getTag()
        {
            return myTag;
        }

        public void setHero(HeroS hero)
        {
            myHero = hero;
        }

        public string getPlayerTag()
        {
            return myTag;
        }



        public string getHeroType()
        {
            return heroType;
        }
        public void setHeroType(string hero)
        {
            heroType = hero;
        }


        public override string ToString()
        {
            return "Username: " + networkID + ", player tag: " + myTag;
        }
    }
}

