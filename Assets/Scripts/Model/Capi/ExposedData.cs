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
    public SimCapiNumber Speed;
    public SimCapiEnum<SpeedRatios> SpeedRatio;

    /// <summary>
    /// Body that currently has focus. Empty string ("") means no body is currently selected (free camera).
    /// </summary>
    public SimCapiString FocusedBody;

    public SimCapiStringArray Perms;

    // Permission switch for "create" perm.
    public SimCapiBoolean CanCreate;

    public SimCapiBoolean ReadOnlyTest;
    public SimCapiBoolean WriteOnlyTest;

    /// <summary>
    /// Sets initial values.
    /// </summary>
    public ExposedData() {
        Speed = new SimCapiNumber(0F);
        SpeedRatio = new SimCapiEnum<SpeedRatios>(SpeedRatios.Stop);
        FocusedBody = new SimCapiString("");
        CanCreate = new SimCapiBoolean(false);
        Perms = new SimCapiStringArray();
    }

    /// <summary>
    /// Exposes all data. Called by the Main processing class after instance is created.
    /// </summary>
    public void exposeAll()
    {
        Speed.expose("Speed", false, false);
        SpeedRatio.expose("Speed ", false, false);
        FocusedBody.expose("Focused", false, false);
        CanCreate.expose("Can Create", false, false);
        Perms.expose("Perm", true, false);
    }

    internal void BodyUpdate(PhysicsBody physicsBody)
    {
        throw new NotImplementedException();
    }


    /// <summary>
    /// Sets all the deligates to handle updates to the Exposed data.
    /// 
    /// Changes done through the SIM generally update the NBody classes automatically.
    /// Changes done through ALEP need to be reflected into the NBody system.
    /// </summary>
    public void setDeligates()
    {
        Speed.setChangeDelegate(
            delegate (float value, ChangedBy changedBy)
            {
                // Any changes done by the SIM go through the Body system first, which updates the Exposed Data.
                if (changedBy == ChangedBy.SIM)
                {
                }
                // If the Changes were done by the ALEP, the NBody system needs to be updated accordingly.               
                else
                {
                    Sim.Settings.Speed = value;
                }

                // Update SpeedRatio
                SpeedRatio.setValue(SpeedRatios.Stop);

            }

        );

        SpeedRatio.setChangeDelegate(
            delegate (SpeedRatios value, ChangedBy changedBy)
            {
                // Any changes done by the SIM go through the Body system first, which updates the Exposed Data.
                if (changedBy == ChangedBy.SIM)
                {
                    // Not further effects here at this time.
                }
                // If the Changes were done by the ALEP, the NBody system needs to be updated accordingly.               
                else
                {
                    Sim.Settings.Speed = (int)value;
                }

            }

        );


        FocusedBody.setChangeDelegate(
           delegate (String value, ChangedBy changedBy)
           {
               // Any changes done by the SIM go through the NBody system first, which updates the Exposed Data.
               if (changedBy == ChangedBy.SIM)
               {
                   // Not further effects here at this time.
               }
               // If the Changes were done by the ALEP, the NBody system needs to be updated accordingly.               
               else
               {
                   // [TODO] Update to new focus
                   // NBody.Focused = value;
               }
               FocusedBody.setValue(value);
           }
       );
    }


}
