using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI; 


public class TimerToGameboard : MonoBehaviour
{

    private float timer = 5f;
    //private TextAlignment timerSeconds;

    // Start is called before the first frame update
    void Start()
    {

        //timerSeconds = GetComponent<TextAlignment>();
        
    }

    // Update is called once per frame
    void Update()
    {
        timer -= Time.deltaTime;
        //timerSeconds.text = timer.ToString("f0");
        if (timer <= 0){

            SceneManager.LoadScene("UnityMadeMeSaveToFile");
        }
        
    }
}
