using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

/// <summary>
/// Primary Simulation instance. Singleton class instances of can be accessed via
/// Sim.(Class) or (Class).Instance
/// </summary>
public class Sim : Singleton<Sim> {

    // Frame Rate Tracker
    private float nextUpdate = 0;
    private float updateTime = 1.0f; //Seconds Per Update
    private int frameCount = 0;

    static public Config Config {
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

    // Location for the Active and Inactive body containers.
    public GameObject BodyContainer;
    public GameObject BodyPrefab;

    public GameObject StatsLabel;

    public string version;

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

        nextUpdate = Time.time + updateTime;
    }

    void Start () {

	}
	
	void Update () {
        frameCount++;

        if (Time.time >= nextUpdate)
        {
            // Set next update.
            nextUpdate = Time.time + updateTime;
            
            // Calculate and display frames/second.
            var fps = frameCount / updateTime;
            StatsLabel.GetComponent<TextMeshProUGUI>().text = string.Format("{0:0} fps", fps);
                
            // Reset FrameCount for next set.
            frameCount = 0;
        }
	}

}
