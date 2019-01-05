using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//TODO: Later Sprint should include saving preferences between 
/// <summary>
/// Holds a static reference to all user preferences.
/// </summary>
public class Preferences {

    /// <summary>
    /// Initializes static properties.
    /// </summary>
    static Preferences() {
        Keyboard = new KeyboardClass();
        Mouse = new MouseClass();
    }

    /// <summary>
    /// Preference Sub-class that holds multiple speed settings:
    /// Movement, Rotation, and Speed.
    /// Values normally range from 0.0 to 1.0.
    /// </summary>
    public class Speed
    {
        public double Movement { get; set; }
        public double Rotation{ get; set; }
        public double Zoom { get; set; }
        
        // Intialize Values
        public Speed()
        {
            Movement = 0.5;
            Rotation = 0.5;
            Zoom = 0.5;
        }
    }

    // Handles speed application.
    public class KeyboardClass: Speed { }
    public class MouseClass : Speed { }

    /// <summary>
    /// Keyboard Preferences.
    /// </summary>
    static public KeyboardClass Keyboard { get; set; }

    /// <summary>
    /// Mouse Preferences.
    /// </summary>
    static public MouseClass Mouse { get; set; }

}
