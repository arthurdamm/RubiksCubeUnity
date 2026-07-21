using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools.Utils;

public class FindMatchingDirectionTests
{
    [Test]
    public void TestMatchingDirections_FindsPositive()
    {
        Camera camera = new Camera();
        GameObject go = new GameObject();
        go.AddComponent<CubeBuilder>();
        CubeBuilder cubeBuilder = go.GetComponent<CubeBuilder>();
        var directionsResolver = new CubeDirectionsResolver(go.transform);
        var comparer = new Vector3EqualityComparer(10e-6f);
        
        Vector3[] candidates = new[] { Vector3.forward, Vector3.up, Vector3.right };
        var expected = Vector3.up;
        var actual = directionsResolver.FindMatchingDirection(expected, candidates);
        
        Assert.That(actual, Is.EqualTo(expected).Using(comparer));
    }
    
    [Test]
    public void TestMatchingDirections_FindsNegative()
    {
        Camera camera = new Camera();
        GameObject go = new GameObject();
        go.AddComponent<CubeBuilder>();
        CubeBuilder cubeBuilder = go.GetComponent<CubeBuilder>();
        var directionsResolver = new CubeDirectionsResolver(go.transform);
        var comparer = new Vector3EqualityComparer(10e-6f);
        
        Vector3[] candidates = new[] { Vector3.forward, Vector3.up, Vector3.right };
        var expected = -Vector3.up;
        var actual = directionsResolver.FindMatchingDirection(expected, candidates);
        
        Assert.That(actual, Is.EqualTo(expected).Using(comparer));
    }
    
    [Test]
    public void TestMatchingDirections_FindsNegativeOffset()
    {
        Camera camera = new Camera();
        GameObject go = new GameObject();
        go.AddComponent<CubeBuilder>();
        CubeBuilder cubeBuilder = go.GetComponent<CubeBuilder>();
        var directionsResolver = new CubeDirectionsResolver(go.transform);
        var comparer = new Vector3EqualityComparer(10e-6f);
        
        Vector3[] candidates = new[] { Vector3.forward, Vector3.up, Vector3.right };
        var expected = -Vector3.up;
        var actual = directionsResolver.FindMatchingDirection(new Vector3(0.1f, -.9f, 0.1f), candidates);
        
        Assert.That(actual, Is.EqualTo(expected).Using(comparer));
    }
}
