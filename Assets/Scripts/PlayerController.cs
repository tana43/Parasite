using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Vector2 velocity_;

    [Header("移動速度"), SerializeField]
    private float speed_ = 2.0f;

    [Header("最大落下速度"), SerializeField]
    private float maxFallSpeed_ = 10.0f;

    [Header("重力"), SerializeField]
    private float gravity_ = 9.8f;

    [Header("ジャンプの初速度"), SerializeField]
    private float jumpSpeed_ = 3.0f;

    private CharacterController characterController_;

    // Start is called before the first frame update
    void Start()
    {
        characterController_ = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        float ax = Input.GetAxis("Horizontal");
        velocity_.x = ax * speed_;

        velocity_.y -= gravity_ * Time.deltaTime;
        velocity_.y = Mathf.Max(velocity_.y, -maxFallSpeed_);

        Vector3 move = new Vector3
        (
            velocity_.x * Time.deltaTime,
            velocity_.y * Time.deltaTime,
            0.00f
        );
        characterController_.Move(move);

        if(characterController_.isGrounded)
        {
            velocity_.y = 0.0f;
        }
    }
}
