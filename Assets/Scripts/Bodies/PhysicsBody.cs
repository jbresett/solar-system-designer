using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicsBody : CapiBody {

    //private static double SOLAR_MASS_CONVERT = 334672.021419; // Solar Mass in Earths.
    private static double KG_MASS_CONVERT = 5.9722E24; // Earth's Mass in KG.
    private static double M_TO_AU = 1.496E11; //AU represented in meters
    public Vector3d Pos {

            get { return position * M_TO_AU; }
            set { position = value / M_TO_AU; }

    }

    /// <summary>
    /// Mass of the Body in kg.
    /// </summary>
    public double KG
    {
        get { return mass * KG_MASS_CONVERT; }
        set { Mass = value / KG_MASS_CONVERT; }
    }
    public Vector3d Vel
    {
        get { return velocity; }
        set {Velocity = value; }  
    }
    
    public Vector3d totalForce
    {
        get { return force; }
        set { force = value; }  
    }

    [SerializeField]
    protected Vector3d force;
    
    public bool isInitialVel
    {
        get { return initialVelocity; }
        set { initialVelocity = value; }  
    }

    [SerializeField]
    protected bool initialVelocity = false;
    new public void Awake()
    {
        base.Awake();
        // Can delete function if not needed in this class: next level down will automatically be called instead.
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
