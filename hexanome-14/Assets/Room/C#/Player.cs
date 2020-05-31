using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System;
using UnityEngine.SceneManagement;
using Newtonsoft.Json;

namespace Andor
{
    [System.Serializable]
    [JsonObject(MemberSerialization.Fields)]
    public class Player
    {

        // [HideInInspector]
        // public GameObject sphere;

        //private Vector3 newPos;

        // ie Player-Male-Dwarf
        // also encodes the corresponding hero type
        // and Sphere-Male-Dwarf. sphere object is attached to this script.
        private string networkID;
        private byte[] color;
        public bool ready = false;


        private Hero myHero;
        // Will need to use this to verify things like: 
        // showTradeRequest() { if player.lookingAt != Battle then showTradeRequest() }
        // private Screen lookingAt;

        public Player() {
            myHero = new Hero();
            networkID = PhotonNetwork.NickName;
            color = new byte[]{ (byte)Game.RANDOM.Next(150, 235), (byte)Game.RANDOM.Next(150, 235), (byte)Game.RANDOM.Next(150, 235), 130};
            Debug.Log(color);
        }

        public string getNetworkID()
        {
            return networkID;
        }
        public void setNetworkID(string nid)
        {
            networkID = nid;
        }

        public void setHero(Hero hero)
        {
            myHero = hero;
        }
        public Hero getHero()
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

        public int getHeroRank()
        {
            return myHero.getHeroRank();
        }
        public void setHeroRank(int rank)
        {
            myHero.setHeroRank(rank);
        }


        public Color32 getColor(int alpha = 255)
        {
            return new Color32(this.color[0], this.color[1], this.color[2], (byte)alpha);
        }
        public void setColor(byte[] color)
        {
            this.color = color;
        }


        public override string ToString()
        {
            return "Username: " + networkID + ", player tag: " + getHeroType();
        }
    }
}

