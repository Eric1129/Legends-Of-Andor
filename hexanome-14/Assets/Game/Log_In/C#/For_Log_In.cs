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
        new Graph();
    }


    public void SignIn()
    {
        txt_Input = GameObject.FindGameObjectWithTag("LogIn_username").GetComponent<InputField>();
        storeUsername.USERNAME = txt_Input.text;
        PlayerPrefs.SetString("NickName", txt_Input.text);

        SceneManager.LoadScene("Loading_Screen");
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
