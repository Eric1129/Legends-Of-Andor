using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Game_Selection : MonoBehaviour
{
    private void Start()
    {
        Game.loadedFromFile = false;
    }
    public void backToMainMenuClick()
    {
        SceneManager.LoadScene("Main_Menu");
    }
}
