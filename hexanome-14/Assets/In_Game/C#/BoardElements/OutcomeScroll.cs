using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OutcomeScroll : MonoBehaviour
{
    public static OutcomeScroll instance; //singleton



    public Text FeedbackText;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        FeedbackText.text = "";
    }

    public void UpdateFeedback(string messageToPlayers)
    {
        FeedbackText.text = messageToPlayers;
        instance.gameObject.SetActive(true);
    }
}

