using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WormController : EnemyController,IParasiteHost
{
    private WalkJumpMovementModule movement_ = new WalkJumpMovementModule();
    private OneHealthModule health_ = new OneHealthModule();

    public MovementModule GetMovementModule() { return movement_; }
    public HealthModule GetHealthModule() { return health_; }

    // �񐶂��ꂽ�Ƃ��̏���
    public void OnParasiteized()
    {

    }

    // �񐶉����̏���
    public void OnReleased()
    {

    }
}
