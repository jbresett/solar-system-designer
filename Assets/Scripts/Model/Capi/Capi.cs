﻿using System;
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

    public ExposedData Exposed { get { return ExposedData.Instance; }  }
    public PersistentData Persistent { get { return PersistentData.Instance; } }
    public Transporter Transporter {  get { return SimCapi.Transporter.getInstance(); } }

    public void Awake()
    {
        Exposed.Init();
 
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
    
    /// <summary>
    /// Logs an simulation event or issue into the Capi server.
    /// </summary>
    /// <param name="eventString"></param>
    public void Log(string eventString)
    {
        Exposed.capiEvents.getList().Add(eventString);
        Exposed.capiEvents.updateValue();
    }
}
