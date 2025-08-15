using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

[RequireComponent(typeof(PlayerInput))]
public class PlayerController : MonoBehaviour
{
    // �e��ړ�������S���R���|�[�l���g
    private CharacterController2D characterController_;

    // �{�̂�CharacterController
    private CharacterController2D ownerCharacterController_;

    // �񐶏�����S���R���|�[�l���g
    private Parasite parasite_;

    private PlayerInput input_;

    [Header("�������ւ̑��x"),SerializeField]
    private float horizontalSpeed_ = 5.0f;

    [Header("�������x"),SerializeField]
    private float maxFallSpeed_ = 5.0f;

    [Header("�d��"),SerializeField]
    private float gravity_ = 10.0f;

    [Header("�W�����v��"),SerializeField]
    private float jumpPower_ = 5.0f;
    [Header("�W�����v��������"),SerializeField]
    private float jumpDuration_ = 0.5f;

    private float jumpTimer_;

    private Vector2 moveVector_;

    // Start is called before the first frame update
    void Awake()
    {
        ownerCharacterController_ = GetComponent<CharacterController2D>();
        characterController_ = ownerCharacterController_;
        parasite_ = GetComponent<Parasite>();
        input_ = GetComponent<PlayerInput>();
    }

    private void Start()
    {
        parasite_.OnParasite += SwitchingCharacterController;
    }

    private void FixedUpdate()
    {
        // �h�傪����Ȃ�h����ړ�
        // ���Ȃ��Ȃ�{�̂��ړ�
        Vector2 moveVec = moveVector_* Time.deltaTime;
        characterController_.Move(moveVec);
    }

    // Update is called once per frame

    private void Update()
    {
        // �ړ�����
        MovementModule movement = parasite_.GetMovementModule();
        if (movement != null)
        {
            movement.Move(this);
        }
        else
        {
            Debug.Log("MovementModule���ݒ肳��Ă��܂���");
        }
    }

    /// <summary>
    /// ���������̓����݂̂��X�V���郁�\�b�h
    /// </summary>
    public void UpdateHorizontalMovement()
    {
        float inputMove = input_.inputHorizontalMove_;

        moveVector_.x = inputMove * horizontalSpeed_; 
    }

    public void UpdateVerticalMovement()
    {
        // �d�͏���
        moveVector_.y -= gravity_ * Time.deltaTime;

        // �������x����
        if (moveVector_.y < -maxFallSpeed_)
        {
            moveVector_.y = -maxFallSpeed_;
        }
        if(characterController_.isGrounded_)
        {
            moveVector_.y = 0f;
        }
    }

    /// <summary>
    /// �W�����v�̍X�V�����݂̂����郁�\�b�h
    /// </summary>
    public void UpdateJump()
    {
        if(characterController_.isGrounded_)
        {
            jumpTimer_ = 0f;
        }

        // ���������͏�ɃW�����v����悤�Ȏd�g�݂ɂ���
        if(input_.inputJump_)
        {
            if(jumpTimer_ < jumpDuration_)
            {
                moveVector_.y = jumpPower_;
                jumpTimer_ += Time.deltaTime;
            }
        }
        else
        {
            if (!characterController_.isGrounded_)
            {
                // ���n���Ă��Ȃ��̂Ƀ{�^���������ꂽ�Ƃ��A�W�����v�����Ȃ�
                jumpTimer_ = jumpDuration_;
            }
        }
    }

    // �񐶐��CharacterController�𑀍삷�邩��؂�ւ������鏈��
    void SwitchingCharacterController()
    {
        if(parasite_.currentHost_ != null)
        {
            characterController_ = parasite_.hostCharacterController_;
        }
        else
        {
            characterController_ = ownerCharacterController_;
        }
    }
}
