using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerController : MonoBehaviour
{
    [Header("�ړ����x"), SerializeField]
    private float speed_ = 5.0f;

    [Header("�ő嗎�����x"), SerializeField]
    private float maxFallSpeed_ = 10.0f;

    [Header("�W�����v�̏����x"), SerializeField]
    private float jumpSpeed_ = 10.0f;

    [Header("�d�͉����x"), SerializeField]
    private float gravity_ = 9.8f;
    

    private Rigidbody2D rb_;


    // Start is called before the first frame update
    void Start()
    {
        rb_ = GetComponent<Rigidbody2D>();

        // RigidBody�̓����蔻������W�����A�ړ������O�ՂŌv�Z����悤�ɕύX
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

        // ���������ړ�
        float ax = Input.GetAxis("Horizontal");
        if (Mathf.Approximately(ax, 0f))
        {
            velocity.x = 0f;
        }
        else
        {
            velocity.x = ax * speed_;
        }

        // �������x����
        velocity.y = Mathf.Max(velocity.y, -maxFallSpeed_);

        rb_.velocity = velocity;

        if(IsGrounded())
        {
            velocity.y = 0.0f;
        }
        
    }

    private void Jump()
    {
        // ��x�A���xY���O�ɂ��ĉ��~���ł��n��Ɠ����W�����v�ł���悤��
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
