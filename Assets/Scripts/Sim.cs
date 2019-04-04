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
    static public WebHandler Web { get { return WebHandler.Instance; } }
    static public Permissions Perm { get { return Permissions.Instance; } }

    /// <summary>
    /// Main Simulation Initiation
    /// </summary>
    public void Awake()
    {
        Web.Init();

        // Set value for FPS
        nextUpdate = Time.time + updateTime;
    }

    void Start () {

        // Check for Parameters in URL. If found, use for initial state.
        if (Web.Param.Count > 0)
        {
            // Update Capi Setup State with URL values.
            Capi.Exposed.StartState.setValue(Web.ParamString);
            // Update Simulation with URL values.
            SetState(Web.ParamString);
        }
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

    }

    public void SetState(string state)
    {
        Dictionary<string, string> states = WebHandler.ToDictionary(state); 
        
        // Read Settings.
        // Note: Most settings are not saved between instances.
        SetProperty<double>(v => Sim.Settings.Speed = v, states, "Speed");

        // Read each Body.
        foreach (Body body in Sim.Bodies.getAll())
        {
            //[TODO] Seperate Task: Set Body based on values.
        }
    }

    /// <summary>
    /// Sets a Property (Lambda) to a dictionary value while converting the value to the choosen type.
    /// </summary>
    /// <typeparam name="T">Type to convert to</typeparam>
    /// <param name="setter">Lambda setter. Use: <code>v => Property = v</code></param>
    /// <param name="dictionary">Dictionary</param>
    /// <param name="key">Key value</param>
    /// <returns>True if the property was set, false if the key does not exist.</returns>
    private bool SetProperty<T>(System.Action<T> setter, Dictionary<string, string> dictionary, string key)
    {
        if (!dictionary.ContainsKey(key)) return false;
        T value = (T)System.Convert.ChangeType(dictionary[key], typeof(T));
        setter(value);
        return true;
    }

}
