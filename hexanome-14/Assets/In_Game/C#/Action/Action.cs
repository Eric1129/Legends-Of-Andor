using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;


public enum Type
{
    Move,
    PassTurn,
    EndTurn
}

public interface Action
{
    Type getType();
    string[] playersInvolved();

}


