using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WalkJumpMovementModule : MovementModule
{
    public override void Move(CharacterController2D characterController)
    {
        float inputX = Input.GetAxis("Horizontal");
        characterController.HorizontalMove(inputX);

        if (Input.GetKeyDown(KeyCode.Space))
        {
            characterController.Jump();
        }
    }
}
