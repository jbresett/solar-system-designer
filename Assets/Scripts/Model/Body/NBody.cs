using Planets;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Stores all data for the NBody system and handles over-time calculations of body locations.
/// </summary>
static public class NBody {
    
    public const int MAX_BODY_COUNT = 30;

    static Planets.PhysicsBody[] Bodies = new Planets.PhysicsBody[MAX_BODY_COUNT];

    static public int register(Planets.PhysicsBody body)
    {

        for (int i = 0; i < MAX_BODY_COUNT; i++)
        {
            if (Bodies[i] == null) {
                Bodies[i] = body;
                return i;
            }
        }
        throw new OverflowException("Already at max body count.");
    }

    static public bool unregister(Planets.PhysicsBody body)
    {
        for (int i = 0; i < MAX_BODY_COUNT; i++)
        {
            if (Bodies[i] == body)
            {
                Bodies[i] = null;
                return true;
            }
        }
        // Not Found.
        return false;
    }


    /// <summary>
    /// List of all bodies within the system.
    /// </summary>
    //public Dictionary<string, Body> Bodies { get; private set; }

    /// <summary>
    /// Current time for the simulation, in N days from starting points.
    /// </summary>
    static public double Time
    {
        get { return time; }
        set
        {
            time = value;
            Capi.Exposed.Time.setValue((float)value);
        }
    }
    static private double time;

    /// <summary>
    /// Amount of days that pass every frame. Intially starts at 0.
    /// </summary>
    static public double Speed { get; set; }

    /// <summary>
    /// Name of the body that currently has focus.
    /// 
    /// If Null or empty string, focus is on coordinates (0,0,0).
    /// </summary>
    static public string Focused {
        get { return focused; }
        set
        {
            focused = value;
            // Instantly updates Exposed value.
            Capi.Exposed.FocusedBody.setValue(value);
        }
    }
    static private string focused;

	/// <summary>
    /// Updates once per frame
    /// </summary>
	static void Update () {
        // Time is updated internally so Capi doesn't require an update on every frame. 
        time += Speed;	
        updatePositions();
	}

    /// <summary>
    /// updates positions of orbital bodies
    /// </summmary>
    static public void updatePositions()
    {

    }

}
