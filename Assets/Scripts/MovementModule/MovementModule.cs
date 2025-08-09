using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController2D))]
public abstract class MovementModule
{
    public abstract void Move(CharacterController2D characterController);
}
