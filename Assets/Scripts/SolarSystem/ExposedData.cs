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
    /// Stores a List of all Bodies in JSON Format.
    /// Example: {"Name":"Earth","Type":2,
    ///     "Orbits":{"ParentName":"Sun","Shape":{"majorAxis":156.0,"minorAxis":146.0},"Revolution":365.0},
    ///     "Mass":5.972e24,"Radius":6371.0,"Rotation":1.0}
    ///   Sets the earth circling around the sun in an eliptical orbit btween 146 and 156
    ///   kilometers (no inclination), with a mass of 5.972*10^24 kg, a radius of 6371 kilometers, 
    ///   and a composition of primarally Oxygen, SIlicon, Aliminum, and Calcium (based on %).
    ///   
    /// Note: NBodies.updateValue() must be called after making changes to the List from getList.
    /// </summary>
    public SimCapiStringArray NBodies;

    /// <summary>
    /// Current simulation time in days from starting point.
    /// </summary>
    public SimCapiNumber Time;

    /// <summary>
    /// Body that currently has focus. Empty string ("") means no body is currently selected (free camera).
    /// </summary>
    public SimCapiString FocusedBody;

    /// <summary>
    /// Sets initial values.
    /// </summary>
    public ExposedData() {
        NBodies = new SimCapiStringArray();
        Time = new SimCapiNumber(0F);
        FocusedBody = new SimCapiString("");
    }

    /// <summary>
    /// Exposes all data. Called by the Main processing class after instance is created.
    /// </summary>
    public void expose()
    {
        NBodies.expose("N Body", false, false);
        Time.expose("Time", false, false);
        FocusedBody.expose("Focused", false, false);
    }

    /// <summary>
    /// Sets all the deligates to handle updates to the Exposed data.
    /// 
    /// Changes done through the SIM generally update the NBody classes automatically.
    /// Changes done through ALEP need to be reflected into the NBody system.
    /// </summary>
    public void setDeligates()
    {

        NBodies.setChangeDelegate(
            delegate (string[] values, ChangedBy changedBy)
            {
                // Any changes done by the SIM go through the NBody system first, which updates the Exposed Data.
                if (changedBy == ChangedBy.SIM)
                {
                    // Not further effects here at this time.
                }
                else
                {
                    // TODO: Part of US #43
                }
            }
        );

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
                    // TODO: Part of US #43
                }

            }
        );

        FocusedBody.setChangeDelegate(
           delegate (String value, ChangedBy changedBy)
           {
               // Any changes done by the SIM go through the NBody system first, which updates the Exposed Data.               if (changedBy == ChangedBy.SIM) return;
               if (changedBy == ChangedBy.SIM)
               {
                   // Not further effects here at this time.
               }
               // If the Changes were done by the ALEP, the NBody system needs to be updated accordingly.               
               else
               {
                   // TODO: Part of US #43
               }
           }
       );
    }


}
