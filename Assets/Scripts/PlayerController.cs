using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

[RequireComponent(typeof(PlayerInput))]
public class PlayerController : MonoBehaviour
{
    // �e��ړ�����
    private CharacterController2D characterController_;

    // �񐶏����̒S���R���|�[�l���g
    private Parasite parasite_;

    private PlayerInput input_;

    [Header("�������ւ̑��x"),SerializeField]
    private float horizontalSpeed_ = 5.0f;

    [Header("�������x"),SerializeField]
    private float maxFallSpeed_ = 5.0f;

    [Header("�d��"),SerializeField]
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
        float move = input_.inputHorizontalMove_;


        if (Input.GetKeyDown(KeyCode.Space))
        {
            characterController_.Jump();
        }
    }
    

   
}
