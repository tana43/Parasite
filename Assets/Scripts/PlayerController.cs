using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerController : MonoBehaviour
{
    // �e��ړ�����
    private CharacterController2D characterController_;

    // �񐶏����̒S���R���|�[�l���g
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
        // �ړ�����
        MovementModule movement = parasite_.GetMovementModule();
        if (movement != null)
        {
            movement.Move(characterController_);
        }
        else
        {
            Debug.Log("MovementModule���ݒ肳��Ă��܂���");
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
