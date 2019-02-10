using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sim : MonoBehaviour {

    static public Config Config {
        get { return Config.Instance; }
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
        
    }

    void Start () {
        Perm.Start();
        Capi.Start();	
	}
	
	// Update is called once per frame
	void Update () {
		
	}

}
