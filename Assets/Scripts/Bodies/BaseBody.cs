using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BaseBody : MonoBehaviour {

    public int Id {
        get { return id; }
        set { id = value; }
    }
    [SerializeField]
    protected int id;

    public bool Active
    {
        get { return gameObject.activeSelf; }
        set
        {
            gameObject.SetActive(value);
            active = value;
        }
    }
    // Note: Active stored in gameObject.  Property below emulates the primitive for direct read-access.
    [SerializeField]
    protected bool active;

    /// <summary>
    /// Body Type.
    /// </summary>
    public BodyType Type
    {
        get { return type; }
        set { type = value; }
    }
    [SerializeField]
    protected BodyType type = BodyType.Undefined;

    /// <summary>
    /// Unique Body Name.
    /// </summary>
    public string Name
    {
        get { return gameObject.name; }
        set { gameObject.name = value; }
    }

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

    /// <summary>x
    /// Current Position in AUs.
    /// </summary>
    public Vector3d Position
    {
        get { return position; }
        set { position = value; }
    }
    [SerializeField]
    protected Vector3d position = new Vector3d();

    /// <summary>
    /// Initial position in AUs.
    /// </summary>
    public Vector3d InitialPosition
    {
        get { return initialPosition; }
        set { initialPosition = value; }
    }
    [SerializeField]
    protected Vector3d initialPosition = new Vector3d();

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
    protected Vector3d velocity = new Vector3d();

    public BodyMaterial Material
    {
        get { return material; }
        set { material = value; }
    }
    [SerializeField]
    protected BodyMaterial material = BodyMaterial.Sun;

    public void Awake()
    {
        // Leave in to ensure no issues with extended classes.

        // Set initial states.
        active = gameObject.activeSelf;
    }

    public void Start()
    {
        // Leave in to ensure no issues with extended classes.
    }

    public void Update()
    {
        // Leave in to ensure no issues with extended classes.
    }
}
