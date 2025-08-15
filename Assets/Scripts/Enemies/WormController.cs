using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WormController : ParasiteHost
{
    private WalkJumpMovementModule movement_ = new WalkJumpMovementModule();
    private OneHealthModule health_ = new OneHealthModule();

    [SerializeField]
    private Sprite parasiteWorm_;

    private SpriteRenderer spriteRenderer_;

    public override MovementModule GetMovementModule() { return movement_; }
    public override HealthModule GetHealthModule() { return health_; }

    private void Awake()
    {
        spriteRenderer_ = GetComponent<SpriteRenderer>();
    }

    // 寄生されたときの処理
    public override void OnParasiteized()
    {
        // 多分スプライトの変更とか
        spriteRenderer_.sprite = parasiteWorm_;
    }

    // 寄生解除の処理
    public override void OnReleased()
    {

    }
}
