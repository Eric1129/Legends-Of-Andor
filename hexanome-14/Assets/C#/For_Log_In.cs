using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class For_Log_In : MonoBehaviour
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
