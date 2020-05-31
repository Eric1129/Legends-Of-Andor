using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class HeroInfoController : MonoBehaviour
{
    public Transform HeroInfoPanel;
    public Text playerText;

    public static HeroInfoController instance;
    private void Start()
    {
        instance = this;
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            HeroInfoPanel.gameObject.SetActive(false);
        }
    }

    public void showHero(Andor.Player player)
    {
        playerText.text = player.getNetworkID();
        HeroInfoPanel.gameObject.SetActive(true);
    }
}
