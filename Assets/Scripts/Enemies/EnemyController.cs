using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    // �e��ړ�����
    private CharacterController2D characterController_;

    void Awake()
    {
        characterController_ = GetComponent<CharacterController2D>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
