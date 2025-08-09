using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IParasiteHost
{
    MovementModule GetMovementModule();
    HealthModule GetHealthModule();

    void OnParasiteized(); // Šñ¶‚³‚ê‚½‚Æ‚«‚Ìˆ—
    void OnReleased(); // Šñ¶‰ğœ‚Ìˆ—
}
