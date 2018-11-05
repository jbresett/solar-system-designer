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

    /// <summary>
    /// Creates an unclassifed massless body with no orbit.
    /// </summary>
    public Body() : this("", BodyType.Unclassified, null, new Vector3d(), 0.0, 0.0) { }

    /// <summary>
    /// Creates a new Body with no layer details.
    /// </summary>
    /// <param name="name">Body Name</param>
    /// <param name="type">BodyType: Unclassified, Sun, Planet, Moon, or Astroid</param>
    /// <param name="orbit">Body orbiting.</param>
    /// <param name="ellipse">Major Axis, Minor Axis, and Incline of orbit.</param>
    /// <param name="revolution">Revolution time (in days).</param>
    public Body(string name, BodyType type, Body orbit, Vector3d ellipse, double revolution, double rotation)
    {
        if (string.IsNullOrEmpty(name))
        {
            throw new ArgumentException("Name is null.");
        }

        if (ellipse == null)
        {
            throw new ArgumentNullException("Ellipse is null.");
        }

        Name = name;
        Type = type;
        Orbit = orbit;
        Ellipse = ellipse;
        Revolution = revolution;
        Rotation = rotation;
        Layers = new List<Layer>();
    }

    public string Name { get; set; }
    public BodyType Type { get; set; }
    public Body Orbit { get; set; }

    //TODO: Replase Vector3d with Ellipse Object once its created.
    private Vector3d ellipse;
    /// <summary>
    /// Major Length (x), Minor Length (y), and Incline (z) of the ellipse.
    /// </summary>
    public Vector3d Ellipse {
        get
        {
            return ellipse;
        }
        set
        {
            ellipse.x = value.x;
            ellipse.y = value.y;
            ellipse.z = value.z;
        }
    }

    /// <summary>
    /// Mass in kg.
    /// </summary>
    public double Mass { get; set; }

    /// <summary>
    /// Average radius of the body itself (not orbital radius).
    /// </summary>
    public double Radius { get; set; }

    /// <summary>
    /// Time in days that it takes the body to make one full revolution around the body it orbits.
    /// </summary>
    public double Revolution { get; set; }

    /// <summary>
    /// Time in days that it takes to make a full revoltion.
    /// </summary>
    public double Rotation { get; set; }

    /// <summary>
    /// Individual Layers, in order from the outside in. Atmosphere, Crust, Mandle, etc.  May contain
    /// limited information and simplified layers based on current knowledge. 
    /// </summary>
    public List<Layer> Layers { get; private set; }

    /// <summary>
    /// Retrives the full composition of the planet. Any changes needs to be made at the individual layer. 
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
