using UnityEngine.SceneManagement;
using UnityEngine;

public class CreateNewGame : MonoBehaviour
{
    public void GameLobby()
    {
        SceneManager.LoadScene("GameRoom");
    }
}
