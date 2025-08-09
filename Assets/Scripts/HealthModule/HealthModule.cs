using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 体力を管理しているモジュール
/// 基底クラスとして扱い、継承先で体力の設定が必ず必要となる
/// </summary>
public abstract class HealthModule
{
    // 体力の初期値、最大値の設定
    public abstract int GetMaxHealth();
};
