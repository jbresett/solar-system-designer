using SimCapi;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// SimCapiVector generates a SimCapiNumber for each axis to a related vector.
/// </summary>
public class SimCapiVector {

    // Name Suffixes for each axis.
    private static readonly string[] NAME_SUFFIX = { " X", " Y", " Z" };

    // Linked Vector
    private Vector3d vector;

    // CapiAxes variables.
    private SimCapiNumber[] capiAxis;

    /// <summary>
    /// Vector. Setting also updates all axis values.
    /// </summary>
    public Vector3d Vector
    {
        get { return vector; }
        set { setValue(value); }
    }

    /// <summary>
    /// Sets the value.
    /// </summary>
    /// <param name="value">Vector3d values</param>
    public void setValue(Vector3d value)
    {
        vector = value;
        capiAxis[0].setValue((float)value.x);
        capiAxis[1].setValue((float)value.y);
        capiAxis[2].setValue((float)value.z);
    }

    /// <summary>
    /// Generate a Capi vector with open access (neither read- nor write- only).
    /// </summary>
    /// <param name="name">Base Name. NAME_SUFFIX will be added to each axis.</param>
    /// <param name="vector">Linked Vector</param>
    public SimCapiVector(string name, Vector3d vector) : this(name, vector, false, false) { }

    /// <summary>
    /// Generate a Capi vector.
    /// </summary>
    /// <param name="name">Base Name. NAME_SUFFIX will be added to each axis.</param>
    /// <param name="vector">Linked Vector</param>
    /// <param name="readOnly">If exposed value is read-only.</param>
    /// <param name="writeOnly">If exposed value is write-only.</param>
    public SimCapiVector(string name, Vector3d vector, bool readOnly, bool writeOnly)
    {
        capiAxis = new SimCapiNumber[3];
        this.vector = vector;

        // Create, Expose, and initiate 
        for (int i = 0; i < 3; i++)
        {
            int axis = i;
            capiAxis[axis] = new SimCapiNumber((float)vector[axis]);
            capiAxis[axis].expose(name + NAME_SUFFIX[axis], readOnly, writeOnly);
            capiAxis[axis].setChangeDelegate(
                delegate (float value, SimCapi.ChangedBy changedBy)
                {
                    if (changedBy == ChangedBy.SIM) return;
                    vector[axis] = value;
                }
            );
        }
    }

}
