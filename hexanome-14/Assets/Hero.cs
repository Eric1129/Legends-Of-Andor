using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hero : MonoBehaviour
{
    private GameObject sphere;
    private SpriteRenderer spriteRenderer;
    private string hero_type;

    // called AFTER gameObject for this script has been created
    // AND the necessary components have been added.
    public void init(GameObject theSphere, string hero_name)
    {
        hero_type = hero_name;
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        sphere = theSphere;
    }

}
