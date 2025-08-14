using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WormController : MonoBehaviour,IParasiteHost
{
    private WalkJumpMovementModule movement_ = new WalkJumpMovementModule();
    private OneHealthModule health_ = new OneHealthModule();

    public MovementModule GetMovementModule() { return movement_; }
    public HealthModule GetHealthModule() { return health_; }

    // 寄生されたときの処理
    public void OnParasiteized()
    {
        // 多分スプライトの変更とか
    }

    // 寄生解除の処理
    public void OnReleased()
    {

    }
}
