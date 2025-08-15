using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public  abstract class ParasiteHost : MonoBehaviour
{
    public abstract MovementModule GetMovementModule();
    public abstract HealthModule GetHealthModule();

    public abstract void OnParasiteized(); // Šñ¶‚³‚ê‚½‚Æ‚«‚Ìˆ—
    public abstract void OnReleased(); // Šñ¶‰ğœ‚Ìˆ—
}
