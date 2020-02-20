using System.Collections;
using System.Collections.Generic;
using UnityEngine;

interface MoveStrategy
{
    void move(ref Node path, Movable obj);
}
