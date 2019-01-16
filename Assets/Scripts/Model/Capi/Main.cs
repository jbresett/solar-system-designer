using System;
using System.Collections;
using System.Collections.Generic;
using SimCapi;
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
        Instance = this;

        transporter = SimCapi.Transporter.getInstance();

        // Create NBody system.
        Bodies = new NBody();

        // Create SSData 
        Exposed = new ExposedData();
        Exposed.expose();
        Exposed.setDeligates();

        Debugger.log("Initializing Transporter");
        transporter.addInitialSetupCompleteListener(setupComplete);
        transporter.addHandshakeCompleteListener(handshakeComplete);
        transporter.notifyOnReady();
    }

    // Initialization
    void Start () {

    }

    /// <summary>
    /// Called by SimCapi when the initial Capi snapshot has been applied.
    /// Starts project initalization (objects/etc).
    /// </summary>
    /// <param name="message"></param>
    public void setupComplete()
    {
        Debugger.log("SimCapi Setup Complete.");
        // Move to ready state.
        State = States.Active;
    }


    private void handshakeComplete(SimCapiHandshake handshake)
    {
        Debugger.log("SimCapi Handshake Complete.");
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
