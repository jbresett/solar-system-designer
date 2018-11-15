using UnityEngine;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using System;

public class NBodyTests {

    private Body Sun;
    private Body Earth;
    private Body Moon;

    public void Setup()
    {
        NBody System = new NBody();
        Sun = new Body(System, "Sun", BodyType.Sun, null, 0, 0, 0);

        Orbit earthOrbit = new Orbit("Sun", new Ellipse(100, 50), 365);
        Earth = new Body(System, "Earth", BodyType.Planet, earthOrbit, 0, 0, 0);

        Orbit moonOrbit = new Orbit("Earth", new Ellipse(10.0, 5.0), 30.0);
        Moon = new Body(System, "Moon", BodyType.Moon, moonOrbit, 0, 0, 0);
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

    // A UnityTest behaves like a coroutine in PlayMode
    // and allows you to yield null to skip a frame in EditMode
    [UnityTest]
    public IEnumerator NBodyTestsWithEnumeratorPasses() {
        // Use the Assert class to test conditions.
        // yield to skip a frame
        yield return null;
    }

}
