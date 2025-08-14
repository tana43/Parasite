using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

[RequireComponent(typeof(CharacterController2D))]
public class EnemyController : MonoBehaviour
{
    // ŠeíˆÚ“®ˆ—
    private CharacterController2D characterController_;

    [SerializeField]
    private float speed = 1.0f; 

    void Awake()
    {
        characterController_ = GetComponent<CharacterController2D>();
    }

    // Update is called once per frame
    void Update()
    {
        // ‚Æ‚è‚ ‚¦‚¸¶‰E‚É“®‚©‚µ‚Ä‚İ‚é
        Vector2 move = Vector2.zero;

        // 3•b‚Éˆê‰ñ”½“]
        if(((int)Time.time % 6) > 2)
        {
            move.x = speed * Time.deltaTime;
        }
        else
        {
            move.x = -speed * Time.deltaTime;
        }
        characterController_.Move(move);
    }
}
