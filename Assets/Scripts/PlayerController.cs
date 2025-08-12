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

    [Header("横方向への速度"),SerializeField]
    private float horizontalSpeed_ = 5.0f;

    [Header("落下速度"),SerializeField]
    private float maxFallSpeed_ = 5.0f;

    [Header("重力"),SerializeField]
    private float gravity_ = 10.0f;

    // Start is called before the first frame update
    void Awake()
    {
        characterController_ = GetComponent<CharacterController2D>();
        parasite_ = GetComponent<Parasite>();
    }

    // Update is called once per frame

    private void Update()
    {
        // 移動処理
        MovementModule movement = parasite_.GetMovementModule();
        if (movement != null)
        {
            movement.Move(characterController_);
        }
        else
        {
            Debug.Log("MovementModuleが設定されていません");
        }
    }

    private void DefaultMove()
    {
        float move = input_.inputHorizontalMove_;


        if (Input.GetKeyDown(KeyCode.Space))
        {
            characterController_.Jump();
        }
    }
    

   
}
