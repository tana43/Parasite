using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerController : MonoBehaviour
{
    // 各種移動処理
    private CharacterController2D characterController_;

    // 寄生処理の担うコンポーネント
    private Parasite parasite_;

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
        float inputX = Input.GetAxis("Horizontal");
        characterController_.HorizontalMove(inputX);

        if (Input.GetKeyDown(KeyCode.Space))
        {
            characterController_.Jump();
        }
    }
    

   
}
