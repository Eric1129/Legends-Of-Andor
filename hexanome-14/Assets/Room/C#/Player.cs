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
        private string networkID;
        public byte[] color;
        public bool ready = false;

        private HeroS myHero;
        // Will need to use this to verify things like: 
        // showTradeRequest() { if player.lookingAt != Battle then showTradeRequest() }
        // private Screen lookingAt;

        public Player() {
            myHero = new HeroS();
            networkID = PhotonNetwork.NickName;
            color = new byte[]{ (byte)Game.RANDOM.Next(150, 235), (byte)Game.RANDOM.Next(150, 235), (byte)Game.RANDOM.Next(150, 235), 130};
        }

        public string getNetworkID()
        {
            return networkID;
        }

        public void setHero(HeroS hero)
        {
            myHero = hero;
        }
        public HeroS getHero()
        {
            return myHero;
        }

        public string getHeroType()
        {
            return myHero.getHeroType();
        }
        public void setHeroType(string hero)
        {
            myHero.setHeroType(hero);
        }


        public override string ToString()
        {
            return "Username: " + networkID + ", player tag: " + getHeroType();
        }
    }
}

