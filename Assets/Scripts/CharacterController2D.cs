using System.Collections;
using System.Collections.Generic;
using UnityEditor.Search;
using UnityEngine;

public class CharacterController2D : MonoBehaviour
{
    private Rigidbody2D rb_;

    [Header("移動速度"), SerializeField]
    private float speed_ = 5.0f;

    [Header("最大落下速度"), SerializeField]
    private float maxFallSpeed_ = 10.0f;

    [Header("ジャンプの初速度"), SerializeField]
    private float jumpSpeed_ = 10.0f;

    [Header("地形判定となるレイヤー"),SerializeField] 
    private LayerMask groundLayer;

    private BoxCollider2D boxCollider2D_;

    void Awake()
    {
        rb_ = GetComponent<Rigidbody2D>();
        boxCollider2D_ = GetComponent<BoxCollider2D>();
    }

    private void Start()
    {
        // RigidBodyの当たり判定を座標基準から、移動した軌跡で計算するように変更
        rb_.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void FixedUpdate()
    {
        MoveUpdate();
    }

    private void MoveUpdate()
    {
        Vector2 velocity = rb_.velocity;

        // 落下速度制限
        if (velocity.y < -maxFallSpeed_)
        {
            velocity.y = -maxFallSpeed_;
        }

        //if (IsGrounded())
        //{
        //    velocity.y = 0.0f;
        //}

        rb_.velocity = velocity;
    }

    public void Jump()
    {
        // 一度、速度Yを０にして下降中でも地上と同じジャンプできるように
        Vector2 velocity = rb_.velocity;
        velocity.y = 0.0f;
        rb_.velocity = velocity;
        rb_.AddForce(Vector2.up * jumpSpeed_, ForceMode2D.Impulse);
    }

    public bool IsGrounded()
    {
        float extraHeight = 0.1f;
        RaycastHit2D hit = Physics2D.BoxCast(boxCollider2D_.bounds.center, boxCollider2D_.bounds.size, 0f, Vector2.down, extraHeight, groundLayer);
        return hit.collider != null;
    }

    /// <summary>
    /// 指定した方向に移動させる
    /// 現状横方向のみの移動に限定している
    /// </summary>
    /// <param name="moveDirectionX">移動する方向</param>
    public void HorizontalMove(float moveDirectionX)
    {
        Vector2 velo = rb_.velocity;
        velo.x = moveDirectionX * speed_;
        rb_.velocity = velo;
    }
}
