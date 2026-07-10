using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools.Utils;

public class RayCasterTests
{
    [Test]
    public void TestMatchingDirections()
    {
        Camera camera = new Camera();
        CubeRayCaster cubeRayCaster = new CubeRayCaster(camera);
        var comparer = new Vector3EqualityComparer(10e-6f);
        
        Vector3[] candidates = new[] { Vector3.forward, Vector3.up, Vector3.right };
        var expected = Vector3.up;
        var actual = cubeRayCaster.FindMatchingDirection(expected, candidates);
        
        Assert.That(actual, Is.EqualTo(expected).Using(comparer));
    }
}
