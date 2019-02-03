using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicsBody : CapiBody {

    private static double SOLAR_MASS_CONVERT = 334672.021419; // Solar Mass in Earths.
    private static double KG_MASS_CONVERT = 5.9722E24; // Earth's Mass in KG.

    /// <summary>
    /// Mass of the Body in Sols.
    /// </summary>
    public double SolarMass
    {
        get { return mass / SOLAR_MASS_CONVERT; }
        set { Mass = value / SOLAR_MASS_CONVERT; }
    }
    /// <summary>
    /// Mass of the Body in kg.
    /// </summary>
    public double KG
    {
        get { return mass * KG_MASS_CONVERT; }
        set { Mass = value / KG_MASS_CONVERT; }
    }
    
    /// <summary>
    /// Set and get initial velocity for the body
    /// </summary>
    public Vector3d initialVelocity
    {
        get { return initialVel; }
        set { initialVel = value; }
    }
    
    [SerializeField]
    protected Vector3d initialVel;
    public Vector3d momentumVector
    {
        get { return momentum; }
        set { momentum = value; }  
    }

    [SerializeField]
    protected Vector3d momentum;

    new public void Awake()
    {
        base.Awake();
        // Can delete function if not needed in this class: next level down will automatically be called instead.
    }

    new public void Start () {
        base.Start();
        // Can delete function if not needed in this class: next level down will automatically be called instead.
    }

    // Update is called once per frame
    new public void Update () {
        base.Update();
	}

    /// <summary>
    /// Gets the Barycenter point between this object and another.
    /// <param name="withBody"></param>
    /// <returns></returns>
    public Vector3d GetBaycenter(PhysicsBody withBody)
    {
        // distance to BaryCenter.
        double baryDistance = (Vector3d.Distance(position, withBody.Position) * withBody.Mass) / (Mass + withBody.Mass);

        return Vector3d.LerpUnclamped(position, withBody.Position, baryDistance);
    }

}
