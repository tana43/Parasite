using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HorizontalMovementModule : MovementModule
{
    public override void Move(PlayerController pc)
    {
        pc.UpdateHorizontalMovement();

        pc.UpdateVerticalMovement();
    }
}
