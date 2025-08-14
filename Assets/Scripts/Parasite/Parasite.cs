using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using UnityEngine;

/// <summary>
/// �񐶒��{�̂̃R���|�[�l���g
/// �񐶐�̏h����Q�Ƃ��A�Ή������e���W���[�����擾����
/// �񐶃��\�b�h��񐶉������\�b�h����A�G�Ɋ񐶂���d�g�݂��\�z����
/// </summary>
public class Parasite : MonoBehaviour
{
    [Header("�h��")]
    public IParasiteHost currentHost_;

    [SerializeField] private MovementModule currentMovement_ = null;
    [SerializeField] private HealthModule currentHealth_ = null;

    private WalkJumpMovementModule defaultMovement_ = new WalkJumpMovementModule();
    private OneHealthModule defaultHealth_ = new OneHealthModule();

    public MovementModule GetMovementModule() => currentMovement_;
    public HealthModule GetHealthModule() => currentHealth_;

    [Header("�񐶎��ɋ@�\���~�߂�R���|�[�l���g"), SerializeField]
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
    /// �񐶏���
    /// </summary>
    /// <param name="host">�V�����񐶐�ƂȂ鑊��</param>
    public void Parasitize(IParasiteHost host)
    {
        // �񐶉�������
        if(currentHost_ != null)
        {
            currentHost_.OnReleased();

            // �񐶌�͎ז��ɂȂ�R���|�[�l���g���~
            for(int i = 0; i < disableComponents_.Length; i++)
            {
                disableComponents_[i].enabled = false;
            }
        }

        currentHost_ = host;

        // �񐶎��̏���
        currentHost_.OnParasiteized();

        // �e���W���[���̓���ւ�
        currentMovement_ = host.GetMovementModule();
        currentHealth_ = host.GetHealthModule();

        // �̗͂̍ő�l��ݒ肵�S��
        damageable_.SetMaxHealth(currentHealth_.GetMaxHealth());
        damageable_.FullRecovery();
    }

    /// <summary>
    /// �񐶂���������
    /// </summary>
    public void Released()
    {
        // �h������
        currentHost_.OnReleased();
        currentHost_ = null;

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
    }
}
