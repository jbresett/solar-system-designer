using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// In-Simulation events that can occur.
/// </summary>
// Note: Safe to add more event types. No additional programming needed.
public enum SimEvent {
    None,      // Placeholder event. 
    Insertion, // User added a planet.
    Collision, // Collision between 2 bodies of similar mass.
    Impact,    // Small body impaacted a larger body.
    Nova,      // Star Nova.
    Collapse,  // Star collapse (black hole created).

}
