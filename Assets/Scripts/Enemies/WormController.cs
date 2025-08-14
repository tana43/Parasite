using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WormController : MonoBehaviour,IParasiteHost
{
    private WalkJumpMovementModule movement_ = new WalkJumpMovementModule();
    private OneHealthModule health_ = new OneHealthModule();

    public MovementModule GetMovementModule() { return movement_; }
    public HealthModule GetHealthModule() { return health_; }

    // �񐶂��ꂽ�Ƃ��̏���
    public void OnParasiteized()
    {
        // �����X�v���C�g�̕ύX�Ƃ�
    }

    // �񐶉����̏���
    public void OnReleased()
    {

    }
}
