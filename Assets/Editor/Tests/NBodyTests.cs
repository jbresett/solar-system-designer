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
            new Body(null, BodyType.Astroid, null, 0.0, 0.0, 0.0, null);
            Assert.Fail();
        } catch (System.ArgumentException) { /* Assert Pased */ }

    }

    [Test]
    public void BodyDistances()
    {
        Body earth = new Body("Earth", BodyType.Planet, null, 0, 0, 0, null);

        Orbit moonOrbit = new Orbit(earth, new Ellipse(10.0, 5.0), 30.0);
        Body moon = new Body("Moon", BodyType.Moon, moonOrbit, 0, 0, 0, null);
        
        //TODO: Build tests once Ellipse class can calculate x/y locations.
        
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
