using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Start_New_Game : MonoBehaviour
{
 
    private const string SceneName = "GameSetupScreen";

    public void StartNewGame()
    {
        SceneManager.LoadScene(SceneName);

    }

    public void OpenSavedGame()
    {

        SceneManager.LoadScene("Select PubicPrivate");

    }

    public void PlayGame()
    {
        SceneManager.LoadScene("Choose Player");
    }

}
