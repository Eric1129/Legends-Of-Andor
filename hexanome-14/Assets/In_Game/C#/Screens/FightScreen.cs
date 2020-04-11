using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FightScreen : MonoBehaviour
{
    // private Button[] buttons;

    void Start()
    {
        Sprite[] sprites =  Resources.LoadAll<Sprite>("fight-scene");
    }
}
