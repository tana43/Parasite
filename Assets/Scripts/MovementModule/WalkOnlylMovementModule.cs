using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HorizontalMovementModule : MovementModule
{
    public override void Move(CharacterController2D characterController)
    {
        float inputX = Input.GetAxis("Horizontal");
        characterController.HorizontalMove(inputX);
    }
}
