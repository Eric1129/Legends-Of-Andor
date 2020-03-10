using UnityEngine.SceneManagement;
using UnityEngine;

public class CreateNewGame : MonoBehaviour
{
    public void CreateGame()
    {
        SceneManager.LoadScene("GameRoom");
    }

    public void Back()
    {
        SceneManager.LoadScene("Main_Menu");
    }
}
