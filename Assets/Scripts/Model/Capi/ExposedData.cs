using Newtonsoft.Json.Linq;
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

    public SimCapiStringArray Perms;

    /// <summary>
    /// Sets initial values.
    /// </summary>
    public ExposedData() {
        NBodies = new SimCapiStringArray();
        Time = new SimCapiNumber(0F);
        FocusedBody = new SimCapiString("");
        Perms = new SimCapiStringArray();
    }

    /// <summary>
    /// Returns a body's index from the NBodies array, based on the body's name.
    /// </summary>
    /// <param name="name"></param>
    /// <returns>Index, or -1 if not found.</returns>
    public int GetBodyIndex(string name)
    {

        // Search through NBodies List.
        List<String> Lines = NBodies.getList();
        for (int i = 0; i < Lines.Count; i++)
        {

            // Parse each item in json, finding by name.
            JObject obj = JObject.Parse(Lines[i]);
            if (name == (string)obj["name"])
            {
                return i;
            } 
        }
        return -1;
    }

    /// <summary>
    /// Exposes all data. Called by the Main processing class after instance is created.
    /// </summary>
    public void expose()
    {
        NBodies.expose("N Body", false, false);
        Time.expose("Time", false, false);
        FocusedBody.expose("Focused", false, false);
        Perms.expose("Perm", false, false);
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
        int index = GetBodyIndex(body.Name);
        
        // If planet is not being tracked by exposed data, ignore changes.
        if (index == -1) return false;

        // Update Capi
        List<string> bodies = NBodies.getList();
        bodies[index] = JsonUtility.ToJson(body);
        NBodies.updateValue();
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
                    // Cycle through each body
                    for (int i = 0; i < values.Length; i++)
                    {

                        // Get the body's name.
                        JObject obj = JObject.Parse(values[i]);
                        string name = (String)obj["name"]; 

                        // If Body is shown visually (exists in NBody system), overwrite from json string.
                        if (Main.Instance.Bodies.ContainsKey(name))
                        {
                            JsonUtility.FromJsonOverwrite(values[i], Main.Instance.Bodies[name]);
                        }
                    }
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
