using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Primary Simulation instance. Singleton class instances of can be accessed via
/// Sim.(Class) or (Class).Instance
/// </summary>
public class Sim : Singleton<Sim> {

    static public Config Config
    {
        get { return Config.Instance; }
    }

    static public Settings Settings
    {
        get { return Settings.Instance; }
    }

    static public Capi Capi
    { 
        get { return Capi.Instance; }
    }

    static public Bodies Bodies
    {
        get { return Bodies.Instance; }
    }

    static public Perm Perm
    {
        get { return Perm.Instance; }
    }

    /// <summary>
    /// Simulation speed. Default of 1.0 is s 1 Day / second, or 86,400x times real-time.
    /// </summary>
    [System.Obsolete("Call Sim.Settings.Speed")]
    static public float Speed
    {
        get { return Settings.Speed;  }
        set { Settings.Speed = value; }
    }

    /// <summary>
    /// Main Simulation Initiation
    /// </summary>
    public void Awake()
    {
        CapiBody.Init();
    }

    void Start () {

	}


}
