using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class For_Log_In : MonoBehaviour
{

    public void SignIn()
    {
        SceneManager.LoadScene("Main_Menu");

    }

    public void Create_Account()
    {

        SceneManager.LoadScene("Create_Account");

    }

}
