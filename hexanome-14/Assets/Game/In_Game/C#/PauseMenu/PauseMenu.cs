using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public static bool mouseOverMenu = false;
    public Transform saveGameContainer;


    private void Start()
    {
        //gameObject.AddComponent<PolygonCollider2D>();
    }

    void OnMouseDown()
    {
        Debug.Log("DOWN ~ " + gameObject.name);
        if (!mouseOverMenu)
        {
            GameController.instance.removePauseMenu();
        }
    }
    void OnMouseOver()
    {
        Debug.Log("OVER ~ " + gameObject.name);
        if (gameObject.name.Equals("PauseContainer"))
        {
            if (!mouseOverMenu)
            {
                mouseOverMenu = true;
            }
        }

        print(gameObject.name);
    }

    void OnMouseExit()
    {
        Debug.Log("EXIT ~ " + gameObject.name);
        if (gameObject.name.Equals("PauseContainer"))
        {
            if (mouseOverMenu)
            {
                mouseOverMenu = false;
            }
        }
    }
    public void saveGameClick()
    {
        saveGameContainer.gameObject.SetActive(true);
    }

    public void exitGameClick()
    {
        PhotonNetwork.LeaveRoom();
        SceneManager.LoadScene("Main_Menu");
    }
}
