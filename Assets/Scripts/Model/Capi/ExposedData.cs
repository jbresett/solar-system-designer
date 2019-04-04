﻿using Planets;
using SimCapi;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// Handles top-level data exposed to SimCapi.
/// </summary>
public class ExposedData: Singleton<ExposedData> {

    /// <summary>
    /// Current simulation speed.
    /// </summary>
    public SimCapiNumber capiSpeedTime;
    public SimCapiEnum<SpeedRatio> capiSpeedRatio;
    public SimCapiBoolean capiPaused;

    /// <summary>
    /// Body that currently has focus. Empty string ("") means no body is currently selected (free camera).
    /// </summary>
    public SimCapiString capiFocused;
    
    /// <summary>
    /// Initial state for the solar system. State includes all solar system details and 
    /// related settings (e.g. Simulation running speed).
    /// </summary>
    public SimCapiString StartState;
    /// <summary>
    /// Current State. Updates after each change while simulation is paused, and after
    /// pausing.
    /// </summary>
    public SimCapiString CurrentState;
    /// <summary>
    /// List of previously saved states. A new prior state is added before each Run.
    /// </summary>
    public List<SimCapiString> PriorStates;

    /// <summary>
    /// Sets initial values.
    /// </summary>
    public void Init() {
        StartState = new SimCapiString("");
        StartState.expose("State.Start", false, false);
        StartState.setChangeDelegate(
            delegate (String value, ChangedBy changedBy)
            {
                // Internal updates
                if (changedBy == ChangedBy.SIM) return;
                CurrentState.setValue(value);
                Sim.Instance.SetState(value);
            }
        );

        CurrentState = new SimCapiString("");
        CurrentState.expose("State.Current", true, false);

        // Intialize prior state list. Individual items are added at run-time.
        PriorStates = new List<SimCapiString>();

        capiPaused = new SimCapiBoolean(Sim.Settings.Paused);
        capiPaused.expose("Speed.Pause", false, false);
        capiPaused.setChangeDelegate(
            delegate (Boolean value, ChangedBy changedBy)
            {
                if (changedBy == ChangedBy.AELP)
                {
                    Sim.Settings.Paused = value;
                }
            }
        );

        capiSpeedTime = new SimCapiNumber((float)Sim.Settings.Speed);
        capiSpeedTime.expose("Speed.Time", false, false);
        capiSpeedTime.setChangeDelegate(
            delegate (float value, ChangedBy changedBy)
            {
                // Debug.Log("Speed.Time " + value + " " + changedBy);
                // Any changes done by the SIM go through the Body system first, which updates the Exposed Data.
                if (changedBy == ChangedBy.AELP)
                { 
                    Sim.Settings.Speed = value * (int)capiSpeedRatio.getValue();
                }

            }
        );

        capiSpeedRatio = new SimCapiEnum<SpeedRatio>(SpeedRatio.Second);
        capiSpeedRatio.expose("Speed.Ratio", false, false);
        capiSpeedRatio.setChangeDelegate(
            delegate (SpeedRatio value, ChangedBy changedBy)
            {
                if (changedBy == ChangedBy.AELP)
                {
                    Sim.Settings.Speed = capiSpeedTime.getValue() * (int)value;
                }
            }
        );

        capiFocused = new SimCapiString("");
        capiFocused.expose("Focused", false, false);
        capiFocused.setChangeDelegate(
            delegate (String value, ChangedBy changedBy)
            {
                if (changedBy == ChangedBy.AELP)
                {
                    capiFocused.setValue(value);
                }
            }
        );
    }

    internal void BodyUpdate(PhysicsBody physicsBody)
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// Adds and exposes a state to the prior state list.
    /// </summary>
    /// <param name="state"></param>
    public void addPriorState(string state)
    {
        if (StartState.getValue() == "")
        {
            StartState.setValue(state);
        }
        SimCapiString capi = new SimCapiString("");
        capi.expose("State.Prior." + PriorStates.Count, false, false);
        capi.setValue(state);
    }
}
