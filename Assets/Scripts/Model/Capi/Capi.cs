using System;
using System.Collections;
using System.Collections.Generic;
using SimCapi;
using UnityEngine;

/// <summary>
/// Main Class for SS Game. Handles SimCapi interface.
/// </summary>
public class Capi: Singleton<Capi> {

    readonly static public String SIM_ID = "SolarSystemDesigner";

    // Program states.
    public enum States
    {
        Startup, Active, Paused, Stopped
    }

    public States State = States.Startup;

    public Transporter Transporter { get { return SimCapi.Transporter.getInstance(); } }

    public ExposedData Exposed { get { return ExposedData.Instance; }  }
    public PersistentData Persistent { get { return PersistentData.Instance; } }


    public void Awake()
    {

        Exposed.Init();
        Sim.Event.Init();
        Sim.Perm.Init();
        
        Debugger.log("Initializing Transporter");
        Transporter.addInitialSetupCompleteListener(setupComplete);
        Transporter.addHandshakeCompleteListener(handshakeComplete);

    }

    // Initialization
    public void Start () {
        Transporter.notifyOnReady();
    }

    /// <summary>
    /// Called by SimCapi when the initial Capi snapshot has been applied.
    /// Starts project initalization (objects/etc).
    /// </summary>
    /// <param name="message"></param>
    public void setupComplete()
    {
        Debugger.log("SimCapi Setup Complete. Context: " + SimCapi.Transporter.getInstance().getConfig().context);
        // Move to ready state.
        State = States.Active;
    }

    private void handshakeComplete(SimCapiHandshake handshake)
    {
        Debugger.log("SimCapi Handshake Complete.");
    }
    
}
