using System.Collections;
using System.Collections.Generic;
using UnityEditor.Search;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Collider2D))]
public class CharacterController2D : MonoBehaviour
{
    [Header("�n�`����ƂȂ郌�C���["),SerializeField] 
    private LayerMask groundLayer_;

    [Header("�n��ɗ����Ă���Ƃ݂Ȃ�����"), SerializeField]
    private float groundedRaycastDistance_ = 0.1f;

    Vector2 previousPosition_;
    Vector2 currentPosition_;
    Vector2 nextMovement_;

    // �n��ɂ��邩
    public bool isGrounded_ { get; private set; }

    // �V��ɓ������Ă��邩
    public bool isCeilinged_ { get; private set; }

    public Vector2 velocity_ { get; private set; }

    private Rigidbody2D rigidbody2D_;
    private CapsuleCollider2D capsuleCollider2D_;
    ContactFilter2D contactFilter_;
    RaycastHit2D[] hitBuffer_ = new RaycastHit2D[5];
    RaycastHit2D[] foundHits_ = new RaycastHit2D[3];
    Collider2D[] groundColliders_ = new Collider2D[3];


    void Awake()
    {
        rigidbody2D_ = GetComponent<Rigidbody2D>();
        capsuleCollider2D_ = GetComponent<CapsuleCollider2D>();

        currentPosition_ = rigidbody2D_.position;
        previousPosition_ = rigidbody2D_.position;

        // raycast�̃I�v�V����
        contactFilter_.layerMask = groundLayer_;
        contactFilter_.useLayerMask = true;
        contactFilter_.useTriggers = false;

        // Raycast�̊J�n�_�������̃R���C�_�[�������炾�����Ƃ��ɂ��̃R���C�_�[�𖳎�����悤�ɐݒ�
        Physics2D.queriesStartInColliders = false;
    }

    private void Start()
    {
        // RigidBody�̓����蔻������W�����A�ړ������O�ՂŌv�Z����悤�ɕύX
        rigidbody2D_.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
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
        previousPosition_ = rigidbody2D_.position;
        currentPosition_ = previousPosition_ + nextMovement_;

        velocity_ = (currentPosition_ - previousPosition_) / Time.deltaTime;
        rigidbody2D_.MovePosition(currentPosition_);
        nextMovement_ = Vector2.zero;

        // �V��ƒn�ʂƂ̔���
        CheckCapsuleEndCollisions(true);
        CheckCapsuleEndCollisions(false);
    }

    /// <summary>
    /// Rigidbody�ɂ��n�`������������ړ������B
    /// </summary>
    /// <param name="movement">�ړ��l�B�P�ʂ�m/s</param>
    public void Move(Vector2 movement)
    {
        nextMovement_ += movement;
    }

    /// <summary>
    /// �����蔻��𖳎������ړ�����
    /// </summary>
    /// <param name="position">�]�ڐ�̍��W</param>
    public void Teleport(Vector2 position)
    {
        Vector2 delta = position - currentPosition_;
        previousPosition_ += delta;
        currentPosition_ = position;
        rigidbody2D_.MovePosition(position);
    }

    /// <summary>
    /// �����isGrounded��������isCeilinged�̏�Ԃ��X�V���܂��BFixedUpdate�ł͎����I�ɌĂяo����܂����A��荂�����x���K�v�ȏꍇ�͂��p�ɂɌĂяo�����Ƃ��ł��܂��B
    /// </summary>
    /// <param name="bottom"></param>
    public void CheckCapsuleEndCollisions(bool bottom = true)
    {
        // ���C��3�{�g�p���Ēn�`��������

        Vector2 raycastDirection;
        Vector2 raycastStart;
        float raycastDistace;

        if (capsuleCollider2D_ != null)
        {
            raycastStart = rigidbody2D_.position + capsuleCollider2D_.offset;
            raycastDistace = capsuleCollider2D_.size.x * 0.5f + groundedRaycastDistance_ * 2f;

            Vector2[] raycastPositions = new Vector2[3];

            if (bottom)
            {
                raycastDirection = Vector2.down;
                Vector2 raycastStartBottomCenter = raycastStart + Vector2.down * (capsuleCollider2D_.offset.y * 0.5f - capsuleCollider2D_.offset.x * 0.5f);

                raycastPositions[0] = raycastStartBottomCenter + Vector2.left * capsuleCollider2D_.size.x * 0.5f;
                raycastPositions[1] = raycastStartBottomCenter;
                raycastPositions[2] = raycastStartBottomCenter + Vector2.right * capsuleCollider2D_.size.x * 0.5f;
            }
            else 
            {
                raycastDirection = Vector2.up;
                Vector2 raycastStartBottomCenter = raycastStart + Vector2.up * (capsuleCollider2D_.offset.y * 0.5f + capsuleCollider2D_.offset.x * 0.5f);

                raycastPositions[0] = raycastStartBottomCenter + Vector2.left * capsuleCollider2D_.size.x * 0.5f;
                raycastPositions[1] = raycastStartBottomCenter;
                raycastPositions[2] = raycastStartBottomCenter + Vector2.right * capsuleCollider2D_.size.x * 0.5f;
            }

            // �p�ӂ������C�̊J�n�ʒu���牺�������͏�փ��C���΂�
            for (int i = 0; i < raycastPositions.Length; i++)
            {
                int count = Physics2D.Raycast(raycastPositions[i], raycastDirection, contactFilter_, hitBuffer_, raycastDistace);

                if (bottom)
                {
                    // �n�ʂɂ��������ꍇ�A���̃R���C�_�[�ƃ��C�̏���ێ�
                    foundHits_[i] = count > 0 ? hitBuffer_[0] : new RaycastHit2D();
                    groundColliders_[i] = foundHits_[i].collider;
                }
                else
                {
                    // �V��ɂ���������

                    isCeilinged_ = false;

                    for (int j = 0; j < hitBuffer_.Length; j++)
                    {
                        if (hitBuffer_[j].collider != null)
                        {
                            isCeilinged_ = true;
                        }
                    }
                }
            }

            if(bottom)
            {
                // ���C�����������n�ʂ̖@���̕��ϒl����n��ɂ��Ă��邩���肷��

                Vector2 groundNormal = Vector2.zero;
                int hitCount = 0;

                for (int i = 0; i < foundHits_.Length; i++)
                {
                    if (foundHits_[i].collider != null)
                    {
                        groundNormal += foundHits_[i].normal;
                        hitCount++;
                    }
                }

                if(hitCount > 0)
                {
                    groundNormal.Normalize();
                }

                if (Mathf.Approximately(groundNormal.x, 0f) && Mathf.Approximately(groundNormal.y, 0f))
                {
                    isGrounded_ = false;
                }
                else
                {
                    isGrounded_ = velocity_.y <= 0f;
                }
            }

            for (int i = 0; i < hitBuffer_.Length; i++)
            {
                hitBuffer_[i] = new RaycastHit2D();
            }
        }
    }
}
