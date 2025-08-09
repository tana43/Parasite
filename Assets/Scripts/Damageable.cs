using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public partial class Damageable : MonoBehaviour
{
    [Header("�̗�"),SerializeField]
    private int health_;

    [Header("�̗͂̍ő�l"),SerializeField]
    private int maxHealth_;

    public void SetMaxHealth(int maxHealth) { maxHealth_ = maxHealth; }

    [Header("��_����̖��G����"),SerializeField]
    private float invulnerabiltyTime_;

    // ���G��Ԃ�
    private bool isInvulnerable_ { get; set; }

    // ��_����̌o�ߎ���
    protected float timeSinceLastHit_ = 0.0f;

    // �_���[�W���󂯂��Ƃ��̏󋵂ɉ����ČĂ΂�鏈��
    public UnityEvent OnDeath;                  // ���񂾂Ƃ�
    public UnityEvent OnAlreadyDead;            // ���Ɏ���ł���Ƃ�
    public UnityEvent OnReceiveDamage;          // �_���[�W���󂯂��Ƃ�
    public UnityEvent OnHitWhileInvulnerable;   // ���G��ԂɃ_���[�W���󂯂��Ƃ�

    // ���G���Ԃ��������ꂽ�Ƃ��ɌĂ΂�鏈��
    public UnityEvent OnInvincibleCanceled;     

    private void Update()
    {
        // ���G���Ԃ̍X�V
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
    /// �_���[�W���󂯂�
    /// ��_�����̏󋵂ɉ����Ċe�C�x���g���Ăяo��
    /// </summary>
    /// <param name="dm">�󂯂��U���̏��</param>
    public void ApplyDamage(DamageMessage dm)
    {
        if(health_ <= 0)
        {
            // ���Ɏ���ł���
            OnAlreadyDead.Invoke();
            return;
        }

        if(isInvulnerable_)
        {
            // ���G���
            OnHitWhileInvulnerable.Invoke();
            return;
        }

        // ��_���[�W����
        float health = health_ - dm.amount_;
        isInvulnerable_ = true;

        if(health <= 0)
        {
            // ����̍U���Ŏ���
            OnDeath.Invoke();
            return;
        }

        OnReceiveDamage.Invoke();
        return;
    }

    /// <summary>
    /// �S��
    /// </summary>
    public void FullRecovery()
    {
        health_ = maxHealth_;
    }
}
