using UnityEngine;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;

public class Vector3dTest {

    [Test]
    public void Vector3dSimplePasses() {
        // Use the Assert class to test conditions.
    }

    /// <summary>
    /// Simple Leap test.
    /// </summary>
    [Test]
    public void Leap()
    {
        Vector3d a = new Vector3d(1, 5, 5);
        Vector3d b = new Vector3d(2, -10, 15);

        Assert.AreEqual(a.Leap(b, 0), a);
        Assert.AreEqual(a.Leap(b, 1), b);
        Assert.AreEqual(a.Leap(b, 0.75), b.Leap(a, 0.25));
    }

    // A UnityTest behaves like a coroutine in PlayMode
    // and allows you to yield null to skip a frame in EditMode
    [UnityTest]
    public IEnumerator Vector3dWithEnumeratorPasses() {
        // Use the Assert class to test conditions.
        // yield to skip a frame
        yield return null;
    }
}
