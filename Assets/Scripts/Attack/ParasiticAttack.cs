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
            ParasiteHost host = collision.transform.GetComponent<ParasiteHost>();
            if(host != null)
            {
                // äÒê∂Ç∑ÇÈ
                owner_.Parasitize(ref host);
            }
        }
    }
}
