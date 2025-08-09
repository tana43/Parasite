using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IParasiteHost
{
    MovementModule GetMovementModule();
    HealthModule GetHealthModule();

    void OnParasiteized(); // �񐶂��ꂽ�Ƃ��̏���
    void OnReleased(); // �񐶉����̏���
}
