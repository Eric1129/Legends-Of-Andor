using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

//This script enables redirection from Sign in Button to Login page in Legends of Andor

public class NewBehaviourScript3 : MonoBehaviour
{
    public void StartNewGame()
    {
        SceneManager.LoadScene("Choose_Player");

    }

    public void OpenSavedGame()
    {

        SceneManager.LoadScene("");

    }
    public void PlayGame()
    {



    }

}
