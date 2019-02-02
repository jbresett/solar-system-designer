using System;
using System.Collections;
using System.Collections.Generic;
using SimCapi;
using UnityEngine;

/// <summary>
/// Main Class for SS Game. Handles SimCapi interface.
/// </summary>
public class Capi {

    readonly static public String SIM_ID = "SolarSystemDesigner";

    // Singleton instance. Created at start.
    static public Capi Instance { get; private set; }

    // Program states.
    public enum States
    {
        Startup, Active, Paused, Stopped
    }

    static public States State = States.Startup;

    static public ExposedData Exposed { get; private set; }
    static public PersistentData Persistent { get; private set; }

    static SimCapi.Transporter transporter;

    static public void Init()
    {

        transporter = SimCapi.Transporter.getInstance();

        // Create SSData 
        Exposed = new ExposedData();
        Exposed.exposeAll();
        Exposed.setDeligates();

        Debugger.log("Initializing Transporter");
        transporter.addInitialSetupCompleteListener(setupComplete);
        transporter.addHandshakeCompleteListener(handshakeComplete);

    }

    // Initialization
    static public void Start () {
        transporter.notifyOnReady();
    }

    /// <summary>
    /// Called by SimCapi when the initial Capi snapshot has been applied.
    /// Starts project initalization (objects/etc).
    /// </summary>
    /// <param name="message"></param>
    static public void setupComplete()
    {
        Debugger.log("SimCapi Setup Complete.");
        Debugger.log("SimCapi Context: " + SimCapi.Transporter.getInstance().getConfig().context);
        // Move to ready state.
        State = States.Active;
    }


    static private void handshakeComplete(SimCapiHandshake handshake)
    {
        Debugger.log("SimCapi Handshake Complete.");
    }

    // Frame Update
    static public void Update() {
        /// Code that occurs every frame, regardless of state.
        //TODO: Add future frame code here.    

        /** Active state only. **/
        if (State != States.Active) return;
        
        //TODO: Add future frame code here.
	}
}
