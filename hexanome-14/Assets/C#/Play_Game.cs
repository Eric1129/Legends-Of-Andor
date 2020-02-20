using UnityEngine;
using UnityEngine.SceneManagement;

public class Play_Game : MonoBehaviour
{

	public void Quit()
	{
		SceneManager.LoadScene("GameRoom");
	}

}
