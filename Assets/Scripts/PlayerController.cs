using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

[RequireComponent(typeof(PlayerInput))]
public class PlayerController : MonoBehaviour
{
    // 各種移動処理を担うコンポーネント
    private CharacterController2D characterController_;

    // 本体のCharacterController
    private CharacterController2D ownerCharacterController_;

    // 寄生処理を担うコンポーネント
    private Parasite parasite_;

    private PlayerInput input_;

    [Header("横方向への速度"),SerializeField]
    private float horizontalSpeed_ = 5.0f;

    [Header("落下速度"),SerializeField]
    private float maxFallSpeed_ = 5.0f;

    [Header("重力"),SerializeField]
    private float gravity_ = 10.0f;

    [Header("ジャンプ力"),SerializeField]
    private float jumpPower_ = 5.0f;
    [Header("ジャンプ持続時間"),SerializeField]
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
        // 宿主がいるなら宿主を移動
        // いないなら本体を移動
        Vector2 moveVec = moveVector_* Time.deltaTime;
        characterController_.Move(moveVec);
    }

    // Update is called once per frame

    private void Update()
    {
        // 移動処理
        MovementModule movement = parasite_.GetMovementModule();
        if (movement != null)
        {
            movement.Move(this);
        }
        else
        {
            Debug.Log("MovementModuleが設定されていません");
        }
    }

    /// <summary>
    /// 水平方向の動きのみを更新するメソッド
    /// </summary>
    public void UpdateHorizontalMovement()
    {
        float inputMove = input_.inputHorizontalMove_;

        moveVector_.x = inputMove * horizontalSpeed_; 
    }

    public void UpdateVerticalMovement()
    {
        // 重力処理
        moveVector_.y -= gravity_ * Time.deltaTime;

        // 落下速度制限
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
    /// ジャンプの更新処理のみをするメソッド
    /// </summary>
    public void UpdateJump()
    {
        if(characterController_.isGrounded_)
        {
            jumpTimer_ = 0f;
        }

        // 長押し中は常にジャンプするような仕組みにする
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
                // 着地していないのにボタンが離されたとき、ジャンプさせない
                jumpTimer_ = jumpDuration_;
            }
        }
    }

    // 寄生先のCharacterControllerを操作するかを切り替えさせる処理
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
