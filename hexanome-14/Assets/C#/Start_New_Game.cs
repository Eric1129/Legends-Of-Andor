using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Start_New_Game : MonoBehaviour
{
 
    public void StartNewGame()
    {
        SceneManager.LoadScene("CreateNewGame");

    }

    public void OpenSavedGame()
    {

        SceneManager.LoadScene("Select PubicPrivate");

    }

    public void PlayGame()
    {
        SceneManager.LoadScene("Game_Lobby");
    }

}
