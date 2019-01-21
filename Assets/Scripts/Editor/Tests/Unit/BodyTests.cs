using UnityEngine;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;
using System;
using System.Collections.Generic;

#pragma warning disable CS0618 // Type or member is obsolete
public class BodyTests {
    private Body Sun;
    private Body Earth;

    /// <summary>
    /// Tests the degrees at any given time.
    /// </summary>
    [Test]
    public void DegreeOverTime()
    {

        // Expected degrees at each given time interval.
        double[,] time = new double[,] { { 0, 0 }, { 91.25, 90}, { 182.5, 180 }, { 273.75, 270 }, { 365.0, 0 }};
        // Initial position at 0 days should be 1/2 X-Axis.
        for (int i = 0; i < time.GetLength(0); i++) {
            double degree = Earth.Orbits.getDegree(time[i,0]);
            Assert.AreEqual(time[i,1], Math.Round(degree));
        }

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
