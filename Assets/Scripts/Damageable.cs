using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public partial class Damageable : MonoBehaviour
{
    [Header("体力"),SerializeField]
    private int health_;

    [Header("体力の最大値"),SerializeField]
    private int maxHealth_;

    public void SetMaxHealth(int maxHealth) { maxHealth_ = maxHealth; }

    [Header("被ダメ後の無敵時間"),SerializeField]
    private float invulnerabiltyTime_;

    // 無敵状態か
    private bool isInvulnerable_ { get; set; }

    // 被ダメ後の経過時間
    protected float timeSinceLastHit_ = 0.0f;

    // ダメージを受けたときの状況に応じて呼ばれる処理
    public UnityEvent OnDeath;                  // 死んだとき
    public UnityEvent OnAlreadyDead;            // 既に死んでいるとき
    public UnityEvent OnReceiveDamage;          // ダメージを受けたとき
    public UnityEvent OnHitWhileInvulnerable;   // 無敵状態にダメージを受けたとき

    // 無敵時間が解除されたときに呼ばれる処理
    public UnityEvent OnInvincibleCanceled;     

    private void Update()
    {
        // 無敵時間の更新
        if (isInvulnerable_)
        {
            timeSinceLastHit_ += Time.deltaTime;
            if(timeSinceLastHit_ > invulnerabiltyTime_)
            {
                isInvulnerable_ = false;
                timeSinceLastHit_ = 0.0f;
                OnInvincibleCanceled.Invoke();
            }
        }
    }

    /// <summary>
    /// ダメージを受ける
    /// 被ダメ時の状況に応じて各イベントを呼び出す
    /// </summary>
    /// <param name="dm">受けた攻撃の情報</param>
    public void ApplyDamage(DamageMessage dm)
    {
        if(health_ <= 0)
        {
            // 既に死んでいる
            OnAlreadyDead.Invoke();
            return;
        }

        if(isInvulnerable_)
        {
            // 無敵状態
            OnHitWhileInvulnerable.Invoke();
            return;
        }

        // 被ダメージ処理
        float health = health_ - dm.amount_;
        isInvulnerable_ = true;

        if(health <= 0)
        {
            // 今回の攻撃で死んだ
            OnDeath.Invoke();
            return;
        }

        OnReceiveDamage.Invoke();
        return;
    }

    /// <summary>
    /// 全回復
    /// </summary>
    public void FullRecovery()
    {
        health_ = maxHealth_;
    }
}
