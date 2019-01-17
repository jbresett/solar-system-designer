using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Double Precision Math Functions.
/// 
/// Partial code based on Unity's Mathf: https://github.com/Unity-Technologies/UnityCsReference/blob/master/Runtime/Export/Mathf.cs
/// </summary>
public class Mathd {

    // Degrees-to-radians conversion constant (RO).
    public const double Deg2Rad = Math.PI * 2.0 / 360.0;

    // Radians-to-degrees conversion constant (RO).
    public const double Rad2Deg = 1.0 / Deg2Rad;

    /// <summary>
    /// Clamps a value between a minimum float and maximum float value.
    /// </summary>
    public static double Clamp(double value, double min, double max)
    {
        if (value < min) return min;
        if (value > max) return max;
        return value;
    }

    /// <summary>
    /// Moves from a point to a destination, up to a maximum distance away from the start.
    /// </summary>
    /// <returns></returns>
    public static double MoveTowards(double start, double destination, double maxDistance)
    {
        double distance = Math.Abs(destination - start);
        if (distance <= maxDistance) return destination;
        return start + Math.Sign(destination - start) * maxDistance;
    }

}