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
        
        // Null Name
        try
        {
            new Body(null, BodyType.Astroid, null, new Vector3d(), 0.0, 0.0);
            Assert.Fail();
        } catch (System.ArgumentException) { /* Assert Pased */ }

        // Null Ellipse
        try
        {
            new Body("TheMoon", BodyType.Astroid, null, null, 0.0, 0.0);
            Assert.Fail();
        }
        catch (System.ArgumentException) { /* Assert Pased */ }

    }

    [Test]
    public void BodyDistances()
    {
        Body b1 = new Body("Earth", BodyType.Planet, null, new Vector3d(), 0.0, 0.0);
        Body b2 = new Body("Moon", BodyType.Moon, null, new Vector3d(), 0.0, 0.0);
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
            b1.Ellipse.moveTo(Vectors[i]);
            b2.Ellipse.moveTo(Vectors[i + 1]);
            // TODO: Update based on physics calculations.
            //Assert.AreEqual(b1.getDistance(b2), Distance[i]);
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
