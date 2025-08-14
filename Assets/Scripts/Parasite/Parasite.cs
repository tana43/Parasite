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
    [Header("宿主")]
    public IParasiteHost currentHost_;

    [SerializeField] private MovementModule currentMovement_ = null;
    [SerializeField] private HealthModule currentHealth_ = null;

    private WalkJumpMovementModule defaultMovement_ = new WalkJumpMovementModule();
    private OneHealthModule defaultHealth_ = new OneHealthModule();

    public MovementModule GetMovementModule() => currentMovement_;
    public HealthModule GetHealthModule() => currentHealth_;

    [Header("寄生時に機能を止めるコンポーネント"), SerializeField]
    private Behaviour[] disableComponents_;

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
        if(currentHost_ != null)
        {
            currentHost_.OnReleased();

            // 寄生後は邪魔になるコンポーネントを停止
            for(int i = 0; i < disableComponents_.Length; i++)
            {
                disableComponents_[i].enabled = false;
            }
        }

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
        currentHost_ = null;

        // 各モジュールの入れ替え
        currentMovement_ = defaultMovement_;
        currentHealth_ = defaultHealth_;

        // 体力の最大値を設定し全回復
        damageable_.SetMaxHealth(currentHealth_.GetMaxHealth());
        damageable_.FullRecovery();

        // 寄生後に停止したコンポーネントを起動
        for (int i = 0; i < disableComponents_.Length; i++)
        {
            disableComponents_[i].enabled = false;
        }
    }
}
