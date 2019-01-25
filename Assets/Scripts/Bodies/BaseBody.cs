using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseBody : MonoBehaviour {

    /// <summary>
    /// Body Type.
    /// </summary>
    public BodyType Type
    {
        get { return type; }
        set { type = value; }
    }
    [SerializeField]
    protected BodyType type;

    /// <summary>
    /// Unique Body Name.
    /// </summary>
    public string Name
    {
        get { return name; }
        set { name = value; }
    }
    //[SerializeField] "name" not needed: Part of the MonoBehavior.

    /// <summary>
    /// Mass in Earths.
    /// </summary>
    public double Mass
    {
        get { return mass; }
        set
        {
            if (value < 0) Debugger.log("Body '{0}' set to negative mass.".Format());
            mass = value;
        }
    }
    [SerializeField]
    protected double mass;

    /// <summary>
    /// Diameter in Earths.
    /// </summary>
    public double Diameter
    {
        get { return diameter; }
        set { diameter = value; }
    }
    [SerializeField]
    protected double diameter;

    /// <summary>
    /// Current Position in AUs.
    /// </summary>
    public Vector3d Position
    {
        get { return position; }
        set { position = value; }
    }
    protected Vector3d position;

    /// <summary>
    /// Initial position in AUs.
    /// </summary>
    public Vector3d InitialPosition
    {
        get { return initialPosition; }
        set { initialPosition = value; }
    }
    protected Vector3d initialPosition;

    /// <summary>
    /// Rotation in Earth Days.
    /// </summary>
    public double Rotation
    {
        get { return rotation; }
        set { rotation = value; }
    }
    [SerializeField]
    protected double rotation;

    /// <summary>
    /// Sum of all vectors on the object.
    /// </summary>
    public Vector3d Velocity
    {
        get { return velocity; }
        set { velocity = value; }
    }
    [SerializeField]
    protected Vector3d velocity;

    public void Awake()
    {

    }

    public void Start()
    {

    }

    public void Update()
    {

    }
}
