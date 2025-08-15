using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public  abstract class ParasiteHost : MonoBehaviour
{
    public abstract MovementModule GetMovementModule();
    public abstract HealthModule GetHealthModule();

    public abstract void OnParasiteized(); // �񐶂��ꂽ�Ƃ��̏���
    public abstract void OnReleased(); // �񐶉����̏���
}
