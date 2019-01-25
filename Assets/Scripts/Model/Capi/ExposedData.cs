using Planets;
using SimCapi;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// Handles top-level data exposed to SimCapi.
/// </summary>
public class ExposedData {


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
    /// Current simulation time in days from starting point.
    /// </summary>
    public SimCapiNumber Time;

    /// <summary>
    /// Body that currently has focus. Empty string ("") means no body is currently selected (free camera).
    /// </summary>
    public SimCapiString FocusedBody;

    public SimCapiStringArray Perms;

    private int priorCount = NBody.MAX_BODY_COUNT;

    // Permission switch for "create" perm.
    public SimCapiBoolean CanCreate;

    public SimCapiBoolean ReadOnlyTest;
    public SimCapiBoolean WriteOnlyTest;

    /// <summary>
    /// Sets initial values.
    /// </summary>
    public ExposedData() {
        Time = new SimCapiNumber(0F);
        FocusedBody = new SimCapiString("");
        CanCreate = new SimCapiBoolean(false);
        Perms = new SimCapiStringArray();
    }

    /// <summary>
    /// Exposes all data. Called by the Main processing class after instance is created.
    /// </summary>
    public void exposeAll()
    {
        Time.expose("Time", false, false);
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

        Time.setChangeDelegate(
            delegate (float value, ChangedBy changedBy)
            {
                // Any changes done by the SIM go through the NBody system first, which updates the Exposed Data.
                if (changedBy == ChangedBy.SIM)
                {
                    // Not further effects here at this time.
                }
                // If the Changes were done by the ALEP, the NBody system needs to be updated accordingly.               
                else
                {
                    NBody.Time = value;
                }
                Time.setValue(value);

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
                   NBody.Focused = value;
               }
               FocusedBody.setValue(value);
           }
       );
    }


}
