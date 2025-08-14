using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParasiticAttack : MonoBehaviour
{
    private Parasite owner_;
    private PlayerInput input_;

    private void Awake()
    {
        owner_ = GetComponentInParent<Parasite>();
        input_ = GetComponentInParent<PlayerInput>();
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if(input_.inputParasiticAttack_)
        {
            IParasiteHost host = collision.transform.GetComponent<IParasiteHost>();
            if(host != null)
            {
                // �񐶂���
                owner_.Parasitize(host);
            }
        }
    }
}
