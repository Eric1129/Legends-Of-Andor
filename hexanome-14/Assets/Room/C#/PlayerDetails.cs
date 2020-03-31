using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerDetails : MonoBehaviour
{

    public Text nameLabel;
    public Text heroLabel;
    public Button readyButton;

    private ColorBlock colorNotReady;
    private ColorBlock colorReady;

    private bool ready = false;

    private void Start()
    {
        colorNotReady = new ColorBlock();
        colorNotReady.normalColor = new Color(255, 95, 95);
        colorNotReady.highlightedColor = new Color(253, 142, 142);
        colorNotReady.pressedColor = new Color(255, 182, 182);
        colorNotReady.selectedColor = new Color(255, 206, 206);
        colorNotReady.disabledColor = new Color(255, 95, 95);

        colorReady = new ColorBlock();
        colorReady.normalColor = new Color(95, 255, 95);
        colorReady.highlightedColor = new Color(142, 253, 142);
        colorReady.pressedColor = new Color(182, 255, 182);
        colorReady.selectedColor = new Color(206, 255, 206);
        colorReady.disabledColor = new Color(95, 255, 95);


    }

    public void setReady(bool r)
    {
        this.ready = r;
        if (Game.myPlayer.ready)
        {
            readyButton.colors = colorReady;
        }
        else
        {
            readyButton.colors = colorNotReady;
        }
    }


    public void readyClick()
    {
        this.ready = !this.ready;
        Game.myPlayer.ready = this.ready;
        Game.updatePlayer(Game.myPlayer);

        if (this.ready)
        {
            readyButton.colors = colorReady;
        }
        else
        {
            readyButton.colors = colorNotReady;
        }
    }
}
