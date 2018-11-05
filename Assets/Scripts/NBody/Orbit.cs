using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Orbit {

    public Body Around { get; set; }
    public Vector3d Ellipse { get; set; }
    public double RevolutionTime { get; set; }

    /// <summary>
    /// Calculates radian based on time in orbit.
    /// </summary>
    /// <param name="days">Number of days from initial starting position.</param>
    public double getRadian(double days)
    {
        // Gets # of turns based on time of a single revolution.
        double turns = days / RevolutionTime;

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
        double turns = days / RevolutionTime;

        // Percent of Turn
        double turn = turns - Math.Truncate(turns);

        // 1 Full turn = 2pi
        return turn * 360;
    }
}
