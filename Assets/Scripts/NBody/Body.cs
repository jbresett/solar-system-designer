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

    // ** Properites ** //
    public NBody System { get; set; }
    public string Name;
    public BodyType Type;
    /// <summary>
    /// Primary orbital body or point. May be null.
    /// </summary>
    public Orbit Orbits;
    /// <summary>
    /// Mass in kg.
    /// </summary>
    public double Mass;
    /// <summary>
    /// Average radius of the body itself (not orbital radius).
    /// </summary>
    public double Radius;
    /// <summary>
    /// Time in days that it takes to make a full revoltion.
    /// Use positive #'s for clockwise, negative for counterclockwise.
    /// </summary>
    public double Rotation;
    /// <summary>
    /// Individual Layers, in order from the outside in. Atmosphere, Crust, Mandle, etc.  May contain
    /// limited information and simplified layers based on current knowledge. 
    /// May be null for completly unknown planets.
    /// </summary>
    public List<Layer> Layers;

    /// <summary>
    /// Creates a unclassifed, unnammed body with no orbit, mass, rotation, or layers.
    /// </summary>
    public Body(NBody system) : this(system, "", BodyType.Unclassified, null, 0.0, 0.0, 0.0, null) { }

    /// <summary>
    /// Creates a Body from a json string.
    /// </summary>
    public Body(NBody system, String json) : this(system, "", BodyType.Unclassified, null, 0.0, 0.0, 0.0, null)
    {
        JsonUtility.FromJsonOverwrite(json, this);

        // Serailization does not include orbiting planet object. Find based on name and update accordingly.
        // TODO: Under construction.
        
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
    public Body(NBody system, string name, BodyType type, Orbit orbits, double mass, double radius, double rotation, List<Layer> layers)
    {
        if (system == null)
        {
            throw new ArgumentNullException("System needs to be included.");
        }
        System = system;
        System.Bodies.Add(this);

        if (name == null)
        {
            throw new ArgumentNullException("Name not found.");
        }
        Name = name;
        Type = type;
        Orbits = orbits;
        if (mass < 0.0)
        {
            throw new ArgumentNullException("Must have 0 or positive mass.");
        }
        Mass = mass;
        Radius = radius;
        if (mass < 0.0)
        {
            throw new ArgumentNullException("Must have 0 or positive radius.");
        }
        Rotation = rotation;
        Layers = layers;
    }

    /// <summary>
    /// Gets the Body's realative position based on it's orbit and current time.
    /// Returns (0,0,0) if the body is not orbiting another body.
    /// </summary>
    /// <param name="days"></param>
    public Vector3d getPosition(double days)
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
    public double getDistance(Body from, double days) 
    {
        Vector3d distance = getPosition(days).subtract(from.getPosition(days));
        return distance.magnatude;
    }

    /// <summary>
    /// Retrives the full composition of the planet. 
    /// Read-only: Any changes needs to be made at the individual layer(s). 
    /// </summary>
    public Dictionary<Compound, double> getComposition()
    {
        Dictionary<Compound, double> result = new Dictionary<Compound, double>();
        foreach (Layer layer in Layers)
        {
            foreach (Compound compound in layer.Composition.Keys)
            {
                // Adds the new compound ammounts to the current one.
                result.Add(compound, layer.Composition[compound] +
                    (result.ContainsKey(compound) ? result[compound] : 0)
                    );
            }
        }
        return result;
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
