using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    public bool inputAttack_ { get; private set; }
    public float inputHorizontalMove_ { get; private set; }
    public bool inputJump_ { get; private set; }
    public bool inputParasiticAttack_ { get; private set; }

    // Update is called once per frame
    void Update()
    {
        UpdateAttack();

        UpdateJump();

        UpdateParasiteAttack();

        UpdateHorizontalMove();
    }

    private void UpdateAttack()
    {
        inputAttack_ = Input.GetKeyDown(KeyCode.E);
    }

    private void UpdateHorizontalMove()
    {
        inputHorizontalMove_ = Input.GetAxis("Horizontal");
    }

    private void UpdateJump()
    {
        inputJump_ = Input.GetKey(KeyCode.Space);
    }

    private void UpdateParasiteAttack()
    {
        inputParasiticAttack_ = Input.GetKeyDown(KeyCode.F);
    }
}
