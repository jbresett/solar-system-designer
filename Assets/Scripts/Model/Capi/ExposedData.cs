using Planets;
using SimCapi;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Extensions;


/// <summary>
/// Handles top-level data exposed to SimCapi.
/// </summary>
public class ExposedData {
    public const int MAX_BODY_COUNT = 50;

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
    /// Stores a List of all Bodies.
    /// Each is stored in an array: Name, Type, Mass, Radius, Initial Position, 
    /// Example: [Earth, Planet, 5.972e24, 6371.0 kg, 1 AU]
    /// Note: NBodies.updateValue() must be called after making changes to the List from getList.
    /// </summary>
    public SimCapiStringArray[] Bodies;

    /// <summary>
    /// Current simulation time in days from starting point.
    /// </summary>
    public SimCapiNumber Time;

    /// <summary>
    /// Body that currently has focus. Empty string ("") means no body is currently selected (free camera).
    /// </summary>
    public SimCapiString FocusedBody;

    public SimCapiStringArray Perms;

    // Permission switch for "create" perm.
    public SimCapiBoolean CanCreate;

    /// <summary>
    /// Sets initial values.
    /// </summary>
    public ExposedData() {
        Time = new SimCapiNumber(0F);
        FocusedBody = new SimCapiString("");

        CanCreate = new SimCapiBoolean(false);
        Perms = new SimCapiStringArray();
        for (int i = 0; i < MAX_BODY_COUNT; i++)
        {
            Bodies[i] = new SimCapiStringArray();
        }
    }

    /// <summary>
    /// Exposes all data. Called by the Main processing class after instance is created.
    /// </summary>
    public void expose()
    {
        Time.expose("Time", false, false);
        FocusedBody.expose("Focused", false, false);
        CanCreate.expose("Can Create", false, false);
        Perms.expose("Perm", true, false);
        for (int i = 0; i < MAX_BODY_COUNT; i++)
        {
            Bodies[i].expose("Body [{0:D2}]".Format(i), false, false);
        }
    }

    /// <summary>
    /// Returns a body's index from the NBodies array, based on the body's name.
    /// </summary>
    /// <param name="name"></param>
    /// <returns>Index, or -1 if not found.</returns>
    public int GetBodyIndex(string name)
    {

        // Search through NBodies List.
        for (int i = 0; i < MAX_BODY_COUNT; i++)
        {
            SimCapiStringArray body = Bodies[i];
            if (body.getList().Count > 0 && body.getList()[0] == name)
            {
                return i;
            }
        }
        return -1;
    }

    /// <summary>
    /// Returns a body based on the body's name.
    /// </summary>
    /// <param name="name"></param>
    /// <returns>Body, or null if not found.</returns>
    public SimCapiStringArray GetBody(string name)
    {

        // Search through NBodies List.
        foreach (SimCapiStringArray body in Bodies)
        {
            if (body.getList().Count > 0 && body.getList()[0] == name)
            {
                return body;
            }
        }
        return null;
    }


    /// <summary>
    /// Update the SimCapiStringArray NBodies once a change to the Body has been invoked.
    /// 
    /// Called by: Body.UpdateCapi
    /// </summary>
    /// <param name="body"></param>
    /// <returns></returns>
    public bool BodyUpdate(OrbitalBody body)
    {
        SimCapiStringArray Ary = GetBody(body.Name);
        if (Ary == null) return false;
        Ary.getList().Clear();
        Ary.getList().Add(body.Name);
        Ary.getList().Add(body.Type);
        Ary.getList().Add(body.Mass.ToString());
        Ary.updateValue();
        return true;
    }

    /// <summary>
    /// Sets all the deligates to handle updates to the Exposed data.
    /// 
    /// Changes done through the SIM generally update the NBody classes automatically.
    /// Changes done through ALEP need to be reflected into the NBody system.
    /// </summary>
    public void setDeligates()
    {
        foreach (SimCapiStringArray Body in Bodies)
        {

            Body.setChangeDelegate(
                delegate (string[] values, ChangedBy changedBy)
                {
                    // Any changes done by the SIM go through the NBody system first, which updates the Exposed Data.
                    if (changedBy == ChangedBy.SIM)
                    {
                        // Not further effects here at this time.
                    }
                    else
                    {
                        // Cycle through each body
                        for (int i = 0; i < values.Length; i++)
                        {

                            // Get the body's name.
                            string name = Body.getList()[0];

                            // If Body is shown visually (exists in NBody system), overwrite from json string.
                            if (Main.Instance.Bodies.ContainsKey(name))
                            {
                                // [TODO] Update for Physics Body.
                            }
                        }
                    }
                }
            );
        }

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
                    Main.Instance.Bodies.Time = value;
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
                   Main.Instance.Bodies.Focused = value;
               }
           }
       );
    }


}
