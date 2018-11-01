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

    // TODO: Replace Example Data with actual data during further integration sprint.

    // Name of all bodies in the current n-body system.
    private SimCapi.SimCapiStringArray IBodyNames;
    
    // Property used to translate SimCapi Data types to C# data types.
    // Not required, but improves ease-of-use.
    public List<String> BodyNames
    {
        get { return IBodyNames.getList(); }
        set
        {
            IBodyNames.setWithStringArray(value.ToArray());
            IBodyNames.updateValue();
        }
    }

    /// <summary>
    /// Sets initial values.
    /// </summary>
    public ExposedData() {
        IBodyNames = new SimCapi.SimCapiStringArray();
    }

    /// <summary>
    /// Exposes all data.
    /// </summary>
    public void expose()
    {
        IBodyNames.expose("Body Names", false, false);
    }


}
