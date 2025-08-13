using System.Collections;
using System.Collections.Generic;
using UnityEditor.Search;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Collider2D))]
public class CharacterController2D : MonoBehaviour
{
    [Header("地形判定となるレイヤー"),SerializeField] 
    private LayerMask groundLayer_;

    [Header("地上に立っているとみなす距離"), SerializeField]
    private float groundedRaycastDistance_ = 0.1f;

    Vector2 previousPosition_;
    Vector2 currentPosition_;
    Vector2 nextMovement_;

    // 地上にいるか
    public bool isGrounded_ { get; private set; }

    // 天井に当たっているか
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

        // raycastのオプション
        contactFilter_.layerMask = groundLayer_;
        contactFilter_.useLayerMask = true;
        contactFilter_.useTriggers = false;

        // Raycastの開始点が何かのコライダー内部からだったときにそのコライダーを無視するように設定
        Physics2D.queriesStartInColliders = false;
    }

    private void Start()
    {
        // RigidBodyの当たり判定を座標基準から、移動した軌跡で計算するように変更
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

        // 天井と地面との判定
        CheckCapsuleEndCollisions(true);
        CheckCapsuleEndCollisions(false);
    }

    /// <summary>
    /// Rigidbodyによる地形判定を持った移動処理。
    /// </summary>
    /// <param name="movement">移動値。単位はm/s</param>
    public void Move(Vector2 movement)
    {
        nextMovement_ += movement;
    }

    /// <summary>
    /// 当たり判定を無視した移動処理
    /// </summary>
    /// <param name="position">転移先の座標</param>
    public void Teleport(Vector2 position)
    {
        Vector2 delta = position - currentPosition_;
        previousPosition_ += delta;
        currentPosition_ = position;
        rigidbody2D_.MovePosition(position);
    }

    /// <summary>
    /// これはisGroundedもしくはisCeilingedの状態を更新します。FixedUpdateでは自動的に呼び出されますが、より高い精度が必要な場合はより頻繁に呼び出すことができます。
    /// </summary>
    /// <param name="bottom"></param>
    public void CheckCapsuleEndCollisions(bool bottom = true)
    {
        // レイを3本使用して地形判定を取る

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

            // 用意したレイの開始位置から下もしくは上へレイを飛ばす
            for (int i = 0; i < raycastPositions.Length; i++)
            {
                int count = Physics2D.Raycast(raycastPositions[i], raycastDirection, contactFilter_, hitBuffer_, raycastDistace);

                if (bottom)
                {
                    // 地面にあたった場合、そのコライダーとレイの情報を保持
                    foundHits_[i] = count > 0 ? hitBuffer_[0] : new RaycastHit2D();
                    groundColliders_[i] = foundHits_[i].collider;
                }
                else
                {
                    // 天井にあたったか

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
                // レイが当たった地面の法線の平均値から地上にいているか判定する

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
