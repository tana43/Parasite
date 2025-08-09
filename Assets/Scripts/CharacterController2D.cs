using System.Collections;
using System.Collections.Generic;
using UnityEditor.Search;
using UnityEngine;

public class CharacterController2D : MonoBehaviour
{
    private Rigidbody2D rb_;

    [Header("�ړ����x"), SerializeField]
    private float speed_ = 5.0f;

    [Header("�ő嗎�����x"), SerializeField]
    private float maxFallSpeed_ = 10.0f;

    [Header("�W�����v�̏����x"), SerializeField]
    private float jumpSpeed_ = 10.0f;

    [Header("�n�`����ƂȂ郌�C���["),SerializeField] 
    private LayerMask groundLayer;

    private BoxCollider2D boxCollider2D_;

    void Awake()
    {
        rb_ = GetComponent<Rigidbody2D>();
        boxCollider2D_ = GetComponent<BoxCollider2D>();
    }

    private void Start()
    {
        // RigidBody�̓����蔻������W�����A�ړ������O�ՂŌv�Z����悤�ɕύX
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

        // �������x����
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
        // ��x�A���xY���O�ɂ��ĉ��~���ł��n��Ɠ����W�����v�ł���悤��
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
    /// �w�肵�������Ɉړ�������
    /// ���󉡕����݂̂̈ړ��Ɍ��肵�Ă���
    /// </summary>
    /// <param name="moveDirectionX">�ړ��������</param>
    public void HorizontalMove(float moveDirectionX)
    {
        Vector2 velo = rb_.velocity;
        velo.x = moveDirectionX * speed_;
        rb_.velocity = velo;
    }
}
