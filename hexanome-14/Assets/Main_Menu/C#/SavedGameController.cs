using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class SavedGameController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }


    public static void saveGame(GameState gs, string saveName)
    {
        string path = Application.persistentDataPath + saveName + ".andor";

        using (StreamWriter file = File.CreateText(path))
        {
            //JsonSerializer serializer = new JsonSerializer();
            //serialize object directly into file stream
            //serializer.Serialize(file, _data);
        }
    }


    public static bool gameWithSameName(string saveName)
    {
        if(File.Exists(Application.persistentDataPath + saveName + ".andor"))
        {
            return true;
        }
        return false;
    }

    public static void loadGame(string saveName)
    {

    }

    private string JSONGameState(GameState gs, int ind = 0)
    {
        string json = "";
        
        json += JSONinsertObject(new string[] { "Legend", "Player-Locations" },
            new string[] { ((int)gs.legend).ToString(), JSONDictionary(gs.playerLocations, ind + 1, true)},
            ind);

        json += indent(ind) + "Hero: {\n";
        //json += JSONHero(p.getHero(), ind + 1);
        json += indent(ind) + "},\n";

        //json += indent(ind) + "NetworkID" + ": " + p.getNetworkID() + ",\n";
        //json += indent(ind) + "Color" + ": " + p.color + "\n";


        return json;
    }

    private string JSONDictionary<T, R>(Dictionary<T, R> dict, int ind = 0, bool bracket = true)
    {
        string json = indent(ind) + "{\n";


        foreach (KeyValuePair<T, R> pair in dict)
        {
            json += indent(ind + 1) + pair.Key.ToString() + ": " + pair.Value.ToString() + ",\n";
        }

        json = json.Substring(0, json.Length - 3) + "\n";
        json += indent(ind) + "}\n";

        return json;
    }

    private string JSONPlayer(Andor.Player p, int ind = 0)
    {
        string json = "";
        json += indent(ind) + "Hero: {\n";
        json += JSONHero(p.getHero(), ind + 1);
        json += indent(ind) + "},\n";

        json += indent(ind) + "NetworkID" + ": " + p.getNetworkID() + ",\n";
        json += indent(ind) + "Color" + ": " + p.color + "\n";



        return json;
    }

    private string JSONHero(HeroS h, int ind = 0)
    {
        string json = JSONinsertObject(new string[] {"HeroType", "Gold" },
            new string[] {h.getHeroType(), h.getGold().ToString() },
            ind);

        return json;
    }

    private string JSONinsertObject(string[] key, string[] value, int ind = 0, bool end = true)
    {
        string json = "";
        for(int i = 0; i<key.Length; i++)
        {
            json += indent(ind) + key[i] + ": " + value[i] + ",\n";
        }

        if (end)
        {
            return json.Substring(0, json.Length - 3) + "\n";
        }

        return json;
    }

    private string indent(int indent)
    {
        string ret = "";
        for(int i = 0; i<indent; i++)
        {
            ret += "\t";
        }
        return ret;
    }
}
