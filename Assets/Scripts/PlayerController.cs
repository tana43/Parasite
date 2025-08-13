using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

[RequireComponent(typeof(PlayerInput))]
public class PlayerController : MonoBehaviour
{
    // �e��ړ�����
    private CharacterController2D characterController_;

    // �񐶏����̒S���R���|�[�l���g
    private Parasite parasite_;

    private PlayerInput input_;

    [Header("�������ւ̑��x")]
    private float horizontalSpeed_ = 5.0f;

    [Header("�������x"),SerializeField]
    private float maxFallSpeed_ = 5.0f;

    [Header("�d��"),SerializeField]
    private float gravity_ = 10.0f;

    [Header("�W�����v��"),SerializeField]
    private float jumpPower_ = 5.0f;

    private Vector2 moveVector_;

    // Start is called before the first frame update
    void Awake()
    {
        characterController_ = GetComponent<CharacterController2D>();
        parasite_ = GetComponent<Parasite>();
        input_ = GetComponent<PlayerInput>();
    }

    private void FixedUpdate()
    {
        characterController_.Move(moveVector_ * Time.deltaTime);
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
        moveVector_.y -= gravity_;

        // �������x����
        if (moveVector_.y < maxFallSpeed_)
        {
            moveVector_.y = -maxFallSpeed_;
        }
    }

    /// <summary>
    /// �W�����v�̍X�V�����݂̂����郁�\�b�h
    /// </summary>
    public void UpdateJump()
    {
        if(input_.inputJump_)
        {
            moveVector_.y += jumpPower_;
        }
    }
   
}
