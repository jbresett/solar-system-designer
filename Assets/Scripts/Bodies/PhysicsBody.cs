using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PhysicsBody : CapiBody {

    private const double KG_MASS_CONVERT = 5.9722E24; // Earth's Mass in KG.
    private const double M_TO_AU = 1.496E11; //AU represented in meters.

    /// <summary>
    /// This method is used by the Gravity class to get and set the
    /// celestial object's position.
    /// 
    /// </summary>
    public Vector3d Pos {

            get { return position * M_TO_AU; }
            set { Position = value / M_TO_AU; }

    }

    /// <summary>
    /// Mass of the Body in kg, which is needed for
    /// physics calculations within the Gravity class.
    /// </summary>
    public double KG
    {
        get { return mass * KG_MASS_CONVERT; }
        set { Mass = value / KG_MASS_CONVERT; }
    }
    
    /// <summary>
    /// This Method gets and sets the velocity for the Gravity
    /// Class.
    /// </summary>
    public Vector3d Vel
    {
        get { return velocity; }
        set {Velocity = value; }  
    }
    
    /// <summary>
    /// This method stores the total force that is applied
    /// to a particular body.
    /// </summary>
    public Vector3d totalForce
    {
        get { return force; }
        set { force = value; }  
    }

    [SerializeField]
    protected Vector3d force;
    
    public Body MostPull
    {
        get { return mostPull; }
        set { mostPull = value; }  
    }

    [SerializeField]
    protected Body mostPull;
    
    
    /// <summary>
    /// This method checks to see if initial velocity has been
    /// set or not.  This is so that the gravity class can calculate
    /// an initial velocity if a new body is added to the simulation.
    /// </summary>
    public bool isInitialVel
    {
        get { return initialVelocity; }
        set { initialVelocity = value; }  
    }

    [SerializeField] protected bool initialVelocity;
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

    public BodyType whatAmI()
    {
        //todo Replace with proper classification system.
        
        //Hack to get star/nonstar differentiation for demo, REPLACE.
        BodyMaterial[] starTypeBodyMaterials =
        {
            BodyMaterial.Star_Blue, BodyMaterial.Star_Orange, BodyMaterial.Star_Red, BodyMaterial.Star_White,
            BodyMaterial.Star_Yellow
        };
        return starTypeBodyMaterials.Contains(material) ? BodyType.Star : BodyType.Planet;
    }

}
