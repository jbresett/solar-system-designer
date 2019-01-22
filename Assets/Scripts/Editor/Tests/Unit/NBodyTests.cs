using UnityEngine;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using System;

/// <summary>
/// This class is used to test the nbody system class in order
/// to verify an nbody collection can be properly created
/// </summary>
#pragma warning disable CS0618 // Type or member is obsolete
public class NBodyTests {

    private Body Earth;
    private Body Moon;

    public void Setup()
    {
        Body Sun = new Body("Sun", BodyType.Sun, null, 0, 0, 0);

        Orbit earthOrbit = new Orbit("Sun", new Ellipse(100, 50), 365);
        Earth = new Body("Earth", BodyType.Planet, earthOrbit, 0, 0, 0);

        Orbit moonOrbit = new Orbit("Earth", new Ellipse(10.0, 5.0), 30.0);
        Moon = new Body("Moon", BodyType.Moon, moonOrbit, 0, 0, 0);
    }

    /// <summary>
    /// Basic Position over time tests using relative positioning.
    /// </summary>
    [Test]
    public void PositionOverTime()
    {
        Setup();

        // For {time, x, y}, Expected (x,y) coordinates at each given time interval.
        double[,] time = new double[,] { { 0, 50, 0 }, { 91.25, 0, 25 }, { 182.5, -50, 0 }, { 273.75, 0, -25 }, { 365.0, 50, 0 } };

        for (int i = 0; i < time.GetLength(0); i++)
        {
            Vector3d Position = Earth.GetPosition(time[i, 0]);
            Assert.AreEqual(time[i, 1], Math.Round(Position.x));
            Assert.AreEqual(time[i, 2], Math.Round(Position.y));
        }

    }

    /// <summary>
    /// A UnityTest behaves like a coroutine in PlayMode
    /// and allows you to yield null to skip a frame in EditMode
    /// </summary>
    [UnityTest]
    public IEnumerator NBodyTestsWithEnumeratorPasses() {
        // Use the Assert class to test conditions.
        // yield to skip a frame
        yield return null;
    }

}
