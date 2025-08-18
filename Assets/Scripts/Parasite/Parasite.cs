using System;
using System.Collections;

using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// �񐶒��{�̂̃R���|�[�l���g
/// �񐶐�̏h����Q�Ƃ��A�Ή������e���W���[�����擾����
/// �񐶃��\�b�h��񐶉������\�b�h����A�G�Ɋ񐶂���d�g�݂��\�z����
/// </summary>
[RequireComponent(typeof(Damageable))]
public class Parasite : MonoBehaviour
{
    [Header("�h��")]
    public ParasiteHost currentHost_;

    [SerializeField] private MovementModule currentMovement_ = null;
    [SerializeField] private HealthModule currentHealth_ = null;

    private WalkOnlyMovementModule defaultMovement_ = new WalkOnlyMovementModule();
    private OneHealthModule defaultHealth_ = new OneHealthModule();

    public MovementModule GetMovementModule() => currentMovement_;
    public HealthModule GetHealthModule() => currentHealth_;

    [Header("�񐶎��ɋ@�\���~�߂�R���|�[�l���g"), SerializeField]
    private Behaviour[] disableComponents_;

    private Damageable damageable_;
    private Collider2D collider_;
    public CharacterController2D hostCharacterController_ { get; private set; }
    public  SpriteRenderer hostSpriteRenderer_ { get; private set; }

    // �񐶂����Ƃ��ɌĂяo����鏈��
    public event Action OnParasite;
    // �񐶉��������Ƃ��ɌĂяo����鏈��
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
    /// �񐶏���
    /// </summary>
    /// <param name="host">�V�����񐶐�ƂȂ鑊��</param>
    public void Parasitize(ref ParasiteHost host)
    {
        // �񐶉�������
        if(currentHost_ != null)
        {
            currentHost_.OnReleased();
        }

        // �񐶌�͎ז��ɂȂ�R���|�[�l���g���~
        for (int i = 0; i < disableComponents_.Length; i++)
        {
            disableComponents_[i].enabled = false;
        }
        hostSpriteRenderer_.enabled = false;
        collider_.enabled = false;


        currentHost_ = host;
        hostCharacterController_ = host.GetComponent<CharacterController2D>();

        // �񐶎��̏���
        currentHost_.OnParasiteized();

        // �G�l�~�[�R���g���[���[���~�����āA�����œ����Ȃ��悤��
        currentHost_.GetComponent<EnemyController>().enabled = false;

        // �e���W���[���̓���ւ�
        currentMovement_ = host.GetMovementModule();
        currentHealth_ = host.GetHealthModule();

        // �̗͂̍ő�l��ݒ肵�S��
        damageable_.SetMaxHealth(currentHealth_.GetMaxHealth());
        damageable_.FullRecovery();

        OnParasite.Invoke();
    }

    /// <summary>
    /// �񐶂���������
    /// </summary>
    public void Released()
    {
        // �h������
        currentHost_.OnReleased();
        currentHost_ = null;
        hostCharacterController_ = null;

        // �e���W���[���̓���ւ�
        currentMovement_ = defaultMovement_;
        currentHealth_ = defaultHealth_;

        // �̗͂̍ő�l��ݒ肵�S��
        damageable_.SetMaxHealth(currentHealth_.GetMaxHealth());
        damageable_.FullRecovery();

        // �񐶌�ɒ�~�����R���|�[�l���g���N��
        for (int i = 0; i < disableComponents_.Length; i++)
        {
            disableComponents_[i].enabled = false;
        }
        hostSpriteRenderer_.enabled = false;

        OnReleased.Invoke();
    }
}
