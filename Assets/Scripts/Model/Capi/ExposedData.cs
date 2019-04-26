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
    /// Sets initial values.
    /// </summary>
    public void Init() {
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

    }

    internal void BodyUpdate(PhysicsBody physicsBody)
    {
        throw new NotImplementedException();
    }


}
