using System;
using System.Collections;

using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// 寄生虫本体のコンポーネント
/// 寄生先の宿主を参照し、対応した各モジュールを取得する
/// 寄生メソッドや寄生解除メソッドから、敵に寄生する仕組みを構築する
/// </summary>
[RequireComponent(typeof(Damageable))]
public class Parasite : MonoBehaviour
{
    [Header("宿主")]
    public ParasiteHost currentHost_;

    [SerializeField] private MovementModule currentMovement_ = null;
    [SerializeField] private HealthModule currentHealth_ = null;

    private WalkOnlyMovementModule defaultMovement_ = new WalkOnlyMovementModule();
    private OneHealthModule defaultHealth_ = new OneHealthModule();

    public MovementModule GetMovementModule() => currentMovement_;
    public HealthModule GetHealthModule() => currentHealth_;

    [Header("寄生時に機能を止めるコンポーネント"), SerializeField]
    private Behaviour[] disableComponents_;

    private Damageable damageable_;
    private Collider2D collider_;
    public CharacterController2D hostCharacterController_ { get; private set; }
    public  SpriteRenderer hostSpriteRenderer_ { get; private set; }

    // 寄生したときに呼び出される処理
    public event Action OnParasite;
    // 寄生解除したときに呼び出される処理
    public event Action OnReleased;

    private void Awake()
    {
        damageable_ = GetComponent<Damageable>();
        hostSpriteRenderer_ = GetComponent<SpriteRenderer>();
        collider_ = GetComponent<Collider2D>();
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
    public void Parasitize(ref ParasiteHost host)
    {
        // 寄生解除処理
        if(currentHost_ != null)
        {
            currentHost_.OnReleased();
        }

        // 寄生後は邪魔になるコンポーネントを停止
        for (int i = 0; i < disableComponents_.Length; i++)
        {
            disableComponents_[i].enabled = false;
        }
        hostSpriteRenderer_.enabled = false;
        collider_.enabled = false;


        currentHost_ = host;
        hostCharacterController_ = host.GetComponent<CharacterController2D>();

        // 寄生時の処理
        currentHost_.OnParasiteized();

        // エネミーコントローラーを停止させて、自動で動かないように
        currentHost_.GetComponent<EnemyController>().enabled = false;

        // 各モジュールの入れ替え
        currentMovement_ = host.GetMovementModule();
        currentHealth_ = host.GetHealthModule();

        // 体力の最大値を設定し全回復
        damageable_.SetMaxHealth(currentHealth_.GetMaxHealth());
        damageable_.FullRecovery();

        OnParasite.Invoke();
    }

    /// <summary>
    /// 寄生を解除する
    /// </summary>
    public void Released()
    {
        // 宿主を解放
        currentHost_.OnReleased();
        currentHost_ = null;
        hostCharacterController_ = null;

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
        hostSpriteRenderer_.enabled = false;

        OnReleased.Invoke();
    }
}
