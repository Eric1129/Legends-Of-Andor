using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

//This script enables redirection from Sign in Button to Login page in Legends of Andor

public class Create_Account : MonoBehaviour
{
    private const string SceneName = "Main_Menu";

    public void CreateAccount()
    {
        SceneManager.LoadScene(SceneName);

    }


}
