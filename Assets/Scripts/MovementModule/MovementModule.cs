using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerController))]
public abstract class MovementModule
{
    public abstract void Move(PlayerController pc);
}
