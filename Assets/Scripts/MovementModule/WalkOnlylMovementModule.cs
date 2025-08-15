using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WalkOnlyMovementModule : MovementModule
{
    public override void Move(PlayerController pc)
    {
        pc.UpdateHorizontalMovement();

        pc.UpdateVerticalMovement();
    }
}
