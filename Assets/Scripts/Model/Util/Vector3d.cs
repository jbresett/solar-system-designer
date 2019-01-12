using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class Vector3d
{
    /// <summary>
    /// Creates a new vector at (0,0,0).
    /// </summary>
    public Vector3d() : this(0, 0, 0) { }

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
    public Vector3 Vec3 {
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
    public double x {
        set { axis[0] = value; }
        get { return axis[0]; }
    }
    public double y {
        set { axis[1] = value; }
        get { return axis[1]; }
    }
    public double z {
        set { axis[2] = value; }
        get { return axis[2]; }
    }

    public double magnatude { 
        get
        {
            return Math.Sqrt(x * x + y * y + z * z);
        }
    }

    /// <summary>
    /// Moves the vector to another position.
    /// </summary>
    /// <param name="vector"></param>
    public void moveTo(Vector3d vector)
    {
        x = vector.x;
        y = vector.y;
        z = vector.z;
    }


     /// <summary>
    /// Returns distance between this vector and another one.
    /// </summary>
    /// <param name="target"></param>
    /// <returns></returns>
    public double getDistance(Vector3d target)
    {
        double xPow = Math.Pow(x - target.x, 2);
        double yPow = Math.Pow(y - target.y, 2);
        double zPow = Math.Pow(z - target.z, 2);
        return Math.Sqrt(xPow + yPow + zPow);
    }

    //
    /// <summary>
    /// Linearly interpolates between two vectors.
    /// </summary>
    /// <param name="target">Target Vector.</param>
    /// <param name="distance">Fractional distance to target. Negative values will calculate in the opposite
    ///     direciton. Values over 1 will result in positions past the target.</param>
    /// <returns></returns>
    public Vector3d Leap(Vector3d target, double distance)
    {
        double rx, ry, rz; //resulting axes 

        rx = (target.x - x) * distance + x;
        ry = (target.y - y) * distance + y;
        rz = (target.z - z) * distance + z;
        return new Vector3d(rx, ry, rz);
    }

    /// <summary>
    /// Returns a resulting vector of adding another vector to the current one.
    /// </summary>
    /// <param name="target"></param>
    /// <returns></returns>
    public Vector3d add(Vector3d target)
    {
        return new Vector3d(x + target.x, y + target.y, z + target.z);
    }

    /// <summary>
    /// Returns the resulting vector of subtracting another vector from the current one.
    /// </summary>
    /// <param name="vector3d"></param>
    /// <returns></returns>
    public Vector3d subtract(Vector3d target)
    {
        return new Vector3d(x - target.x, y - target.y, z - target.z);
    }

    public override string ToString() {
        return String.Format("({0},{1},{2})", x, y, z);
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
}
