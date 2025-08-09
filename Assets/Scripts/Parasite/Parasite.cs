using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using UnityEngine;

/// <summary>
/// 寄生虫本体のコンポーネント
/// 寄生先の宿主を参照し、対応した各モジュールを取得する
/// 寄生メソッドや寄生解除メソッドから、敵に寄生する仕組みを構築する
/// </summary>
public class Parasite : MonoBehaviour
{
    [Header("宿主"),SerializeField]
    private IParasiteHost currentHost_;

    [SerializeField] private MovementModule currentMovement_ = null;
    [SerializeField] private HealthModule currentHealth_ = null;

    private HorizontalMovementModule defaultMovement_ = new HorizontalMovementModule();
    private OneHealthModule defaultHealth_ = new OneHealthModule();

    public MovementModule GetMovementModule() => currentMovement_;
    public HealthModule GetHealthModule() => currentHealth_;

    private Damageable damageable_;

    private void Awake()
    {
        damageable_ = GetComponent<Damageable>();
    }

    private void Start()
    {
        currentMovement_ = defaultMovement_;
        currentHealth_ = defaultHealth_;
    }

    /// <summary>
    /// 寄生処理
    /// </summary>
    /// <param name="host">新しい寄生先となる相手</param>
    public void Parasitize(IParasiteHost host)
    {
        // 寄生解除処理
        currentHost_.OnReleased();

        currentHost_ = host;

        // 寄生時の処理
        currentHost_.OnParasiteized();

        // 各モジュールの入れ替え
        currentMovement_ = host.GetMovementModule();
        currentHealth_ = host.GetHealthModule();

        // 体力の最大値を設定し全回復
        damageable_.SetMaxHealth(currentHealth_.GetMaxHealth());
        damageable_.FullRecovery();
    }

    /// <summary>
    /// 寄生を解除する
    /// </summary>
    public void Released()
    {
        // 宿主を解放
        currentHost_.OnReleased();

        // 各モジュールの入れ替え
        currentMovement_ = defaultMovement_;
        currentHealth_ = defaultHealth_;

        // 体力の最大値を設定し全回復
        damageable_.SetMaxHealth(currentHealth_.GetMaxHealth());
        damageable_.FullRecovery();
    }
}
