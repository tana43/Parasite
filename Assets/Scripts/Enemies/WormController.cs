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

    // �񐶂��ꂽ�Ƃ��̏���
    public override void OnParasiteized()
    {
        // �����X�v���C�g�̕ύX�Ƃ�
        spriteRenderer_.sprite = parasiteWorm_;
    }

    // �񐶉����̏���
    public override void OnReleased()
    {

    }
}
