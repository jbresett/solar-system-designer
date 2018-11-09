using UnityEngine;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;
using System;

public class EllipseTests {

    /// <summary>
    /// Basic Cardinal Direction Tests.
    /// </summary>
    [Test]
    public void Cardinal() 
    {
        Ellipse e = new Ellipse(100, 50);
        Vector3d pt;

        pt = e.calcPoint(0);
        Assert.AreEqual(50, Math.Round(pt.x));
        Assert.AreEqual(0, Math.Round(pt.y));

        pt = e.calcPoint(90);

        Assert.AreEqual(0, Math.Round(pt.x));
        Assert.AreEqual(25, Math.Round(pt.y));

        pt = e.calcPoint(180);
        Assert.AreEqual(-50, Math.Round(pt.x));
        Assert.AreEqual(0, Math.Round(pt.y));

        pt = e.calcPoint(270);
        Assert.AreEqual(0, Math.Round(pt.x));
        Assert.AreEqual(-25, Math.Round(pt.y));
    }

    // A UnityTest behaves like a coroutine in PlayMode
    // and allows you to yield null to skip a frame in EditMode
    [UnityTest]
    public IEnumerator EllipseTestsWithEnumeratorPasses() {
        // Use the Assert class to test conditions.
        // yield to skip a frame
        yield return null;
    }
}
