using SimCapi;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class State : Singleton<State> {
    
    // Format for appending body details to a State string. {Id, Key, Value}
    private const string STATE_BODY_APPEND_FMT = "&{0}.{1}={2}";

    // The maximum number of prior saved states stored.
    private const int SAVE_STATE_LIMIT = 20;

    void Awake () {
        CurrentState = new SimCapiString("");
        CurrentState.expose("State.Current", true, false);
        CurrentState.setChangeDelegate(
            delegate (string value, ChangedBy changedBy)
            {
                // Internal updates
                if (changedBy == ChangedBy.SIM) return;
                CurrentState.setValue(value);
                this.Current = value;
            }
        );

        // Intialize prior state list. Individual items are added at run-time.
        savedStates = new List<SimCapiString>();
    }


    /// <summary>
    /// List of previously saved states. A new prior state is added each time the Play button is hit,
    /// and each time the state is manually set.
    /// </summary>
    public List<string> Saved
    {
        get
        {
            List<string> result = new List<string>();
            foreach (SimCapiString state in savedStates)
            {
                result.Add(state.ToString());
            }
            return result;
        }
    }
    private List<SimCapiString> savedStates;
    // Next Id to generate.
    private int NextId = 0;

    /// <summary>
    /// Returns the most recent saved state.
    /// If no states have been saved, returns null.
    /// </summary>
    /// <returns></returns>
    public string GetLastSave()
    {
        if (savedStates.Count == 0) return null;
        return savedStates[savedStates.Count - 1].getValue();
    }

    /// <summary>
    /// Saves the current state to a save slot.
    /// </summary>
    public void Save()
    {
        string state = Current;

        // Ignore save is state has not changed from last save.
        if (state == GetLastSave()) return;

        SimCapiString capi = new SimCapiString("");
        capi.expose("State.History." + NextId, false, false);
        capi.setValue(state);
        savedStates.Add(capi);
        NextId++;

        // Remove oldest state (except original) if more prior states exist then the PRIOR_STATE_LIMIT.
        if (savedStates.Count > SAVE_STATE_LIMIT)
        {
            savedStates[1].unexpose();
            savedStates.RemoveAt(1);
        }
    }

    /// <summary>
    /// Current state of the simulation, including all active bodies.
    /// </summary>
    public string Current
    {
        get { return GetState(); }
        set
        {
            Settings.Instance.Paused = true;
            SetState(value);
            UpdateCapi();
            Save();
        }
    }

    /// <summary>
    /// Updates the displayed value in Capi. 
    /// </summary>
    /// <param name="value"></param>
    public void UpdateCapi()
    {
        CurrentState.setValue(Current);
    }
    private SimCapiString CurrentState;

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
            string vel = (body.isInitialVel ? body.Velocity.ToString() : "Auto");
            result.AppendFormat(STATE_BODY_APPEND_FMT, body.Id, "Velocity", vel);
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
                // Velocity: Check if it's automatic or has been set.
                string vel = states[i + ".Velocity"];
                if (string.Equals(vel, "Auto", System.StringComparison.InvariantCultureIgnoreCase))
                {
                    body.Velocity = new Vector3d();
                    body.isInitialVel = false;
                }
                else
                {
                    body.Velocity = new Vector3d(vel);
                    body.isInitialVel = true;
                }
                body.Active = true;
            }
        }
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
}
