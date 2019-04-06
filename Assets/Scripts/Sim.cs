using System.Collections;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;

/// <summary>
/// Primary Simulation instance. Singleton class instances of can be accessed via
/// Sim.(Class) or (Class).Instance
/// </summary>
public class Sim : Singleton<Sim> {
    // Format for appending body details to a State string. {Id, Key, Value}
    private const string STATE_BODY_APPEND_FMT = "&{0}.{1}={2}";

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
    }


    public static Dictionary<string, string> ToDictionary(string state)
    {
        // Convert Parameters into dictionary.
        Dictionary<string, string> result = new Dictionary<string, string>();

        foreach (string param in state.Split('&'))
        {
            // 'Key=Value' pair.
            string[] pair = param.Split('=');

            // Check for proper array length:
            // * must have exactly 1 '='
            // * include both a key and value (neither 0-length).
            if (pair.Length == 2 && pair[0].Length > 0 && pair[1].Length > 0)
            {
                // Convert from escaped URL to standard string.
                result.Add(WWW.UnEscapeURL(pair[0]), WWW.UnEscapeURL(pair[1]));
            }
        }
        return result;
    }



    /// <summary>
    /// Current state of the simulation, including all active bodies.
    /// </summary>
    public string State
    {
        get { return GetState(); }
        set { SetState(value); }
    }

    private string GetState()
    {
        StringBuilder result = new StringBuilder();
        result.Append("Speed=");
        result.Append(Sim.Settings.Speed);
        foreach (Body body in Sim.Bodies.Active)
        {
            // Append Basic Details
            result.AppendFormat(STATE_BODY_APPEND_FMT, body.Id, "Name", body.Name.Escape());
            result.AppendFormat(STATE_BODY_APPEND_FMT, body.Id, "Type", body.Type.ToString().Escape());
            result.AppendFormat(STATE_BODY_APPEND_FMT, body.Id, "Material", body.Material.ToString().Escape());

            // Append Internal
            result.AppendFormat(STATE_BODY_APPEND_FMT, body.Id, "Mass", body.Mass);
            result.AppendFormat(STATE_BODY_APPEND_FMT, body.Id, "Diameter", body.Diameter);

            // Append Motion
            result.AppendFormat(STATE_BODY_APPEND_FMT, body.Id, "Rotation", body.Rotation);
            result.AppendFormat(STATE_BODY_APPEND_FMT, body.Id, "Position", body.Position);
            result.AppendFormat(STATE_BODY_APPEND_FMT, body.Id, "Velocity", body.Velocity);
        }

        return result.ToString();
    }

    private void SetState(string state)
    {
        Dictionary<string, string> states = ToDictionary(state);
        Sim.Settings.Speed = int.Parse(states["Speed"]);
        for (int i = 0; i < Sim.Bodies.All.Length; i++)
        {
            Body body = Sim.Bodies.All[i];

            // Deactivates a body before making any changes. 
            // If no state details exist, body will remain deactivated.
            body.Active = false;
            if (states.ContainsKey(i + ".Name"))
            {
                body.Name = states[i + ".Name"].UnEscape();
                body.Type = states[i + ".Type"].UnEscape().Enum<BodyType>();
                body.Material = states[i + ".Material"].UnEscape().Enum<BodyMaterial>();
                body.Mass = double.Parse(states[i + ".Mass"]);
                body.Diameter = double.Parse(states[i + ".Diameter"]);
                body.Rotation = double.Parse(states[i + ".Rotation"]);
                body.Position = new Vector3d(states[i + ".Position"]);
                body.Velocity = new Vector3d(states[i + ".Velocity"]);

                body.Active = true;
            }
        }
    }
}
