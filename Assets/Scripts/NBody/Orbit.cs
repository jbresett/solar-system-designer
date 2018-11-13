using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Creates an simple orbit around a body using a basic ellipse around a parent object.
/// Simplified model: Constant speed over time, no barycenter calculations.
/// </summary>
[Serializable]
public class Orbit {

    [SerializeField]
    private string ParentName;

    private Body _parent;
    public Body Parent {
        get { return _parent; }
        set
        {
            _parent = value;
            ParentName = (value == null ? "" : value.Name);
        }
    }
    public Ellipse Shape;
    public double Revolution;

    public string GetParentName()
    {
        return ParentName;
    }

    /// <summary>
    /// Generates orbit around a body, given a shape and revolution Time.
    /// </summary>
    /// <param name="parent">Object's parent.</param>
    /// <param name="shape">Ellipse shape.</param>
    /// <param name="revolutionTime">Time in Days for a full revolution.</param>
    public Orbit(Body parent, Ellipse shape, double revolutionTime)
    {
        if (parent == null)
        {
            throw new ArgumentException("Around is null.");
        }

        Parent = parent;
        Shape = shape;
        Revolution = revolutionTime;
    }

    public Orbit(string orbitString)
    {
    }

    /// <summary>
    /// Get a orbit's position relative to it's parent based on the time.
    /// </summary>
    /// <param name="days"></param>
    /// <returns></returns>
    public Vector3d getPosition(double days)
    {
        // Get theta and return the calculated point.
        double degree = getDegree(days);
        return Shape.calcPoint(degree);
    }

    /// <summary>
    /// Calculates radian based on time in orbit.
    /// </summary>
    /// <param name="days">Number of days from initial starting position.</param>
    public double getRadian(double days)
    {
        // Gets # of turns based on time of a single revolution.
        double turns = days / Revolution;

        // Percent of Turn
        double turn = turns - Math.Truncate(turns);

        return turn * 2 * Math.PI;
    }

    /// <summary>
    /// Calculates degrees based on time in orbit.
    /// </summary>
    /// <param name="days">Number of days from initial starting position.</param>
    public double getDegree(double days)
    {
        // Gets # of turns based on time of a single revolution.
        double turns = days / Revolution;

        // Percent of Turn
        double turn = turns - Math.Truncate(turns);

        // 1 Full turn = 2pi
        return turn * 360;
    }

    public override bool Equals(object obj)
    {
        var orbit = obj as Orbit;
        return orbit != null &&
               EqualityComparer<Body>.Default.Equals(Parent, orbit.Parent) &&
               EqualityComparer<Ellipse>.Default.Equals(Shape, orbit.Shape) &&
               Revolution == orbit.Revolution;
    }

    public override int GetHashCode()
    {
        var hashCode = -6172727;
        hashCode = hashCode * -1521134295 + EqualityComparer<Body>.Default.GetHashCode(Parent);
        hashCode = hashCode * -1521134295 + EqualityComparer<Ellipse>.Default.GetHashCode(Shape);
        hashCode = hashCode * -1521134295 + Revolution.GetHashCode();
        return hashCode;
    }
}
