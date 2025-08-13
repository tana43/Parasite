using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

[RequireComponent(typeof(PlayerInput))]
public class PlayerController : MonoBehaviour
{
    // 各種移動処理
    private CharacterController2D characterController_;

    // 寄生処理の担うコンポーネント
    private Parasite parasite_;

    private PlayerInput input_;

    [Header("横方向への速度")]
    private float horizontalSpeed_ = 5.0f;

    [Header("落下速度"),SerializeField]
    private float maxFallSpeed_ = 5.0f;

    [Header("重力"),SerializeField]
    private float gravity_ = 10.0f;

    [Header("ジャンプ力"),SerializeField]
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
        moveVector_.y -= gravity_;

        // 落下速度制限
        if (moveVector_.y < maxFallSpeed_)
        {
            moveVector_.y = -maxFallSpeed_;
        }
    }

    /// <summary>
    /// ジャンプの更新処理のみをするメソッド
    /// </summary>
    public void UpdateJump()
    {
        if(input_.inputJump_)
        {
            moveVector_.y += jumpPower_;
        }
    }
   
}
