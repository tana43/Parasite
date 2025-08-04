using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerController : MonoBehaviour
{
    [Header("移動速度"), SerializeField]
    private float speed_ = 5.0f;

    [Header("最大落下速度"), SerializeField]
    private float maxFallSpeed_ = 10.0f;

    [Header("ジャンプの初速度"), SerializeField]
    private float jumpSpeed_ = 10.0f;

    [Header("重力加速度"), SerializeField]
    private float gravity_ = 9.8f;
    

    private Rigidbody2D rb_;


    // Start is called before the first frame update
    void Start()
    {
        rb_ = GetComponent<Rigidbody2D>();

        // RigidBodyの当たり判定を座標基準から、移動した軌跡で計算するように変更
        rb_.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        MoveUpdate();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Jump();
        }
    }

    private void MoveUpdate()
    {
        Vector2 velocity = rb_.velocity;

        // 水平方向移動
        float ax = Input.GetAxis("Horizontal");
        if (Mathf.Approximately(ax, 0f))
        {
            velocity.x = 0f;
        }
        else
        {
            velocity.x = ax * speed_;
        }

        // 落下速度制限
        velocity.y = Mathf.Max(velocity.y, -maxFallSpeed_);

        rb_.velocity = velocity;

        if(IsGrounded())
        {
            velocity.y = 0.0f;
        }
        
    }

    private void Jump()
    {
        // 一度、速度Yを０にして下降中でも地上と同じジャンプできるように
        Vector2 velocity = rb_.velocity;
        velocity.y = 0.0f;
        rb_.velocity = velocity;
        rb_.AddForce(Vector2.up * jumpSpeed_, ForceMode2D.Impulse);
    }

    private bool IsGrounded()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, 0.1f);
        return hit.collider != null;
    }
}
