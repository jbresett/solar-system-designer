using UnityEngine;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;

public class NBodyTests {

    [Test]
    public void BodyMustHaveValidArguments() {
        
        // Each of the following new bodies should throw the exception.  If no exception
        // is thrown, the test fails.
        try
        {
            new Body(null);
            Assert.Fail();
        } catch (System.ArgumentException) { /* Assert Pased */ }

        try
        {
            new Body("");
            Assert.Fail();
        }
        catch (System.ArgumentException) { /* Assert Pased */ }

        try
        {
            new Body("Test", BodyType.Moon, null);
            Assert.Fail();
        }
        catch (System.ArgumentException) { /* Assert Pased */ }

    }

    [Test]
    public void BodyDistances()
    {
        Body b1 = new Body("Earth", BodyType.Planet);
        Body b2 = new Body("Moon", BodyType.Moon);
        Vector3d[] Vectors = new Vector3d[] {
            new Vector3d(0, 0, 0),
            new Vector3d(0, 0, 0),
            new Vector3d(3, 4, 0),
            new Vector3d(-3, -4, 0)
        };
        double[] Distance = new double[]
        {
            0.0, 5.0, 10.0
        };

        for (int i = 0; i < Vectors.Length - 1; i++)
        {
            b1.Position.moveTo(Vectors[i]);
            b2.Position.moveTo(Vectors[i + 1]);
            Assert.AreEqual(b1.getDistance(b2), Distance[i]);
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
