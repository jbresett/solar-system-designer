using UnityEngine;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;
using System;
using System.Collections.Generic;

#pragma warning disable CS0618 // Type or member is obsolete
public class BodyTests: IPrebuildSetup {
    private Body Sun;
    private Body Earth;

    public void Setup()
    {
        NBody System = new NBody();

        Sun = new Body(System, "Sun", BodyType.Sun, null, 0, 0, 0);

        Orbit earthOrbit = new Orbit("Sun", new Ellipse(100, 50), 365);
        Earth = new Body(System, "Earth", BodyType.Planet, earthOrbit, 0, 0, 0);
    }

    /// <summary>
    /// Tests the degrees at any given time.
    /// </summary>
    [Test]
    public void DegreeOverTime()
    {
        Setup();

        // Expected degrees at each given time interval.
        double[,] time = new double[,] { { 0, 0 }, { 91.25, 90}, { 182.5, 180 }, { 273.75, 270 }, { 365.0, 0 }};
        // Initial position at 0 days should be 1/2 X-Axis.
        for (int i = 0; i < time.GetLength(0); i++) {
            double degree = Earth.Orbits.getDegree(time[i,0]);
            Assert.AreEqual(time[i,1], Math.Round(degree));
        }

    }

    [Test]
    public void Serial()
    {
        NBody System = new NBody();

        Sun = new Body(System, "Sun", BodyType.Sun, null, 0, 0, 0);
        Orbit earthOrbit = new Orbit("Sun", new Ellipse(156, 146), 365);
        Earth = new Body(System, "Earth", BodyType.Planet, earthOrbit, 5.972e+24, 6371.0, 1.0);

        Debug.Log("Sun JSON: " + Sun.ToJson());
        Debug.Log("Earth JSON: " + Earth.ToJson());
    }

    // A UnityTest behaves like a coroutine in PlayMode
    // and allows you to yield null to skip a frame in EditMode
    [UnityTest]
    public IEnumerator BodyTestsWithEnumeratorPasses() {
        // Use the Assert class to test conditions.
        // yield to skip a frame
        yield return null;
    }
}
