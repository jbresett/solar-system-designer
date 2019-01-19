using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class is the rotation script for
/// the celestial objects, the class is 
/// used for demo creation and will be later
/// replaced with a script that rotates celestial
/// objects based on physics.
/// </summary>
public class Rotation : MonoBehaviour
{

    /// <summary>
    /// gets and sets the rotation speed
    /// </summary>
    public float RotSpd
    {
        get { return rotSpd; }
        set { rotSpd = value; }
    }
    [SerializeField]
    private float rotSpd;

    /// <summary>
    /// gets and sets the Dam amount
    /// </summary>
    public float DampAmt
    {
        get { return dampAmt; }
        set { dampAmt = value; }
    }
    [SerializeField]
    private float dampAmt;

    /// <summary>
    /// Updates once per frame
    /// </summary>
    void Update()
    {
        transform.Rotate((Vector3.up * RotSpd) * (Time.deltaTime * DampAmt), Space.Self);
        //transform.Rotate((Vector3.up * rotSpd) * (Time.deltaTime * dampAmt), Space.World);
        transform.RotateAround(Vector3.zero, transform.up, Time.deltaTime * DampAmt);
    }
}

