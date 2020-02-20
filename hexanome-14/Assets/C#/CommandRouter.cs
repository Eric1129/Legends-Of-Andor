using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommandRouter : MonoBehaviour
{
    // purpose of this class is that every time a button like: diceRollButton
    // gets clicked, they just inform the master class of their button name
    // if this button name is the key to a command, then when master Class sees this request
    // it just calls this.cmdRouter.execute(name);


    private Dictionary<string, Command> commands;

    // we'll need a file with a big switch statement to create these commands
    // maybe some kind of factory pattern?

    public void addCommand(string cmdName, Command cmd)
    {
        if (commands == null)
        {
            commands = new Dictionary<string, Command>();
        }
        commands[cmdName] = cmd;
    }

    public void execute(string cmdName)
    {
        Command cmd = commands[cmdName];


        if (cmd == null)
        {
            Debug.Log("Given cmdName that is not a valid key in commands dictionary: " + cmdName);
            throw new ArgumentException("Given cmdName that is not a valid key in commands dictionary: " + cmdName);
        }
        if (!cmd.isLegal())
        {
            // if cmd is not legal thats fine ish
            // but we should only have buttons highlighted if they're legal anyways.
            // just need to call isLegal() on every button per screen

            Debug.Log("given cmdName that is not legal: " + cmdName);
            return;
        }

        cmd.execute();
    }
}
