using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class SaveGame : MonoBehaviour
{
    public Transform saveGameContainer;

    public InputField saveName;

    public void saveClick()
    {
        Game.gameState.setSaveTime(DateTime.Now);
        SavedGameController.saveGame(Game.gameState, saveName.text);
        Debug.Log("Saved Game!");
        saveGameContainer.gameObject.SetActive(false);

    }
    public void cancelClick()
    {
        saveGameContainer.gameObject.SetActive(false);
    }
}
