using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

//This script enables redirection from Sign in Button to Login page in Legends of Andor

public class NewBehaviourScript1 : MonoBehaviour
{
    public void SignIn()
    { 
        SceneManager.LoadScene("Log_In"); 

    }

    public void CreateAccount()
    {

        SceneManager.LoadScene("Create Account");

    }

}
