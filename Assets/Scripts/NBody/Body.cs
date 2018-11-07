using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public enum BodyType
{
    Unclassified, Sun, Planet, Moon, Astroid
}

public class Body
{
    // ** Properites ** //

    public string Name { get; set; }
    public BodyType Type { get; set; }
    /// <summary>
    /// Primary orbital body or point. May be null.
    /// </summary>
    public Orbit Orbits { get; set; }
    /// <summary>
    /// Mass in kg.
    /// </summary>
    public double Mass { get; set; }
    /// <summary>
    /// Average radius of the body itself (not orbital radius).
    /// </summary>
    public double Radius { get; set; }
    /// <summary>
    /// Time in days that it takes to make a full revoltion.
    /// </summary>
    public double Rotation { get; set; }
    /// <summary>
    /// Individual Layers, in order from the outside in. Atmosphere, Crust, Mandle, etc.  May contain
    /// limited information and simplified layers based on current knowledge. 
    /// May be null for completly unknown planets.
    /// </summary>
    public List<Layer> Layers { get; private set; }

    /// <summary>
    /// Creates a unclassifed, unnammed body with no orbit, mass, rotation, or layers.
    /// </summary>
    public Body() : this("", BodyType.Unclassified, null, 0.0, 0.0, 0.0, null) { }

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
    public Body(string name, BodyType type, Orbit orbits, double mass, double radius, double rotation, List<Layer> layers)
    {
        if (name == null)
        {
            throw new ArgumentNullException("Name not found.");
        }
        Name = name;
        Type = type;
        Orbits = orbits;
        Mass = mass;
        Radius = radius;
        Rotation = rotation;
        Layers = layers;
    }

    /// <summary>
    /// Gets the Body's primary position based on it's orbit and current time.
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

}
