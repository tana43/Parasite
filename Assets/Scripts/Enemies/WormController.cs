using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WormController : EnemyController,IParasiteHost
{
    private WalkJumpMovementModule movement_ = new WalkJumpMovementModule();
    private OneHealthModule health_ = new OneHealthModule();

    public MovementModule GetMovementModule() { return movement_; }
    public HealthModule GetHealthModule() { return health_; }

    // Šñ¶‚³‚ê‚½‚Æ‚«‚Ìˆ—
    public void OnParasiteized()
    {

    }

    // Šñ¶‰ğœ‚Ìˆ—
    public void OnReleased()
    {

    }
}
