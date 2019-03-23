using Planets;
using SimCapi;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// Handles top-level data exposed to SimCapi.
/// </summary>
public class ExposedData: Singleton<ExposedData> {


    /* Data Types:
     * SimCapi.SimCapiNumber
     * SimCapi.SimCapiString
     * SimCapi.SimCapiStringArray
     * SimCapi.SimCapiBoolean
     * SimCapi.SimCapiEnum<T>
     * SimCapi.SimCapiMathExpression
     * SimCapi.SimCapiPointArray
     */

    /// <summary>
    /// Current simulation speed.
    /// </summary>
    public SimCapiNumber capiSpeed;
    public SimCapiEnum<SpeedRatios> speedRatio;
    public SimCapiBoolean capiPaused;

    /// <summary>
    /// Body that currently has focus. Empty string ("") means no body is currently selected (free camera).
    /// </summary>
    public SimCapiString capiFocused;

    public SimCapiStringArray capiPerms;

    public SimCapiStringArray capiEvents;

    /// <summary>
    /// Sets initial values.
    /// </summary>
    public void Init() {
        capiSpeed = new SimCapiNumber((float)Sim.Settings.Speed);
        capiSpeed.expose("Speed", false, false);
        capiSpeed.setChangeDelegate(
            delegate (float value, ChangedBy changedBy)
            {
                // Any changes done by the SIM go through the Body system first, which updates the Exposed Data.
                if (changedBy == ChangedBy.AELP)
                { 
                    Sim.Settings.Speed = value;
                }

                // Update Speed Ratio to matching value.
                SpeedRatios ratio = SpeedRatios.Custom;
                foreach (SpeedRatios r in Enum.GetValues(typeof(SpeedRatios)))
                {
                    if ((int)r == (int)value)
                    {
                        ratio = r;
                    }
                }
                speedRatio.setValue(ratio);
            }
        );

        speedRatio = new SimCapiEnum<SpeedRatios>(SpeedRatios.Stop);
        speedRatio.expose("Speeds", false, false);
        speedRatio.setChangeDelegate(
            delegate (SpeedRatios value, ChangedBy changedBy)
            {
                if (changedBy == ChangedBy.AELP)
                {
                    Sim.Settings.Speed = (int)value;
                }
            }
        );

        capiPaused = new SimCapiBoolean(Sim.Settings.Paused);
        capiPaused.expose("Paused", false, false);
        capiPaused.setChangeDelegate(
            delegate (Boolean value, ChangedBy changedBy)
            {
                if (changedBy == ChangedBy.AELP)
                {
                    Sim.Settings.Paused = value;
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

        capiPerms = new SimCapiStringArray();
        capiPerms.expose("Perm", false, false);

        capiEvents = new SimCapiStringArray();
        capiEvents.expose("Events", false, false);
    }

    internal void BodyUpdate(PhysicsBody physicsBody)
    {
        throw new NotImplementedException();
    }

}
