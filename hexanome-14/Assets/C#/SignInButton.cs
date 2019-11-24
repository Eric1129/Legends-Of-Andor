using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

//This script enables redirection from Sign in Button to Login page in Legends of Andor

public class SignInButton : MonoBehaviour
{
    private const string SceneName = "Main_Menu";

    public void SignIn()
    {
        SceneManager.LoadScene(SceneName);

    }

    public void Create_Account()
    {

        SceneManager.LoadScene("Create_Account");

    }

}
