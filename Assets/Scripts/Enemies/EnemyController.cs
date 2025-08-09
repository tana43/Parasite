using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    // ŠeíˆÚ“®ˆ—
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
