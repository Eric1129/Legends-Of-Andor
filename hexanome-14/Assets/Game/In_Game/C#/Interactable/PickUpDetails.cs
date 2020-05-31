using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PickUpDetails : MonoBehaviour
{
    public static Color32 NON_SELECTED_COLOR = new Color32(0,255,255,255);
    public static Color32 SELECTED_COLOR = new Color32(0, 255, 0, 255);

    public Text label;

    public bool Selected = false;

    public void clickPickUp()
    {
        // Remove selection from all
        for(int i = 0; i<transform.parent.childCount; i++)
        {
            transform.parent.GetChild(i).GetComponent<PickUpDetails>().removeSelection();
        }

        Selected = true;
        gameObject.GetComponent<Image>().color = SELECTED_COLOR;

    }
    public void removeSelection()
    {
        gameObject.GetComponent<Image>().color = NON_SELECTED_COLOR;
        Selected = false;
    }
}
