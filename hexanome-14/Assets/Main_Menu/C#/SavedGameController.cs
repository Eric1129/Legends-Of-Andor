using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using Newtonsoft.Json;
using UnityEngine.SceneManagement;

public class SavedGameController : MonoBehaviour
{
    public Transform gameContainer;

    public GameObject gameDetailsPrefab;
    public Button privateButton;
    public Button publicButton;

    public static bool isPrivate = false;

    // Start is called before the first frame update
    void Start()
    {
        Game.loadedFromFile = true;
        publicButton.interactable = false;
        privateButton.interactable = true;
        isPrivate = false;
        getGames();
    }

    public void backToMainMenuClick()
    {
        Game.loadedFromFile = false;
        SceneManager.LoadScene("Main_Menu");
    }

    public void privateClick()
    {
        isPrivate = true;
        publicButton.interactable = true;
        privateButton.interactable = false;
    }
    public void publicClick()
    {
        isPrivate = false;
        publicButton.interactable = false;
        privateButton.interactable = true;
    }


    public void getGames()
    {
        Debug.Log("Looking for Games...");
        Debug.Log(Directory.GetFiles(Application.persistentDataPath).Length);
        foreach(string path in Directory.GetFiles(Application.persistentDataPath))
        {
            Debug.Log(path);

            listGame(new FileInfo(path));
        }
    }

    public void listGame(FileInfo file)
    {
        GameState gameState = SavedGameController.loadGame(file.FullName);
        GameObject listing = Instantiate(gameDetailsPrefab, gameContainer);
        SaveGameDetails details = listing.GetComponent<SaveGameDetails>();
        details.init(file.Name.Substring(0, file.Name.Length-6), gameState);

    }

    public static void saveGame(GameState gs, string saveName)
    {
        Debug.Log("Saving Game...");
        string path = Application.persistentDataPath + "/" + saveName + ".andor";
        Debug.Log("Under path: " + path);


        //Debug.Log(JSONGameState(gs));

        string saveData = SavedGameController.serializeGameState(gs);
        Debug.Log(saveData);


        File.WriteAllText(path, saveData);
        /*using (StreamWriter file = File.CreateText(path))
        {
            //JsonSerializer serializer = new JsonSerializer();
            //serialize object directly into file stream
            //serializer.Serialize(file, _data);
        }*/
    }


    public static bool gameWithSameName(string saveName)
    {
        if(File.Exists(Application.persistentDataPath + saveName + ".andor"))
        {
            return true;
        }
        return false;
    }

    public static GameState loadGame(string path)
    {
        Debug.Log("Loading Game...");
        Debug.Log("Under path: " + path);
        string data = File.ReadAllText(path);
        Debug.Log(data);
        return JsonConvert.DeserializeObject<GameState>(data);
    }


    public static string serializeGameState(GameState gameState)
    {
        return JsonConvert.SerializeObject(gameState, Formatting.Indented);
    }
    public static GameState deserializeGameState(string data)
    {
        return JsonConvert.DeserializeObject<GameState>(data);
    }

    #region encoding

    private static string JSONGameState(GameState gs, int ind = 0)
    {
        string json = indent(ind) + "Game: {\n";
        
        json += JSONinsertObject(new string[] { "Legend", "Player-Locations" },
            new string[] { ((int)gs.legend).ToString(), JSONDictionary(gs.playerLocations, ind + 1, false)},
            ind+1);

        json += indent(ind+1) + "Players: [\n";
        foreach (Andor.Player player in gs.getPlayers())
        {
            json += indent(ind+2) + player.getNetworkID() + ": {\n";
            json += JSONPlayer(player, ind + 3);
            json += indent(ind+2) + "},\n";

        }
        json = json.Substring(0, json.Length - 2) + "\n";

        json += indent(ind+1) + "]\n";

        // Will probably have to add turn manager

        json += indent(ind) + "}\n";
        return json;
    }

    private static string JSONDictionary<T, R>(Dictionary<T, R> dict, int ind = 0, bool end = true)
    {
        string json = indent(ind) + "{\n";


        foreach (KeyValuePair<T, R> pair in dict)
        {
            json += indent(ind + 1) + pair.Key.ToString() + ": " + pair.Value.ToString() + ",\n";
        }

        json = json.Substring(0, json.Length - 2) + "\n";

        if (end)
        {
            json += indent(ind) + "}\n";
        }
        else
        {
            json += indent(ind) + "},\n";
        }

        return json;
    }

    private static string JSONPlayer(Andor.Player p, int ind = 0)
    {
        string json = "";
        json += indent(ind) + "Hero: {\n";
        json += JSONHero(p.getHero(), ind + 1);
        json += indent(ind) + "},\n";

        json += indent(ind) + "NetworkID" + ": " + p.getNetworkID() + ",\n";
        json += indent(ind) + "Color" + ": [" + p.color[0] + ", " + p.color[1] + ", " + p.color[2] + ", " + p.color[3] + "]\n";



        return json;
    }

    private static string JSONHero(HeroS h, int ind = 0)
    {
        string json = JSONinsertObject(new string[] {"HeroType", "Gold" },
            new string[] {h.getHeroType(), h.getGold().ToString() },
            ind);

        return json;
    }

    private static string JSONinsertObject(string[] key, string[] value, int ind = 0, bool end = true)
    {
        string json = "";
        for(int i = 0; i<key.Length; i++)
        {
            json += indent(ind) + key[i] + ": " + value[i] + ",\n";
        }

        if (end)
        {
            return json.Substring(0, json.Length - 2) + "\n";
        }

        return json;
    }

    private static string indent(int indent)
    {
        string ret = "";
        for(int i = 0; i<indent; i++)
        {
            ret += "\t";
        }
        return ret;
    }

    #endregion
}
