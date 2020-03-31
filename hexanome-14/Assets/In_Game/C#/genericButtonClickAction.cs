using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class genericButtonClickAction : MonoBehaviour
{
    private Command clickCmd;

    // maybe best to just make this class forward the button name to commandRouter
    //
    // maybe buttons just need to have a picture and a name corresponding to the action
    // in commandRouter.
    //
    // and the player.cs class does the click work ie checking what button it was
    // and executing



    public void setClickCmd(Command theClickCommand)
    {
        clickCmd = theClickCommand;
    }

    public bool execute()
    {
        if (!clickCmd.isLegal())
            return false;
        clickCmd.execute();
        return true;
    }
}
