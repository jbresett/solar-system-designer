using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

/// <summary>
/// Double preceision representation of vectors and points.
/// 
/// Partial code based on Unity's Vector3: https://github.com/Unity-Technologies/UnityCsReference/blob/master/Runtime/Export/Vector3.cs
/// </summary>
public class Vector3d
{

    // Point
    public const double kEpsilon = 0.00001F;
    public const double kEpsilonNormalSqrt = 1e-15F;

    // Standard vectors.
    static public Vector3d back { get { return new Vector3d(0, 0, -1); } }
    static public Vector3d down { get { return new Vector3d(0, -1, 0); } }
    static public Vector3d forward { get { return new Vector3d(0, 0, 1); } }
    static public Vector3d left { get { return new Vector3d(-1, 0, 0); } }
    static public Vector3d negativeInfinity { get { return new Vector3d(double.NegativeInfinity, double.NegativeInfinity, double.NegativeInfinity); } }
    static public Vector3d one { get { return new Vector3d(1, 1, 1); } }
    static public Vector3d positiveInfinity { get { return new Vector3d(double.PositiveInfinity, double.PositiveInfinity, double.PositiveInfinity); } }
    static public Vector3d right { get { return new Vector3d(1, 0, 0); } }
    static public Vector3d up { get { return new Vector3d(0, 1, 0); } }
    static public Vector3d zero { get { return new Vector3d(0, 0, 0); } }

    /// <summary>
    /// Creates a new vector at (0,0,0).
    /// </summary>
    public Vector3d() : this(0, 0, 0) { }

    /// <summary>
    /// Creates a new vector with X, Y, & Z all equal to the same value.
    /// </summary>
    /// <param name="value"></param>
    public Vector3d(double value) : this(value, value, value) { }

    /// <summary>
    /// Creates a new vector at choosen coordinates.
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <param name="z"></param>
    /// <param name="magnatude"></param>
    public Vector3d(double x, double y, double z)
    {
        axis[0] = x;
        axis[1] = y;
        axis[2] = z;
    }

    public double this[int index]
    {
        get
        {
            return axis[index];
        }
        set
        {
            axis[index] = value;
        }
    }
    [SerializeField]
    private double[] axis = new double[3] { 0, 0, 0 };

    /// <summary>
    /// Creates a vector, duplicating the coordinates of a Unity vector.
    /// </summary>
    /// <param name="vector"></param>
    public Vector3d(Vector3 vector)
    {
        this.x = vector.x;
        this.y = vector.y;
        this.z = vector.z;
    }

    /// <summary>
    /// Creates vector at the current positon of another vector.
    /// </summary>
    /// <param name="vector"></param>
    public Vector3d(Vector3d vector)
    {
        this.x = vector.x;
        this.y = vector.y;
        this.z = vector.z;
    }

    /// <summary>
    /// Converts Vector3d to Unity Vector3. Some precision may be lost.
    /// </summary>
    public Vector3 Vec3
    {
        get
        {
            return new Vector3((float)x, (float)y, (float)z);
        }
        set
        {
            x = value.x;
            y = value.y;
            z = value.z;
        }
    }

    // Position
    public double x
    {
        set { axis[0] = value; }
        get { return axis[0]; }
    }
    public double y
    {
        set { axis[1] = value; }
        get { return axis[1]; }
    }
    public double z
    {
        set { axis[2] = value; }
        get { return axis[2]; }
    }

    /// <summary>
    /// Sets this vector's x, y, and z components.
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <param name="z"></param>
    public void Set(double x, double y, double z)
    {
        this.x = x;
        this.y = y;
        this.z = z;
    }

    /// <summary>
    /// Sets this vector's x, y, and z components.
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <param name="z"></param>
    public void Set(Vector3d source)
    {
        this.x = source.x;
        this.y = source.y;
        this.z = source.z;
    }
    // ### Properties ###

    /// <summary>
    /// Returns the length of this vector. 
    /// </summary>
    public double magnitude
    {
        get { return Math.Sqrt((x * x) + (y * y) + (z * z)); }
    }

    /// <summary>
    /// Returns this vector with a magnitude of 1.
    /// </summary>
    public Vector3d normalized
    {
        get { return this / magnitude; }
    }

    /// <summary>
    /// Returns the squred length of this vector. 
    /// </summary>
    public double sqrMagnitude
    {
        get { return ((x * x) + (y * y) + (z * z)); }
    }

// ### Static Methods ###

    /// <summary>
    /// Returns the smallest angle between two points.
    /// </summary>
    /// <returns></returns>
    public static double Angle(Vector3d from, Vector3d to)
    {
        // sqrt(a) * sqrt(b) = sqrt(a * b) -- valid for real numbers
        double denominator = Math.Sqrt(from.sqrMagnitude * to.sqrMagnitude);
        if (denominator < kEpsilonNormalSqrt)
            return 0.0;

        double dot = Mathd.Clamp(Dot(from, to) / denominator, -1.0, 1.0);
        return Math.Acos(dot) * Mathd.Rad2Deg;
    }


    /// <summary>
    /// Returns a copy of the vector with it's magnitude limited to a specific radius.
    /// </summary>
    /// <returns></returns>
    static public Vector3d ClampMagnitude(Vector3d vec, double radius)
    {
        double magnitude = vec.magnitude;

        // If magnitude is within the radius, return the original vector (no further calculations needed).
        if (magnitude <= radius) return new Vector3d(vec);

        // Multiply the vector by the fraction of it's magnitude to radius.
        return vec * (radius / magnitude);
    }

    /// <summary>
    /// Returns a cross product of two vectors.
    /// </summary>
    /// <param name="a"></param>
    /// <param name="b"></param>
    /// <returns></returns>
    static public Vector3d Cross(Vector3d a, Vector3d b)
    {
        double x = (a.y * b.z) - (a.z * b.y);
        double y = (a.z * b.x) - (a.x * b.z);
        double z = (a.x * b.y) - (a.y * b.x);
        return new Vector3d(x, y, z);
    }

    /// <summary>
    /// Returns distance between this vector and another one.
    /// </summary>
    /// <returns></returns>
    static public double Distance(Vector3d vecA, Vector3d vecB)
    {
        return Math.Sqrt(DistanceSqr(vecA, vecB));
    }

    static public double DistanceSqr(Vector3d vecA, Vector3d vecB)
    {
        double xPow = Math.Pow(vecA.x - vecB.x, 2);
        double yPow = Math.Pow(vecA.y - vecB.y, 2);
        double zPow = Math.Pow(vecA.z - vecB.z, 2);
        return xPow + yPow + zPow;
    }

    /// <summary>
    /// Returns the Dot product between two vectors.
    /// </summary>
    /// <param name="from"></param>
    /// <param name="to"></param>
    /// <returns></returns>
    private static double Dot(Vector3d from, Vector3d to)
    {
        return from.x * to.x + from.y * to.y + from.z * to.z;
    }

    //
    /// <summary>
    /// Linearly interpolates between two vectors. The resulting vector is restricted to a radius of [0,1].
    /// </summary>
    /// <returns></returns>
    public Vector3d Lerp(Vector3d from, Vector3d target, double distance)
    {
        return LerpUnclamped(from, target, Mathd.Clamp(distance, 0, 1));
    }

    /// <summary>
    /// Linearly interpolates between two vectors.
    /// </summary>
    /// <returns></returns>
    public static Vector3d LerpUnclamped(Vector3d from, Vector3d target, double distance)
    {
        double rx, ry, rz; //resulting axes 

        rx = (target.x - from.x) * distance + from.x;
        ry = (target.y - from.y) * distance + from.y;
        rz = (target.z - from.z) * distance + from.z;
        return new Vector3d(rx, ry, rz);
    }

    /// <summary>
    /// Returns a vector made from the largest components of two vectors.
    /// </summary>
    /// <returns></returns>
    public static Vector3d Max(Vector3d vecA, Vector3d vecB)
    {
        return new Vector3d(Math.Max(vecA.x, vecB.x), Math.Max(vecA.y, vecB.y), Math.Max(vecA.z, vecB.z));
    }

    /// <summary>
    /// Returns a vector mode from the smalest components of two vectors.
    /// </summary>
    /// <param name="vecA"></param>
    /// <param name="vecB"></param>
    /// <returns></returns>
    public static Vector3d Min(Vector3d vecA, Vector3d vecB)
    {
        return new Vector3d(Math.Min(vecA.x, vecB.x), Math.Min(vecA.y, vecB.y), Math.Min(vecA.z, vecB.z));
    }

    /// <summary>
    /// Moves a vector towards a point.
    /// </summary>
    /// <param name="from"></param>
    /// <param name="to"></param>
    /// <param name="maxDistance"></param>
    /// <returns></returns>
    public static Vector3d MoveTowards(Vector3d from, Vector3d to, double maxDistance)
    {
        // Limits distance via ClampMagnitude.
        return from + ClampMagnitude(to, maxDistance);
    }

    /// <summary>
    /// Normalizes this vector.
    /// </summary>
    public void Normalize()
    {
        Set(this.normalized);
    }


 // ### Operator Functions ###

    public static Vector3d operator+ (Vector3d from, Vector3d target)
    {
        return new Vector3d(from.x + target.x, from.y + target.y, from.z + target.z);
    }
    
    public static Vector3d operator- (Vector3d from, Vector3d target)
    {
        return new Vector3d(from.x - target.x, from.y - target.y, from.z - target.z);
    }

    public static Vector3d operator* (Vector3d from, Vector3d target)
    {
        return new Vector3d(from.x * target.x, from.y * target.y, from.z * target.z);
    }

    public static Vector3d operator* (Vector3d from, double multipler)
    {
        return new Vector3d(from.x * multipler, from.y * multipler, from.z * multipler);
    }

    public static Vector3d operator/ (Vector3d dividend, Vector3d divisor)
    {
        return new Vector3d(dividend.x / divisor.x, dividend.y / divisor.y, dividend.z / divisor.z);
    }

    public static Vector3d operator/ (Vector3d dividend, double divisor)
    {
        return new Vector3d(dividend.x / divisor, dividend.y / divisor, dividend.z / divisor);
    }

 // ### Comparator Functions ###

    public static bool operator== (Vector3d vecA, Vector3d vecB)
    {
        if (object.ReferenceEquals(vecA, null))
        {
            return object.ReferenceEquals(vecB, null);
        }
        return vecA.Equals(vecB);
    }

    public static bool operator !=(Vector3d vecA, Vector3d vecB)
    {
        return !vecA.Equals(vecB);
    }

    public override string ToString() {
        return String.Format("({0},{1},{2})", axis[0], axis[1], axis[2]);
    }

    public override bool Equals(object obj)
    {
        if (obj is Vector3d)
        {
            Vector3d target = (Vector3d)obj;
            return (x == target.x && y == target.y && z == target.z);
        }
        return false;
    }

    public override int GetHashCode()
    {
        var hashCode = 373119288;
        hashCode = hashCode * -1521134295 + x.GetHashCode();
        hashCode = hashCode * -1521134295 + y.GetHashCode();
        hashCode = hashCode * -1521134295 + z.GetHashCode();
        return hashCode;
    }
}
