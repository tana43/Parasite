using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OneHealthModule : HealthModule
{
    const int health_ = 1;
    public override int GetMaxHealth()
    {
        return health_;
    }
}
