using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class For_Log_In : MonoBehaviour
{
    
    InputField txt_Input;

    void Start()
    {
        if (PlayerPrefs.HasKey("NickName"))
        {
            txt_Input = GameObject.FindGameObjectWithTag("LogIn_username").GetComponent<InputField>();
            txt_Input.text = PlayerPrefs.GetString("NickName");
        }
    }


    public void SignIn()
    {
        txt_Input = GameObject.FindGameObjectWithTag("LogIn_username").GetComponent<InputField>();
        SceneManager.LoadScene("Loading_Screen");
        storeUsername.USERNAME = txt_Input.text;

        Debug.Log(txt_Input.text + " Logged in!");
    }

    public void Create_Account()
    {

        SceneManager.LoadScene("Create_Account");

    }

}

public static class storeUsername
{
    public static string USERNAME { get; set; }
}
