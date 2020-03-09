using System;
using UnityEngine;
using UnityEngine.UI;

public class LoadSprite : MonoBehaviour
{
    Sprite sprite;
    // Use this for initialization
    public void Start()
    {
        sprite = Resources.Load<Sprite>("Images/SampleImage");

        GameObject image = GameObject.Find("Image");
        image.GetComponent<Image>().sprite = sprite;
        
    }

    
}