using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public enum BodyType
{
    Unclassified, Sun, Planet, Moon, Astroid
}

[Serializable]
public class Body
{
    public NBody System { get; set; }

    /// <summary>
    /// Planet Name. Must be non-null and unique for it's system.
    /// </summary>
    public string Name
    {
        get { return name; }
        set {
            if (value == null) throw new InvalidOperationException("Planet name must not be null.");
            if (System.Bodies.ContainsKey(value)) throw new InvalidOperationException("Planet already exists in system with same name.");
            name = value;
        }
    }
    [SerializeField]
    private String name;

    public BodyType Type
    {
        get { return type; }
        set { type = value; }
    }
    [SerializeField]
    private BodyType type;

    /// <summary>
    /// Primary orbital body or point. May be null.
    /// </summary>
    public Orbit Orbits
    {
        get { return orbits; }
        set { orbits = value; }
    }
    [SerializeField]
    private Orbit orbits;

    /// <summary>
    /// Mass in kg.
    /// </summary>
    public double Mass
    {
        get { return mass; }
        set {
            if (value < 0.0) throw new InvalidOperationException("Mass can not be negative.");
            mass = value;
        }
    }
    [SerializeField]
    private double mass;

    /// <summary>
    /// Average radius of the body itself (not orbital radius).
    /// </summary>
    public double Radius {
        get { return radius; }
        set {
            if (value < 0.0) throw new InvalidOperationException("Radius can not be negative.");
            radius = value;
        }
    }
    [SerializeField]
    private double radius;

    /// <summary>
    /// Time in days that it takes to make a full revoltion.
    /// Use positive #'s for clockwise, negative for counterclockwise.
    /// </summary>
    public double Rotation
    {
        get { return rotation; }
        set { rotation = value; }
    }
    [SerializeField]
    private double rotation;

    /// <summary>
    /// Creates a unclassifed, unnammed body with no orbit, mass, rotation, or layers.
    /// </summary>
    public Body(NBody system) : this(system, "", BodyType.Unclassified, null, 0.0, 0.0, 0.0) { }

    /// <summary>
    /// Creates a Body from a json string.
    /// </summary>
    public Body(NBody system, String json) : this(system, "", BodyType.Unclassified, null, 0.0, 0.0, 0.0)
    {
        JsonUtility.FromJsonOverwrite(json, this); 
    }
    /// <summary>
    /// Creates a body.
    /// </summary>
    /// <param name="name">Body Name.</param>
    /// <param name="type">Body Classification</param>
    /// <param name="orbits">Primary orbital body or point. May be null.</param>
    /// <param name="mass">Mass in kg.</param>
    /// <param name="radius">Average radius from center of the plane to the outer crust.</param>
    /// <param name="rotation">Time (in days) for the planet to make a full rotation.</param>
    /// <param name="layers">Body composition of each layer.  May be null.</param>
    public Body(NBody system, string name, BodyType type, Orbit orbits, double mass, double radius, double rotation)
    {
        System = system;
        Name = name;
        Type = type;
        Orbits = orbits;
        Mass = mass;
        Radius = radius;
        Rotation = rotation;

        System.Bodies.Add(name, this);
    }

    /// <summary>
    /// Gets the Body's realative position based on it's orbit and current time.
    /// Returns (0,0,0) if the body is not orbiting another body.
    /// </summary>
    /// <param name="days"></param>
    public Vector3d GetPosition(double days)
    {
        if (Orbits == null)
        {
            return new Vector3d();
        }
        return Orbits.getPosition(days);
    }

    /// <summary>
    /// Get Distance of any two vectors at N days.
    /// </summary>
    /// <param name="from"></param>
    /// <param name="days"></param>
    /// <returns></returns>
    public double GetDistance(Body from, double days) 
    {
        Vector3d distance = GetPosition(days).subtract(from.GetPosition(days));
        return distance.magnatude;
    }

    /// <summary>
    /// Returns a serialized version of this object.
    /// </summary>
    /// <returns></returns>
    public string ToJson()
    {
        return JsonUtility.ToJson(this);
    }

}
