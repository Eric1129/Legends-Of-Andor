using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
public class DistributeArticleController : MonoBehaviour
{
	public Transform distributeScreen;
	public Transform waitingScreen;
	public bool gameStart = false;
    // Start is called before the first frame update
    void Start()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            distributeScreen.gameObject.SetActive(true);
        }
        else
        {
            waitingScreen.gameObject.SetActive(true);
        }
    }

    // Update is called once per frame
    void Update()
    {
		
       
        
    }
}
