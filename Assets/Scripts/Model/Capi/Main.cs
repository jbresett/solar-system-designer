using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Main Class for SS Game. Handles SimCapi interface.
/// </summary>
public class Main: MonoBehaviour {

    readonly static public String SIM_ID = "SolarSystemDesigner";

    // Singleton instance. Created at start.
    static public Main Instance { get; private set; }

    // Program states.
    public enum States
    {
        Startup, Active, Paused, Stopped
    }
    public States State = States.Startup;


    // NBody Simulator
    public NBody Bodies { get; private set; }

    public ExposedData Exposed { get; private set; }
    public PersistentData Persistent { get; private set; }

    SimCapi.Transporter transporter;

    private void Awake()
    {
        transporter = SimCapi.Transporter.getInstance();
    }

    // Initialization
    void Start () {

        // Create singleton instance.
        if (Instance != null) throw new InvalidOperationException("Process has already been started.");
        Instance = this;

        // Create NBody system.
        Bodies = new NBody();

        // Create SSData 
        Exposed = new ExposedData();
        Exposed.expose();
        Exposed.setDeligates();

        SimCapi.Transporter transporter = SimCapi.Transporter.getInstance();
        // Feature not implemented by Capi at this time.
        //transporter.addInitialSetupCompleteListener(this.setupComplete);
        transporter.notifyOnReady();
    }

    /// <summary>
    /// Called by SimCapi when the initial Capi snapshot has been applied.
    /// Starts project initalization (objects/etc).
    /// </summary>
    /// <param name="message"></param>
    public void setupComplete(SimCapi.Message message)
    {
        //TODO: Add future init code here.

        // Move to ready state.
        State = States.Active;
    }

    // Frame Update
    void Update() {
        /// Code that occurs every frame, regardless of state.
        //TODO: Add future frame code here.    

        /** Active state only. **/
        if (State != States.Active) return;
        
        //TODO: Add future frame code here.
	}
}
