using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Sim : MonoBehaviour {

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
    /// <summary>
    /// Time Ratios for Speed.  Used by the Capi interface default options (can be manually set in Capi as well).
    /// </summary>
    public enum SpeedRatio
    {
        Custom = int.MinValue, // For Capi Interface.
        Paused = 0,
        Second = 1,
        Minute = 60,
        Hour = 360,
        Day = 86400,
        Month = 2629800,
        Year = 31557600,
    }

    // Location for the Active and Inactive body containers.
    public GameObject BodyContainer;
    public GameObject BodyPrefab;

    public GameObject StatsLabel;

    public string version;

    // Instance for Sim
    static private Sim instance;
    static public Sim Instance {
       get
       {
            if (instance == null)
            {
                instance = GameObject.FindObjectOfType<Sim>();
            }
            return instance;
       }
    }

    [SerializeField]
    /// <summary>
    /// Simulation speed. Default of 1.0 is s 1 Day / second, or 86,400x times real-time.
    /// </summary>
    static public float Speed
    {
        get { return Capi.Exposed.Speed.getValue(); }
        set {
            Capi.Exposed.Speed.setValue(value);
        }
    }
    /// <summary>
    /// Main Simulation Initiation
    /// </summary>
    private void Awake()
    {
        Perm.Init();
        Bodies.Init(BodyContainer, BodyPrefab);
        Capi.Init();
        CapiBody.Init();

        nextUpdate = Time.time + updateTime;
    }

    void Start () {
        Perm.Start();
        Capi.Start();	
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
