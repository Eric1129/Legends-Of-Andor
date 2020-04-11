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
        foreach(string path in Directory.GetFiles(Application.persistentDataPath, "*.andor"))
        {
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
        //Debug.Log("Under path: " + path);
        string data = File.ReadAllText(path);
        //Debug.Log(data);
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
}
