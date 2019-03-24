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

    // Location for the Active and Inactive body containers.
    public GameObject BodyContainer;
    public GameObject BodyPrefab;
    public GameObject StatsLabel;

    // Simple References
    static public Config Config { get { return Config.Instance; } }
    static public Settings Settings { get { return Settings.Instance; } }
    static public Capi Capi { get { return Capi.Instance; } }
    static public Bodies Bodies { get { return Bodies.Instance; } }
    static public SimEventHandler Event { get { return SimEventHandler.Instance; } }
    static public Perm Perm { get { return Perm.Instance; } }

    /// <summary>
    /// Main Simulation Initiation
    /// </summary>
    public void Awake()
    {
        // Set value for FPS
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

    public void Reset()
    {
        Sim.Settings.Paused = true;
        Sim.Event.Clear();

        foreach (Body body in Sim.Bodies.getAll())
        {
            body.Position = body.InitialPosition;
            body.Velocity = new Vector3d();
            body.isInitialVel = false;
        }
    }

}
